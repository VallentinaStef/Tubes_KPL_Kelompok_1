
using System;
using Tubes_KPL_Kelompok_1.Modules;
using System;
using Tubes_KPL_Kelompok_1.src.API;
using Tubes_KPL_Kelompok_1.src.Models;
using Tubes_KPL_Kelompok_1.src.Services;
using Tubes_KPL_Kelompok_1.src.States;
using Tubes_KPL_Kelompok_1.src.Utils;
using Tubes_KPL_Kelompok_1.Modules.Notifikasi;

public class Program
{
    static void Main()
    {
        AuthService auth = new AuthService();
        MedicalApiClient api = new MedicalApiClient();
        MedicalServices services = new MedicalServices(api);
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
                Console.WriteLine("9. Logout");
                Console.WriteLine("10. Notifikasi & Konsultasi");
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
                        Console.WriteLine(auth.Logout().Message);
                        break;

                    case 10:
                        var modulNotifikasi = new NotifikasiModule();
                        modulNotifikasi.Jalankan();
                        break;

                    default:
                        Console.WriteLine("Pilihan tidak valid.");
                        break;
                }
                ReservationApiClient apii = new ReservationApiClient();
                ReservationService service = new ReservationService(apii);

                bool running = true;

                while (running)
                {
                    Console.WriteLine("\n=== TELKOMEDIKA ===");
                    Console.WriteLine("1. Lihat Jadwal Operasional");
                    Console.WriteLine("2. Kelola Jadwal Dokter");
                    Console.WriteLine("3. Kelola Reservasi");
                    Console.WriteLine("0. Keluar");
                    Console.Write("Pilih menu: ");

                    int.TryParse(
                        Console.ReadLine(),
                        out int menu);

                    switch (menu)
                    {
                        case 1:
                            ShowOperationalSchedules(service);
                            break;

                        case 2:
                            DoctorMenu(service);
                            break;

                        case 3:
                            ReservationMenu(service);
                            break;

                        case 0:
                            running = false;
                            break;

                        default:
                            Console.WriteLine("Menu tidak valid!");
                            break;
                    }
                }
            }

            static void ShowOperationalSchedules(ReservationService service)
            {
                var schedules = service.GetOperationalSchedules();

                Console.WriteLine("\n=== JADWAL OPERASIONAL ===");

                foreach (var s in schedules)
                {
                    if (s.Is24Hours)
                    {
                        Console.WriteLine($"{s.Day} : 24 Jam");
                    }
                    else
                    {
                        Console.WriteLine(
                            $"{s.Day} : " +
                            $"{s.OpenTime} - " +
                            $"{s.CloseTime}");
                    }
                }
            }

            static void DoctorMenu(ReservationService service)
            {
                bool back = false;

                while (!back)
                {
                    Console.WriteLine("\n=== KELOLA JADWAL DOKTER ===");
                    Console.WriteLine("1. Lihat Jadwal");
                    Console.WriteLine("2. Tambah Jadwal");
                    Console.WriteLine("3. Edit Jadwal");
                    Console.WriteLine("4. Hapus Jadwal");
                    Console.WriteLine("0. Kembali");
                    Console.Write("Pilih menu: ");

                    string input = Console.ReadLine();

                    if (!int.TryParse(input, out int choice))
                    {
                        Console.WriteLine("Input harus angka!");
                        continue;
                    }

                    switch (choice)
                    {
                        case 1:
                            ShowDoctorSchedules(service);
                            break;

                        case 2:
                            AddDoctorSchedule(service);
                            break;

                        case 3:
                            UpdateDoctorSchedule(service);
                            break;

                        case 4:
                            DeleteDoctorSchedule(service);
                            break;

                        case 0:
                            back = true;
                            break;

                        default:
                            Console.WriteLine("Menu tidak valid!");
                            break;

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

            if (string.IsNullOrWhiteSpace(day))
            {
                Console.WriteLine("Hari tidak valid!");
                continue;
            }

            day = char.ToUpper(day[0]) + day.Substring(1).ToLower();

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
            static void ShowDoctorSchedules(ReservationService service)
            {
                var schedules = service.GetDoctorSchedules();

                Console.WriteLine("\n=== JADWAL DOKTER ===");

                for (int i = 0; i < schedules.Count; i++)
                {
                    var s = schedules[i];

                    Console.WriteLine(
                        $"{i + 1}. " +
                        $"{s.DoctorName} | " +
                        $"{s.Day} | " +
                        $"{s.Time} | " +
                        $"Kuota: {s.AvailableQuota}");
                }
            }

            if (!string.IsNullOrWhiteSpace(time)
                && time.Contains(":")
                && time.Contains("-"))
                break;

                string doctor;
                while (true)
                {
                    Console.Write("Nama Dokter (0 = kembali): ");
                    doctor = Console.ReadLine();

                    if (doctor == "0") return;

                    if (!string.IsNullOrWhiteSpace(doctor) && !doctor.All(char.IsDigit))
                        break;

                    Console.WriteLine("Nama dokter tidak valid!");
                }

                string[] validDays =
                {
                        "Senin","Selasa","Rabu",
                        "Kamis","Jumat","Sabtu","Minggu"
                };

                string day;
                while (true)
                {
                    Console.Write("Hari (0 = kembali): ");
                    day = Console.ReadLine();

                    if (day == "0") return;

                    day = char.ToUpper(day[0]) + day.Substring(1).ToLower();

                    if (validDays.Contains(day))
                        break;

                    Console.WriteLine("Hari tidak valid!");
                }

                string time;
                while (true)
                {
                    Console.Write("Jam (08:00 - 10:00): ");
                    time = Console.ReadLine();

                    if (time == "0") return;

                    if (time.Contains(":") && time.Contains("-"))
                        break;

                    Console.WriteLine("Format jam tidak valid!");
                }

                int quota;
                while (true)
                {
                    Console.Write("Kuota: ");
                    string input = Console.ReadLine();

                    if (input == "0") return;

                    if (int.TryParse(input, out quota) && quota > 0)
                        break;

                    Console.WriteLine("Kuota harus angka > 0!");
                }

                DoctorSchedule schedule = new DoctorSchedule
                {
                    DoctorName = doctor,
                    Day = day,
                    Time = time,
                    AvailableQuota = quota
                };

            if (string.IsNullOrWhiteSpace(day))
            {
                Console.WriteLine("Hari tidak valid!");
                continue;
            }

            day = char.ToUpper(day[0]) + day.Substring(1).ToLower();

            static void UpdateDoctorSchedule(ReservationService service)
            {
                var schedules = service.GetDoctorSchedules();

                ShowDoctorSchedules(service);

                Console.Write("\nMasukkan nomor jadwal (0 = kembali): ");

            if (!string.IsNullOrWhiteSpace(time)
                && time.Contains(":")
                && time.Contains("-"))
                break;

                if (index == 0) return;

                if (index < 1 || index > schedules.Count)
                {
                    Console.WriteLine("Nomor tidak valid!");
                    return;
                }

                index--;

                string doctor;
                while (true)
                {
                    Console.Write("Nama Dokter Baru: ");
                    doctor = Console.ReadLine();

                    if (!string.IsNullOrWhiteSpace(doctor) && !doctor.All(char.IsDigit))
                        break;

                    Console.WriteLine("Nama dokter tidak valid!");
                }

                string[] validDays =
                {
                        "Senin","Selasa","Rabu",
                        "Kamis","Jumat","Sabtu","Minggu"
                    };

                string day;
                while (true)
                {
                    Console.Write("Hari Baru: ");
                    day = Console.ReadLine();

                    day = char.ToUpper(day[0]) + day.Substring(1).ToLower();

                    if (validDays.Contains(day))
                        break;

                    Console.WriteLine("Hari tidak valid!");
                }

                string time;
                while (true)
                {
                    Console.Write("Jam Baru: ");
                    time = Console.ReadLine();

                    if (time.Contains(":") && time.Contains("-"))
                        break;

                    Console.WriteLine("Format jam salah!");
                }

                int quota;
                while (true)
                {
                    Console.Write("Kuota Baru: ");
                    string input = Console.ReadLine();

                    if (int.TryParse(input, out quota) && quota > 0)
                        break;

                    Console.WriteLine("Kuota harus angka > 0!");
                }

                DoctorSchedule schedule = new DoctorSchedule
                {
                    DoctorName = doctor,
                    Day = day,
                    Time = time,
                    AvailableQuota = quota
                };

                Console.WriteLine(service.UpdateDoctorSchedule(index, schedule));
            }

            static void DeleteDoctorSchedule(ReservationService service)
            {
                List<DoctorSchedule> schedules = service.GetDoctorSchedules();

                ShowDoctorSchedules(service);

                Console.Write("\nMasukkan nomor jadwal yang mau dihapus (0 = kembali): ");

                if (!int.TryParse(Console.ReadLine(), out int index))
                {
                    Console.WriteLine("Input harus angka!");
                    return;
                }

                case 0:
                    back = true;
                    break;

                if (index < 1 || index > schedules.Count)
                {
                    Console.WriteLine("Nomor jadwal tidak tersedia!");
                    return;
                }

                index--;

                Console.WriteLine(service.DeleteDoctorSchedule(index));
            }

        var schedules = service.GetDoctorSchedules()
            .Select(s => new { s.Day, s.Time })
            .Distinct()
            .ToList();

        Console.WriteLine("\nPilih Waktu Reservasi:");

                if (string.IsNullOrWhiteSpace(patient))
                {
                    Console.WriteLine("Nama pasien tidak boleh kosong!");
                    return;
                }

            Console.WriteLine($"{i + 1}. {s.Day} | {s.Time}");
        }

                Console.WriteLine("\nPilih Jadwal Dokter:");

                for (int i = 0; i < schedules.Count; i++)
                {
                    var s = schedules[i];

                    Console.WriteLine($"{i + 1}. {s.DoctorName} | {s.Day} | {s.Time}");
                }

                Console.Write("Pilih nomor: ");

        Reservation reservation = new Reservation
        {
            PatientName = patient,
            DoctorName = "Bebas",
            Day = selected.Day,
            Time = selected.Time
        };

                if (pilih < 1 || pilih > schedules.Count)
                {
                    Console.WriteLine("Pilihan tidak valid!");
                    return;
                }

                var selected = schedules[pilih - 1];

                Reservation reservation = new Reservation
                {
                    PatientName = patient,
                    DoctorName = selected.DoctorName,
                    Day = selected.Day,
                    Time = selected.Time
                };

                Console.WriteLine(service.AddReservation(reservation));
            }

        foreach (var r in reservations)
        {
            Console.WriteLine($"ID: {r.Id}");
            Console.WriteLine($"Booking: " + $"{r.BookingNumber}");
            Console.WriteLine($"Pasien: " + $"{r.PatientName}");
            Console.WriteLine($"Hari: {r.Day}");
            Console.WriteLine($"Jam: {r.Time}");
            Console.WriteLine($"Status: {r.Status}");
            Console.WriteLine($"Tanggal: " + $"{DateHelper.Format(r.ReservationDate)}");
            Console.WriteLine("--------------------");
        }
    }

            static void ApproveReservation(ReservationService service)
            {
                Console.Write("ID Reservasi: ");
                int.TryParse(Console.ReadLine(), out int id);

                Console.WriteLine(service.ApproveReservation(id));
            }

            static void CancelReservation(ReservationService service)
            {
                Console.Write("ID Reservasi: ");

                int.TryParse(Console.ReadLine(), out int id);

                Console.WriteLine(service.CancelReservation(id));
            }
        }
    }
}


