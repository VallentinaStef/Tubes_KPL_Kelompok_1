using Tubes_KPL_Kelompok_1.Config;
using Tubes_KPL_Kelompok_1.Models;
using Tubes_KPL_Kelompok_1.Services;

namespace Tubes_KPL_Kelompok_1.Modules.Notifikasi
{
    public class NotifikasiModule
    {
        private readonly NotifikasiService _notifikasiService;
        private readonly AntrianService _antrianService;
        private readonly KonsultasiService _konsultasiService;
        private readonly ReminderService _reminderService;
        private readonly NotifikasiConfig _config;

        public NotifikasiModule()
        {
            _config = NotifikasiConfig.Instance;
            _notifikasiService = new NotifikasiService();
            _reminderService = new ReminderService(_notifikasiService);
            _antrianService = new AntrianService(_notifikasiService);
            _konsultasiService = new KonsultasiService(_notifikasiService, _reminderService);
        }

        public void Jalankan()
        {
            bool lanjut = true;
            while (lanjut)
            {
                Console.WriteLine("\n===== MODUL NOTIFIKASI & KONSULTASI =====");
                Console.WriteLine("1. Kirim Notifikasi");
                Console.WriteLine("2. Lihat Notifikasi User");
                Console.WriteLine("3. Kelola Antrian Real-Time ");
                Console.WriteLine("4. Kelola Konsultasi Online ");
                Console.WriteLine("5. Kelola Reminder ");
                Console.WriteLine("6. Konfigurasi Runtime");
                Console.WriteLine("0. Kembali");
                Console.Write("Pilih: ");

                switch (Console.ReadLine())
                {
                    case "1": MenuKirimNotifikasi(); break;
                    case "2": MenuLihatNotifikasi(); break;
                    case "3": MenuAntrian(); break;
                    case "4": MenuKonsultasi(); break;
                    case "5": MenuReminder(); break;
                    case "6": MenuKonfigurasi(); break;
                    case "0": lanjut = false; break;
                    default: Console.WriteLine("Pilihan tidak valid."); break;
                }
            }
        }

        private void MenuKirimNotifikasi()
        {
            Console.Write("User ID: ");
            string userId = Console.ReadLine() ?? "";
            Console.Write("Judul: ");
            string judul = Console.ReadLine() ?? "";
            Console.Write("Pesan: ");
            string pesan = Console.ReadLine() ?? "";
            Console.WriteLine("Tipe (0=LayananJadwal, 1=AntreanRealTime, 2=Reminder): ");
            TipeNotifikasi tipe = (TipeNotifikasi)int.Parse(Console.ReadLine() ?? "0");

            var hasil = _notifikasiService.KirimNotifikasi(userId, judul, pesan, tipe);
            Console.WriteLine(hasil.Status ? $"Berhasil: {hasil.Message}" : $"Gagal: {hasil.Message}");
        }

        private void MenuLihatNotifikasi()
        {
            Console.Write("User ID: ");
            string userId = Console.ReadLine() ?? "";

            _notifikasiService.TampilkanSemua(userId);

            Console.Write("Tandai dibaca? Masukkan ID notifikasi (atau 0 untuk skip): ");
            int id = int.Parse(Console.ReadLine() ?? "0");
            if (id > 0)
            {
                var tandai = _notifikasiService.TandaiBaca(id);
                Console.WriteLine(tandai.Status ? tandai.Message : $"Gagal: {tandai.Message}");
            }
        }

