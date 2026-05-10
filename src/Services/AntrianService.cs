using Tubes_KPL_Kelompok_1.Config;
using Tubes_KPL_Kelompok_1.Models;
using Tubes_KPL_Kelompok_1.src.Utils;
using Tubes_KPL_Kelompok_1.States;

namespace Tubes_KPL_Kelompok_1.Services
{
    public class AntrianService
    {
        private readonly List<Antrian> _antrianList = new();
        private readonly Dictionary<int, AntrianStateMachine> _stateMachines = new();
        private int _nextId = 1;
        private readonly NotifikasiService _notifikasiService;

        public AntrianService(NotifikasiService notifikasiService)
        {
            _notifikasiService = notifikasiService;
        }

        public Response<Antrian> DaftarAntrian(string namaPasien, string poli)
        {
            var config = NotifikasiConfig.Instance;

            int jumlahAktif = _antrianList.Count(a => a.Poli == poli && a.Status == StatusAntrian.Menunggu);

            if (jumlahAktif >= config.MaksAntrianPerPoli)
                return new Response<Antrian> { Status = false, Message = $"Antrian {poli} sudah penuh ({config.MaksAntrianPerPoli} orang)." };

            int nomorUrut = _antrianList.Count(a => a.Poli == poli) + 1;
            string nomorAntrian = $"{poli[..Math.Min(3, poli.Length)].ToUpper()}-{nomorUrut:D3}";

            var antrian = new Antrian
            {
                Id = _nextId++,
                NomorAntrian = nomorAntrian,
                NamaPasien = namaPasien,
                Poli = poli,
                WaktuDaftar = DateTime.Now,
                Status = StatusAntrian.Menunggu,
                PosisiAntrian = jumlahAktif + 1
            };

            _antrianList.Add(antrian);
            _stateMachines[antrian.Id] = new AntrianStateMachine();

            _notifikasiService.KirimNotifikasi(
                namaPasien,
                "Antrian Terdaftar",
                $"Nomor antrian Anda: {nomorAntrian}. Posisi: {antrian.PosisiAntrian}",
                TipeNotifikasi.AntreanRealTime
            );

            return new Response<Antrian> { Status = true, Data = antrian, Message = "Berhasil mendaftar antrian." };
        }

        public Response<Antrian> PanggilAntrian(int antrianId)
        {
            var antrian = _antrianList.FirstOrDefault(a => a.Id == antrianId);
            if (antrian == null)
                return new Response<Antrian> { Status = false, Message = "Antrian tidak ditemukan." };

            var sm = _stateMachines[antrianId];
            if (!sm.Panggil())
                return new Response<Antrian> { Status = false, Message = $"Tidak bisa memanggil antrian dengan status: {antrian.Status}" };

            antrian.Status = sm.State;

            _notifikasiService.KirimNotifikasi(
                antrian.NamaPasien,
                "Giliran Anda!",
                $"Nomor {antrian.NomorAntrian} silakan masuk ke poli {antrian.Poli}.",
                TipeNotifikasi.AntreanRealTime
            );

            return new Response<Antrian> { Status = true, Data = antrian, Message = "Antrian dipanggil." };
        }

        public Response<Antrian> SelesaikanAntrian(int antrianId)
        {
            var antrian = _antrianList.FirstOrDefault(a => a.Id == antrianId);
            if (antrian == null)
                return new Response<Antrian> { Status = false, Message = "Antrian tidak ditemukan." };

            var sm = _stateMachines[antrianId];
            if (!sm.Selesaikan())
                return new Response<Antrian> { Status = false, Message = $"Tidak bisa selesaikan antrian dengan status: {antrian.Status}" };

            antrian.Status = sm.State;
            return new Response<Antrian> { Status = true, Data = antrian, Message = "Antrian selesai." };
        }

        public Response<Antrian> BatalkanAntrian(int antrianId)
        {
            var antrian = _antrianList.FirstOrDefault(a => a.Id == antrianId);
            if (antrian == null)
                return new Response<Antrian> { Status = false, Message = "Antrian tidak ditemukan." };

            var sm = _stateMachines[antrianId];
            if (!sm.Batalkan())
                return new Response<Antrian> { Status = false, Message = $"Tidak bisa batalkan antrian dengan status: {antrian.Status}" };

            antrian.Status = sm.State;
            return new Response<Antrian> { Status = true, Data = antrian, Message = "Antrian dibatalkan." };
        }

        public void TampilkanStatusAntrian(string poli)
        {
            var list = _antrianList.Where(a => a.Poli == poli).ToList();
            Console.WriteLine($"\n=== STATUS ANTRIAN POLI: {poli} ===");
            if (!list.Any())
            {
                Console.WriteLine("  (belum ada antrian)");
                return;
            }
            foreach (var a in list)
            {
                string icon = a.Status switch
                {
                    StatusAntrian.Menunggu => "[Menunggu]",
                    StatusAntrian.Dipanggil => "[Dipanggil]",
                    StatusAntrian.Selesai => "[Selesai]",
                    StatusAntrian.Dibatalkan => "[Dibatalkan]",
                    _ => "?"
                };
                Console.WriteLine($"  {icon} {a.NomorAntrian} - {a.NamaPasien}");
            }
            Console.WriteLine("===================================\n");
        }
    }
}