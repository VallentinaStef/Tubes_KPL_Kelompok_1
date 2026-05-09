using System;
using Tubes_KPL_Kelompok_1.Modules;

namespace Tubes_KPL_Kelompok_1
{
    class Program
    {
        static void Main(string[] args)
        {
            ObatModule modulObat = new ObatModule();

            // Simulasi tambah data
            modulObat.TambahJadwal("Paracetamol", "08:00", "500mg");
            modulObat.TambahJadwal("Vitamin C", "12:00", "1 tablet");

            // Tampilkan Jadwal (FR-005)
            modulObat.TampilkanJadwal();

            // Simulasi Pengingat (FR-011)
            Console.Write("\nMasukkan jam sekarang (format HH:mm, misal 08:00): ");
            string inputJam = Console.ReadLine();
            modulObat.CekReminder(inputJam);
        }
    }
}