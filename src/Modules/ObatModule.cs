using System;
using System.Collections.Generic;
using Tubes_KPL_Kelompok_1.Models;
using Tubes_KPL_Kelompok_1.Config;

namespace Tubes_KPL_Kelompok_1.Modules
{
    public class ObatModule
    {
        private List<Obat> tabelJadwal = new List<Obat>();

        public void TambahJadwal(string nama, string waktu, string dosis)
        {
            tabelJadwal.Add(new Obat { Nama = nama, Waktu = waktu, Dosis = dosis });
        }

        public void TampilkanJadwal()
        {
            Console.WriteLine("\n=== JADWAL KONSUMSI OBAT ===");
            foreach (var item in tabelJadwal)
            {
                Console.WriteLine($"- Jam {item.Waktu} | {item.Nama} ({item.Dosis})");
            }
        }

        // FR-011: Pengingat dengan Logika Selisih Waktu
        public void CekReminder(string jamSekarangStr)
        {
            // Parse jam sekarang menjadi tipe TimeSpan agar bisa dihitung
            if (!TimeSpan.TryParse(jamSekarangStr, out TimeSpan jamSekarang))
            {
                Console.WriteLine("Format jam salah! Gunakan HH:mm (contoh 08:00)");
                return;
            }

            bool adaJadwalPas = false;

            foreach (var item in tabelJadwal)
            {
                TimeSpan waktuObat = TimeSpan.Parse(item.Waktu);

                if (waktuObat == jamSekarang)
                {
                    // Jika jamnya pas
                    Console.WriteLine($"\n{AppConfig.ReminderMessage} {item.Nama}!");
                    adaJadwalPas = true;
                }
                else if (waktuObat > jamSekarang)
                {
                    // Jika jam sekarang belum lewat waktu obat (masih ada waktu tersisa)
                    TimeSpan selisih = waktuObat - jamSekarang;

                    Console.WriteLine($"\nKamu harus minum obat {item.Nama}, {selisih.TotalHours} jam lagi.");
                }
            }

            if (!adaJadwalPas && tabelJadwal.TrueForAll(o => TimeSpan.Parse(o.Waktu) < jamSekarang))
            {
                // Jika jam sekarang telah lewat semua waktu obat (tidak ada waktu tersisa)
                Console.WriteLine("\nSemua jadwal obat untuk hari ini sudah lewat.");
            }
        }
    }
}