        private void MenuAntrian()
        {
            bool lanjut = true;
            while (lanjut)
            {
                Console.WriteLine("\n-- ANTRIAN REAL-TIME --");
                Console.WriteLine("1. Daftar Antrian Baru");
                Console.WriteLine("2. Panggil Antrian (by ID)");
                Console.WriteLine("3. Selesaikan Antrian (by ID)");
                Console.WriteLine("4. Batalkan Antrian (by ID)");
                Console.WriteLine("5. Lihat Status Antrian per Poli");
                Console.WriteLine("0. Kembali");
                Console.Write("Pilih: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        Console.Write("Nama Pasien: ");
                        string nama = Console.ReadLine() ?? "";
                        Console.Write("Poli: ");
                        string poli = Console.ReadLine() ?? "";
                        var daftar = _antrianService.DaftarAntrian(nama, poli);
                        Console.WriteLine(daftar.Status
                            ? $"Berhasil! Nomor Antrian: {daftar.Data?.NomorAntrian}, Posisi: {daftar.Data?.PosisiAntrian}"
                            : $"Gagal: {daftar.Message}");
                        break;

                    case "2":
                        Console.Write("ID Antrian: ");
                        int idPanggil = int.Parse(Console.ReadLine() ?? "0");
                        var panggil = _antrianService.PanggilAntrian(idPanggil);
                        Console.WriteLine(panggil.Status
                            ? $"Memanggil: {panggil.Data?.NomorAntrian} - {panggil.Data?.NamaPasien}"
                            : $"Gagal: {panggil.Message}");
                        break;

                    case "3":
                        Console.Write("ID Antrian: ");
                        int idSelesai = int.Parse(Console.ReadLine() ?? "0");
                        var selesai = _antrianService.SelesaikanAntrian(idSelesai);
                        Console.WriteLine(selesai.Status ? selesai.Message : $"Gagal: {selesai.Message}");
                        break;

                    case "4":
                        Console.Write("ID Antrian: ");
                        int idBatal = int.Parse(Console.ReadLine() ?? "0");
                        var batal = _antrianService.BatalkanAntrian(idBatal);
                        Console.WriteLine(batal.Status ? batal.Message : $"Gagal: {batal.Message}");
                        break;

                    case "5":
                        Console.Write("Nama Poli: ");
                        string namaP = Console.ReadLine() ?? "";
                        _antrianService.TampilkanStatusAntrian(namaP);
                        break;

                    case "0": lanjut = false; break;
                    default: Console.WriteLine("Pilihan tidak valid."); break;
                }
            }
        }

        private void MenuKonsultasi()
        {
            bool lanjut = true;
            while (lanjut)
            {
                Console.WriteLine("\n-- KONSULTASI ONLINE --");
                Console.WriteLine("1. Buat Konsultasi Baru");
                Console.WriteLine("2. Mulai Konsultasi");
                Console.WriteLine("3. Selesaikan Konsultasi");
                Console.WriteLine("4. Batalkan Konsultasi");
                Console.WriteLine("5. Lihat Daftar Konsultasi");
                Console.WriteLine("0. Kembali");
                Console.Write("Pilih: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        Console.Write("Nama Pasien: ");
                        string pasien = Console.ReadLine() ?? "";
                        Console.Write("Nama Dokter: ");
                        string dokter = Console.ReadLine() ?? "";
                        Console.Write("Keluhan: ");
                        string keluhan = Console.ReadLine() ?? "";
                        Console.Write("Waktu Rencana (dd/MM/yyyy HH:mm): ");
                        if (!DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy HH:mm", null,
                            System.Globalization.DateTimeStyles.None, out DateTime waktu))
                        {
                            Console.WriteLine("Format waktu tidak valid.");
                            break;
                        }
                        var buat = _konsultasiService.BuatKonsultasi(pasien, dokter, keluhan, waktu);
                        Console.WriteLine(buat.Status ? $"Konsultasi ID: {buat.Data?.Id} - {buat.Message}" : $"Gagal: {buat.Message}");
                        break;

                    case "2":
                        Console.Write("ID Konsultasi: ");
                        int idMulai = int.Parse(Console.ReadLine() ?? "0");
                        var mulai = _konsultasiService.MulaiKonsultasi(idMulai);
                        Console.WriteLine(mulai.Status ? mulai.Message : $"Gagal: {mulai.Message}");
                        break;

                    case "3":
                        Console.Write("ID Konsultasi: ");
                        int idSelesai = int.Parse(Console.ReadLine() ?? "0");
                        Console.Write("Catatan Dokter: ");
                        string catatan = Console.ReadLine() ?? "";
                        var selesai = _konsultasiService.SelesaikanKonsultasi(idSelesai, catatan);
                        Console.WriteLine(selesai.Status ? selesai.Message : $"Gagal: {selesai.Message}");
                        break;

                    case "4":
                        Console.Write("ID Konsultasi: ");
                        int idBatal = int.Parse(Console.ReadLine() ?? "0");
                        var batal = _konsultasiService.BatalkanKonsultasi(idBatal);
                        Console.WriteLine(batal.Status ? batal.Message : $"Gagal: {batal.Message}");
                        break;

                    case "5":
                        _konsultasiService.TampilkanDaftarKonsultasi();
                        break;

                    case "0": lanjut = false; break;
                    default: Console.WriteLine("Pilihan tidak valid."); break;
                }
            }
        }

