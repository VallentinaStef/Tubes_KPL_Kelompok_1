using Tubes_KPL_Kelompok_1.Config;
using Tubes_KPL_Kelompok_1.Models;
using Tubes_KPL_Kelompok_1.src.Utils;
using Tubes_KPL_Kelompok_1.States;

namespace Tubes_KPL_Kelompok_1.Services
{
    public class KonsultasiService
    {
        private readonly List<Konsultasi> _konsultasiList = new();
        private readonly Dictionary<int, KonsultasiStateMachine> _stateMachines = new();
        private int _nextId = 1;
        private readonly NotifikasiService _notifikasiService;
        private readonly ReminderService _reminderService;

        public KonsultasiService(NotifikasiService notifikasiService, ReminderService reminderService)
        {
            _notifikasiService = notifikasiService;
            _reminderService = reminderService;
        }

        public Response<Konsultasi> BuatKonsultasi(string namaPasien, string namaDokter, string keluhan, DateTime waktuRencana)
        {
            var config = NotifikasiConfig.Instance;

            if (!config.KonsultasiOnlineAktif)
                return new Response<Konsultasi> { Status = false, Message = "Layanan konsultasi online sedang tidak aktif." };

            var konsultasi = new Konsultasi
            {
                Id = _nextId++,
                NamaPasien = namaPasien,
                NamaDokter = namaDokter,
                Keluhan = keluhan,
                WaktuMulai = waktuRencana,
                Status = StatusKonsultasi.Menunggu
            };

            _konsultasiList.Add(konsultasi);
            _stateMachines[konsultasi.Id] = new KonsultasiStateMachine();

            _notifikasiService.KirimNotifikasi(
                namaPasien,
                "Konsultasi Terdaftar",
                $"Konsultasi dengan Dr. {namaDokter} pada {waktuRencana:dd/MM/yyyy HH:mm} berhasil dibuat.",
                TipeNotifikasi.LayananJadwal
            );

            _reminderService.TambahReminder(
                namaPasien,
                $"Konsultasi dengan Dr. {namaDokter} {config.MenitSebelumReminder} menit lagi!",
                waktuRencana.AddMinutes(-config.MenitSebelumReminder),
                konsultasi.Id.ToString()
            );

            return new Response<Konsultasi> { Status = true, Data = konsultasi, Message = "Konsultasi berhasil dibuat." };
        }

        public Response<Konsultasi> MulaiKonsultasi(int konsultasiId)
        {
            var konsultasi = _konsultasiList.FirstOrDefault(k => k.Id == konsultasiId);
            if (konsultasi == null)
                return new Response<Konsultasi> { Status = false, Message = "Konsultasi tidak ditemukan." };

            var sm = _stateMachines[konsultasiId];
            if (!sm.Mulai())
                return new Response<Konsultasi> { Status = false, Message = $"Tidak bisa mulai dari status: {konsultasi.Status}" };

            konsultasi.Status = sm.State;
            konsultasi.WaktuMulai = DateTime.Now;

            _notifikasiService.KirimNotifikasi(
                konsultasi.NamaPasien,
                "Konsultasi Dimulai",
                $"Sesi konsultasi dengan Dr. {konsultasi.NamaDokter} telah dimulai.",
                TipeNotifikasi.LayananJadwal
            );

            return new Response<Konsultasi> { Status = true, Data = konsultasi, Message = "Konsultasi dimulai." };
        }

        public Response<Konsultasi> SelesaikanKonsultasi(int konsultasiId, string catatanDokter)
        {
            var config = NotifikasiConfig.Instance;
            var konsultasi = _konsultasiList.FirstOrDefault(k => k.Id == konsultasiId);
            if (konsultasi == null)
                return new Response<Konsultasi> { Status = false, Message = "Konsultasi tidak ditemukan." };

            var sm = _stateMachines[konsultasiId];
            if (!sm.Selesaikan())
                return new Response<Konsultasi> { Status = false, Message = $"Tidak bisa selesaikan dari status: {konsultasi.Status}" };

            konsultasi.WaktuSelesai = DateTime.Now;
            var durasi = (konsultasi.WaktuSelesai - konsultasi.WaktuMulai)?.TotalMinutes ?? 0;

            if (durasi > config.DurasiMaksKonsultasiMenit)
                Console.WriteLine($"[Konsultasi] Durasi melebihi batas: {durasi:F0} menit (maks {config.DurasiMaksKonsultasiMenit})");

            konsultasi.Status = sm.State;
            konsultasi.CatatanDokter = catatanDokter;

            _notifikasiService.KirimNotifikasi(
                konsultasi.NamaPasien,
                "Konsultasi Selesai",
                $"Terima kasih. Catatan: {catatanDokter}",
                TipeNotifikasi.LayananJadwal
            );

            return new Response<Konsultasi> { Status = true, Data = konsultasi, Message = "Konsultasi selesai." };
        }

        public Response<Konsultasi> BatalkanKonsultasi(int konsultasiId)
        {
            var konsultasi = _konsultasiList.FirstOrDefault(k => k.Id == konsultasiId);
            if (konsultasi == null)
                return new Response<Konsultasi> { Status = false, Message = "Konsultasi tidak ditemukan." };

            var sm = _stateMachines[konsultasiId];
            if (!sm.Batalkan())
                return new Response<Konsultasi> { Status = false, Message = $"Tidak bisa batalkan dari status: {konsultasi.Status}" };

            konsultasi.Status = sm.State;

            _notifikasiService.KirimNotifikasi(
                konsultasi.NamaPasien,
                "Konsultasi Dibatalkan",
                $"Konsultasi dengan Dr. {konsultasi.NamaDokter} telah dibatalkan.",
                TipeNotifikasi.LayananJadwal
            );

            return new Response<Konsultasi> { Status = true, Data = konsultasi, Message = "Konsultasi dibatalkan." };
        }

        public void TampilkanDaftarKonsultasi()
        {
            Console.WriteLine("\n=== DAFTAR KONSULTASI ===");
            if (!_konsultasiList.Any())
            {
                Console.WriteLine("  (belum ada konsultasi)");
                return;
            }
            foreach (var k in _konsultasiList)
            {
                string icon = k.Status switch
                {
                    StatusKonsultasi.Menunggu => "[Menunggu]",
                    StatusKonsultasi.Berlangsung => "[Berlangsung]",
                    StatusKonsultasi.Selesai => "[Selesai]",
                    StatusKonsultasi.Dibatalkan => "[Dibatalkan]",
                    _ => "?"
                };
                Console.WriteLine($"  {icon} [{k.Id}] {k.NamaPasien} - Dr.{k.NamaDokter} | {k.WaktuMulai:HH:mm}");
            }
            Console.WriteLine("=========================\n");
        }
    }
}