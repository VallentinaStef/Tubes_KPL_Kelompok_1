using System;
using Tubes_KPL_Kelompok_1.Modules;
using Tubes_KPL_Kelompok_1.src.API;
using Tubes_KPL_Kelompok_1.src.Models;
using Tubes_KPL_Kelompok_1.src.Services;
using Tubes_KPL_Kelompok_1.src.States;
using Tubes_KPL_Kelompok_1.src.Utils;

public class Program
{
    static void Main()
    {
        AuthService auth = new AuthService();
        MedicalApiClient api = new MedicalApiClient();
        MedicalServices services = new MedicalServices(api);
      
        ObatModule modulObat = new ObatModule();
        modulObat.TambahJadwal("Paracetamol", "08:00", "500mg");
        modulObat.TambahJadwal("Vitamin C", "12:00", "1 tablet");

        bool isRunning = true;

        while (isRunning)
        {
            Console.WriteLine("\n=== MENU ===");

            if (auth.State == AuthState.LoggedOut)
            {
                Console.WriteLine("1. Login");
                Console.WriteLine("0. Keluar");
            }
            else
            {
                Console.WriteLine($"Login sebagai: {auth.CurrentUser?.Name} ({auth.CurrentUser?.Role})");
                Console.WriteLine(" ");
                Console.WriteLine("1. Lihat Profil");
                Console.WriteLine("2. Edit Profil");
                Console.WriteLine("3. Tambah Riwayat Layanan");
                Console.WriteLine("4. Lihat Riwayat Layanan");
                Console.WriteLine("5. Tambah Kartu Pasien Digital");
                Console.WriteLine("6. Lihat Kartu Pasien Digital");
                Console.WriteLine("7. Tambah Rekam Medis Digital");
                Console.WriteLine("8. Lihat Rekam Medis Digital");
                Console.WriteLine("9. Cek Jadwal & Pengingat Obat");
                Console.WriteLine("10. Logout");
                Console.WriteLine("0. Keluar");
            }

            Console.Write("Pilih: ");
            int.TryParse(Console.ReadLine(), out int choice);

            if (choice == 0)
            {
                Console.WriteLine("Keluar dari program...");
                isRunning = false;
                continue;
            }

            if (auth.State == AuthState.LoggedOut)
            {
                if (choice == 1)
                {
                    Console.Write("Username: ");
                    string user = Console.ReadLine() ?? "";

                    Console.Write("Password: ");
                    string pass = ReadPassword();

                    var result = auth.Login(user, pass);
                    Console.WriteLine(result.Message);
                }
            }
            else
            {
                switch (choice)
                {
                    case 1:
                        Console.WriteLine("\n=== PROFIL ===");
                        Console.WriteLine($"Username: {auth.CurrentUser?.Username}");
                        Console.WriteLine($"Nama: {auth.CurrentUser?.Name}");
                        Console.WriteLine($"Role: {auth.CurrentUser?.Role}");
                        break;

                    case 2:
                        Console.Write("Nama baru: ");
                        auth.CurrentUser!.Name = Console.ReadLine() ?? "";
                        Console.WriteLine("Profil berhasil diupdate!");
                        break;

                    case 3:
                        AddMedicalHistory(services);
                        break;

                    case 4:
                        ShowHistory(services);
                        break;

                    case 5:
                        AddPatientCard(services);
                        break;

                    case 6:
                        ShowPatientCard(services);
                        break;

                    case 7:
                        AddMedicalRecord(services);
                        break;

                    case 8:
                        ShowMedicalRecords(services);
                        break;

                    case 9:
                        ManageMedicine(modulObat);
                        break;
                    
                    case 10:
                        Console.WriteLine(auth.Logout().Message);
                        break;

                    default:
                        Console.WriteLine("Pilihan tidak valid.");
                        break;
                }
            }
        }
    }

    static string ReadPassword()
    {
        string password = "";
        ConsoleKeyInfo key;

        do
        {
            key = Console.ReadKey(true);

            if (key.Key != ConsoleKey.Enter && key.Key != ConsoleKey.Backspace)
            {
                password += key.KeyChar;
                Console.Write("*");
            }
            else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password = password.Substring(0, password.Length - 1);
                Console.Write("\b \b");
            }
        }
        while (key.Key != ConsoleKey.Enter);