        private void MenuReminder()
        {
            bool lanjut = true;
            while (lanjut)
            {
                Console.WriteLine("\n-- REMINDER --");
                Console.WriteLine("1. Tambah Reminder Manual");
                Console.WriteLine("2. Cek & Kirim Reminder Jatuh Tempo");
                Console.WriteLine("3. Simulasi Kirim Semua Reminder");
                Console.WriteLine("4. Lihat Semua Reminder");
                Console.WriteLine("0. Kembali");
                Console.Write("Pilih: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        Console.Write("Nama Pasien: ");
                        string nama = Console.ReadLine() ?? "";
                        Console.Write("Pesan Reminder: ");
                        string pesan = Console.ReadLine() ?? "";
                        Console.Write("Waktu (dd/MM/yyyy HH:mm): ");
                        if (!DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy HH:mm", null,
                            System.Globalization.DateTimeStyles.None, out DateTime waktu))
                        {
                            Console.WriteLine("Format waktu tidak valid.");
                            break;
                        }
                        var tambah = _reminderService.TambahReminder(nama, pesan, waktu);
                        Console.WriteLine(tambah.Status ? $"Reminder ID: {tambah.Data?.Id}" : $"Gagal: {tambah.Message}");
                        break;

                    case "2":
                        _reminderService.CekDanKirimReminder();
                        break;

                    case "3":
                        _reminderService.SimulasiKirimSemua();
                        break;

                    case "4":
                        _reminderService.TampilkanSemuaReminder();
                        break;

                    case "0": lanjut = false; break;
                    default: Console.WriteLine("Pilihan tidak valid."); break;
                }
            }
        }

        private void MenuKonfigurasi()
        {
            bool lanjut = true;
            while (lanjut)
            {
                Console.WriteLine("\n-- KONFIGURASI RUNTIME --");
                Console.WriteLine($"1. Toggle Notifikasi (sekarang: {(_config.NotifikasiAktif ? "Aktif" : "Nonaktif")})");
                Console.WriteLine($"2. Toggle Konsultasi Online (sekarang: {(_config.KonsultasiOnlineAktif ? "Aktif" : "Nonaktif")})");
                Console.WriteLine($"3. Toggle Reminder (sekarang: {(_config.ReminderAktif ? "Aktif" : "Nonaktif")})");
                Console.WriteLine($"4. Ubah Maks Antrian per Poli (sekarang: {_config.MaksAntrianPerPoli})");
                Console.WriteLine($"5. Ubah Menit Sebelum Reminder (sekarang: {_config.MenitSebelumReminder})");
                Console.WriteLine($"6. Ubah Durasi Maks Konsultasi (sekarang: {_config.DurasiMaksKonsultasiMenit} menit)");
                Console.WriteLine("0. Kembali");
                Console.Write("Pilih: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        _config.NotifikasiAktif = !_config.NotifikasiAktif;
                        Console.WriteLine($"Notifikasi: {(_config.NotifikasiAktif ? "Aktif" : "Nonaktif")}");
                        break;
                    case "2":
                        _config.KonsultasiOnlineAktif = !_config.KonsultasiOnlineAktif;
                        Console.WriteLine($"Konsultasi Online: {(_config.KonsultasiOnlineAktif ? "Aktif" : "Nonaktif")}");
                        break;
                    case "3":
                        _config.ReminderAktif = !_config.ReminderAktif;
                        Console.WriteLine($"Reminder: {(_config.ReminderAktif ? "Aktif" : "Nonaktif")}");
                        break;
                    case "4":
                        Console.Write("Maks antrian per poli baru: ");
                        _config.MaksAntrianPerPoli = int.Parse(Console.ReadLine() ?? "50");
                        Console.WriteLine("Diperbarui.");
                        break;
                    case "5":
                        Console.Write("Menit sebelum reminder baru: ");
                        _config.MenitSebelumReminder = int.Parse(Console.ReadLine() ?? "30");
                        Console.WriteLine("Diperbarui.");
                        break;
                    case "6":
                        Console.Write("Durasi maks konsultasi (menit): ");
                        _config.DurasiMaksKonsultasiMenit = int.Parse(Console.ReadLine() ?? "60");
                        Console.WriteLine("Diperbarui.");
                        break;
                    case "0": lanjut = false; break;
                    default: Console.WriteLine("Pilihan tidak valid."); break;
                }
            }
        }
    }
}