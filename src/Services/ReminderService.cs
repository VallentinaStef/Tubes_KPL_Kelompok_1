using Tubes_KPL_Kelompok_1.Config;
using Tubes_KPL_Kelompok_1.Models;
using Tubes_KPL_Kelompok_1.src.Utils;

namespace Tubes_KPL_Kelompok_1.Services
{
    public class ReminderService
    {
        private readonly List<Reminder> _reminderList = new();
        private int _nextId = 1;
        private readonly NotifikasiService _notifikasiService;

        public ReminderService(NotifikasiService notifikasiService)
        {
            _notifikasiService = notifikasiService;
        }

        public Response<Reminder> TambahReminder(string namaPasien, string pesan, DateTime waktuReminder, string? konsultasiId = null)
        {
            var config = NotifikasiConfig.Instance;

            if (!config.ReminderAktif)
                return new Response<Reminder> { Status = false, Message = "Sistem reminder sedang tidak aktif." };

            var reminder = new Reminder
            {
                Id = _nextId++,
                NamaPasien = namaPasien,
                Pesan = pesan,
                WaktuReminder = waktuReminder,
                SudahTerkirim = false,
                KonsultasiId = konsultasiId
            };

            _reminderList.Add(reminder);
            Console.WriteLine($"[Reminder] Dijadwalkan untuk {namaPasien} pada {waktuReminder:dd/MM HH:mm}");
            return new Response<Reminder> { Status = true, Data = reminder, Message = "Reminder berhasil ditambahkan." };
        }

        public void CekDanKirimReminder()
        {
            var sekarang = DateTime.Now;
            var yangHarusDikirim = _reminderList
                .Where(r => !r.SudahTerkirim && r.WaktuReminder <= sekarang)
                .ToList();

            foreach (var reminder in yangHarusDikirim)
            {
                _notifikasiService.KirimNotifikasi(
                    reminder.NamaPasien,
                    "Reminder Konsultasi",
                    reminder.Pesan,
                    TipeNotifikasi.Reminder
                );
                reminder.SudahTerkirim = true;
                Console.WriteLine($"[Reminder] Terkirim ke {reminder.NamaPasien}: {reminder.Pesan}");
            }

            if (!yangHarusDikirim.Any())
                Console.WriteLine("[Reminder] Tidak ada reminder yang perlu dikirim saat ini.");
        }

        public void SimulasiKirimSemua()
        {
            Console.WriteLine("\n[Reminder] Simulasi kirim semua reminder...");
            foreach (var r in _reminderList.Where(r => !r.SudahTerkirim))
            {
                _notifikasiService.KirimNotifikasi(
                    r.NamaPasien,
                    "Reminder Konsultasi",
                    r.Pesan,
                    TipeNotifikasi.Reminder
                );
                r.SudahTerkirim = true;
            }
        }

        public void TampilkanSemuaReminder()
        {
            Console.WriteLine("\n=== DAFTAR REMINDER ===");
            if (!_reminderList.Any())
            {
                Console.WriteLine("  (belum ada reminder)");
                return;
            }
            foreach (var r in _reminderList)
            {
                string status = r.SudahTerkirim ? "[Terkirim]" : "[Menunggu]";
                Console.WriteLine($"  {status} | {r.NamaPasien} | {r.WaktuReminder:dd/MM HH:mm} | {r.Pesan}");
            }
            Console.WriteLine("=======================\n");
        }
    }
}