        Console.WriteLine();
        return password;
    }

    static void AddMedicalHistory(MedicalServices medicalService)
    {
        Console.WriteLine("\n=== TAMBAH RIWAYAT LAYANAN ===");

        Console.Write("Patient ID: ");
        int.TryParse(Console.ReadLine(), out int patientId);

        Console.Write("Nama Layanan: ");
        string serviceName = Console.ReadLine() ?? "";

        Console.Write("Nama Dokter: ");
        string doctorName = Console.ReadLine() ?? "";

        Console.Write("Keterangan: ");
        string description = Console.ReadLine() ?? "";

        MedicalHistory history = new MedicalHistory
        {
            PatientId = patientId,
            ServiceName = serviceName,
            DoctorName = doctorName,
            Description = description
        };

        var result = medicalService.AddMedicalHistory(history);
        Console.WriteLine(result.Message);
    }

    static void ShowHistory(MedicalServices medicalService)
    {
        Console.Write("\nMasukkan Patient ID: ");
        int.TryParse(Console.ReadLine(), out int patientId);

        var result = medicalService.GetHistory(patientId);
        Console.WriteLine(result.Message);

        if (result.Status && result.Data != null)
        {
            Console.WriteLine("\n=== RIWAYAT LAYANAN ===");

            if (result.Data.Count == 0)
            {
                Console.WriteLine("Belum ada riwayat layanan.");
                return;
            }

            foreach (var history in result.Data)
            {
                Console.WriteLine($"ID Layanan : {history.Id}");
                Console.WriteLine($"Layanan    : {history.ServiceName}");
                Console.WriteLine($"Dokter     : {history.DoctorName}");
                Console.WriteLine($"Tanggal    : {DateHelper.Format(history.ServiceDate)}");
                Console.WriteLine($"Keterangan : {history.Description}");
                Console.WriteLine("--------------------------------");
            }
        }
    }

    static void AddPatientCard(MedicalServices medicalService)
    {
        Console.WriteLine("\n=== TAMBAH KARTU PASIEN DIGITAL ===");

        Console.Write("Patient ID: ");
        int.TryParse(Console.ReadLine(), out int patientId);

        Console.Write("Nama Pasien: ");
        string patientName = Console.ReadLine() ?? "";

        Console.Write("Jenis Kelamin: ");
        string gender = Console.ReadLine() ?? "";

        Console.Write("Tanggal Lahir (yyyy-mm-dd): ");
        DateTime.TryParse(Console.ReadLine(), out DateTime birthDate);

        Console.Write("Alamat: ");
        string address = Console.ReadLine() ?? "";

        PatientCard card = new PatientCard
        {
            PatientId = patientId,
            PatientName = patientName,
            Gender = gender,
            BirthDate = birthDate,
            Address = address
        };

        var result = medicalService.AddPatientCard(card);
        Console.WriteLine(result.Message);
    }

    static void ShowPatientCard(MedicalServices medicalService)
    {
        Console.Write("\nMasukkan Patient ID: ");
        int.TryParse(Console.ReadLine(), out int patientId);

        var result = medicalService.GetPatientCard(patientId);
        Console.WriteLine(result.Message);

        if (result.Status && result.Data != null)
        {
            var card = result.Data;

            Console.WriteLine("\n=== KARTU PASIEN DIGITAL ===");
            Console.WriteLine($"ID Pasien     : {card.PatientId}");
            Console.WriteLine($"Nama          : {card.PatientName}");
            Console.WriteLine($"Jenis Kelamin : {card.Gender}");
            Console.WriteLine($"Tanggal Lahir : {DateHelper.Format(card.BirthDate)}");
            Console.WriteLine($"Alamat        : {card.Address}");
        }
    }

    static void AddMedicalRecord(MedicalServices medicalService)
    {
        Console.WriteLine("\n=== TAMBAH REKAM MEDIS DIGITAL ===");

        Console.Write("Patient ID: ");
        int.TryParse(Console.ReadLine(), out int patientId);

        Console.Write("Nama Pasien: ");
        string patientName = Console.ReadLine() ?? "";

        Console.Write("Nama Dokter: ");
        string doctorName = Console.ReadLine() ?? "";

        Console.Write("Keluhan: ");
        string complaint = Console.ReadLine() ?? "";

        Console.Write("Diagnosis: ");
        string diagnosis = Console.ReadLine() ?? "";

        Console.Write("Obat: ");
        string medicine = Console.ReadLine() ?? "";

        MedicalRecord record = new MedicalRecord
        {
            PatientId = patientId,
            PatientName = patientName,
            DoctorName = doctorName,
            Complaint = complaint,
            Diagnosis = diagnosis,
            Medicine = medicine
        };

        var result = medicalService.AddMedicalRecord(record);
        Console.WriteLine(result.Message);
    }

    static void ShowMedicalRecords(MedicalServices medicalService)
    {
        Console.Write("\nMasukkan Patient ID: ");
        int.TryParse(Console.ReadLine(), out int patientId);
        var result = medicalService.GetMedicalRecords(patientId);
        Console.WriteLine(result.Message);
        if (result.Status && result.Data != null)
        {
            Console.WriteLine("\n=== REKAM MEDIS DIGITAL ===");
            if (result.Data.Count == 0)
            {
                Console.WriteLine("Belum ada rekam medis.");
                return;
            }
            foreach (var record in result.Data)
            {
                Console.WriteLine($"ID Rekam Medis : {record.Id}");
                Console.WriteLine($"Nama Pasien    : {record.PatientName}");
                Console.WriteLine($"Dokter         : {record.DoctorName}");
                Console.WriteLine($"Keluhan        : {record.Complaint}");
                Console.WriteLine($"Diagnosis      : {record.Diagnosis}");
                Console.WriteLine($"Obat           : {record.Medicine}");
                Console.WriteLine($"Tanggal        : {DateHelper.Format(record.RecordDate)}");
                Console.WriteLine("--------------------------------");
            }
        }
    }
    
    static void ManageMedicine(ObatModule obatModule)
    {
        Console.WriteLine("\n=== JADWAL & PENGINGAT OBAT ===");
        obatModule.TampilkanJadwal();

        Console.Write("\nMasukkan jam sekarang (HH:mm): ");
        string inputJam = Console.ReadLine();
        obatModule.CekReminder(inputJam);
    }
}