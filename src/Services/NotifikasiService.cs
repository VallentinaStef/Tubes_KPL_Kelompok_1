using Tubes_KPL_Kelompok_1.Config;
using Tubes_KPL_Kelompok_1.Models;
using Tubes_KPL_Kelompok_1.src.Utils;

namespace Tubes_KPL_Kelompok_1.Services
{
    public class NotifikasiService
    {
        private readonly List<Notifikasi> _notifikasiList = new();
        private int _nextId = 1;

        public Response<Notifikasi> KirimNotifikasi(string userId, string judul, string pesan, TipeNotifikasi tipe)
        {
            var config = NotifikasiConfig.Instance;
            if (!config.NotifikasiAktif)
                return new Response<Notifikasi> { Status = false, Message = "Sistem notifikasi sedang tidak aktif." };

            var notif = new Notifikasi
            {
                Id = _nextId++,
                UserId = userId,
                Judul = judul,
                Pesan = pesan,
                Tipe = tipe,
                WaktuKirim = DateTime.Now,
                SudahDibaca = false
            };

            _notifikasiList.Add(notif);
            Console.WriteLine($"[Notifikasi] [{tipe}] -> {judul}: {pesan}");
            return new Response<Notifikasi> { Status = true, Data = notif, Message = "Notifikasi berhasil dikirim." };
        }

        public Response<List<Notifikasi>> GetNotifikasi(string userId)
        {
            var hasil = _notifikasiList.Where(n => n.UserId == userId).ToList();
            return new Response<List<Notifikasi>> { Status = true, Data = hasil, Message = $"Ditemukan {hasil.Count} notifikasi." };
        }

        public Response<bool> TandaiBaca(int notifId)
        {
            var notif = _notifikasiList.FirstOrDefault(n => n.Id == notifId);
            if (notif == null)
                return new Response<bool> { Status = false, Message = "Notifikasi tidak ditemukan." };

            notif.SudahDibaca = true;
            return new Response<bool> { Status = true, Data = true, Message = "Notifikasi ditandai sudah dibaca." };
        }

        public void TampilkanSemua(string userId)
        {
            var list = _notifikasiList.Where(n => n.UserId == userId).ToList();
            Console.WriteLine($"\n=== NOTIFIKASI untuk {userId} ===");
            if (!list.Any())
            {
                Console.WriteLine("  (tidak ada notifikasi)");
                return;
            }
            foreach (var n in list)
            {
                string status = n.SudahDibaca ? "[Dibaca]" : "[Baru]";
                Console.WriteLine($"  {status} [{n.Id}] [{n.Tipe}] {n.Judul} - {n.WaktuKirim:HH:mm}");
                Console.WriteLine($"    {n.Pesan}");
            }
            Console.WriteLine("================================\n");
        }
    }
}