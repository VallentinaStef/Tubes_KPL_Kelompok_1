using Tubes_KPL_Kelompok_1.src.API;
using Tubes_KPL_Kelompok_1.src.Models;
using Tubes_KPL_Kelompok_1.src.Services;
using Tubes_KPL_Kelompok_1.src.Utils;

public class Program
{
    static void Main()
    {
        ReservationApiClient api = new ReservationApiClient();
        ReservationService service = new ReservationService(api);

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

    static void AddDoctorSchedule(ReservationService service)
    {
        Console.WriteLine("\n=== TAMBAH JADWAL DOKTER ===");

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

            if (string.IsNullOrWhiteSpace(day))
            {
                Console.WriteLine("Hari tidak valid!");
                continue;
            }

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

            if (!string.IsNullOrWhiteSpace(time)
                && time.Contains(":")
                && time.Contains("-"))
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

        Console.WriteLine(service.AddDoctorSchedule(schedule));
    }

    static void UpdateDoctorSchedule(ReservationService service)
    {
        var schedules = service.GetDoctorSchedules();

        ShowDoctorSchedules(service);

        Console.Write("\nMasukkan nomor jadwal (0 = kembali): ");

        if (!int.TryParse(Console.ReadLine(), out int index))
        {
            Console.WriteLine("Input harus angka!");
            return;
        }

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

            if (string.IsNullOrWhiteSpace(day))
            {
                Console.WriteLine("Hari tidak valid!");
                continue;
            }

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

            if (!string.IsNullOrWhiteSpace(time)
                && time.Contains(":")
                && time.Contains("-"))
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

        if (index == 0)
        {
            return;
        }

        if (index < 1 || index > schedules.Count)
        {
            Console.WriteLine("Nomor jadwal tidak tersedia!");
            return;
        }

        index--;

        Console.WriteLine(service.DeleteDoctorSchedule(index));
    }

    static void ReservationMenu(ReservationService service)
    {
        bool back = false;

        while (!back)
        {

            Console.WriteLine("\n=== KELOLA RESERVASI ===");
            Console.WriteLine("1. Tambah Reservasi");
            Console.WriteLine("2. Lihat Reservasi");
            Console.WriteLine("3. Approve Reservasi");
            Console.WriteLine("4. Cancel Reservasi");
            Console.WriteLine("0. Kembali");
            Console.Write("Pilih menu: ");

            int.TryParse(Console.ReadLine(), out int choice);

            switch (choice)
            {
                case 1:
                    AddReservation(service);
                    break;

                case 2:
                    ShowReservations(service);
                    break;

                case 3:
                    ApproveReservation(service);
                    break;

                case 4:
                    CancelReservation(service);
                    break;

                case 0:
                    back = true;
                    break;

                default:
                    Console.WriteLine("Menu tidak Valid!");
                    break;
            }
        }
    }
    static void AddReservation(ReservationService service)
    {
        Console.WriteLine("\n=== TAMBAH RESERVASI ===");

        Console.Write("Nama Pasien: ");
        string patient = Console.ReadLine() ?? "";

        if (string.IsNullOrWhiteSpace(patient))
        {
            Console.WriteLine("Nama pasien tidak boleh kosong!");
            return;
        }

        var schedules = service.GetDoctorSchedules()
            .Select(s => new { s.Day, s.Time })
            .Distinct()
            .ToList();

        Console.WriteLine("\nPilih Waktu Reservasi:");

        for (int i = 0; i < schedules.Count; i++)
        {
            var s = schedules[i];

            Console.WriteLine($"{i + 1}. {s.Day} | {s.Time}");
        }

        Console.Write("Pilih nomor: ");

        int.TryParse(Console.ReadLine(), out int pilih);

        if (pilih < 1 || pilih > schedules.Count)
        {
            Console.WriteLine("Pilihan tidak valid!");
            return;
        }

        var selected = schedules[pilih - 1];

        Reservation reservation = new Reservation
        {
            PatientName = patient,
            DoctorName = "Bebas",
            Day = selected.Day,
            Time = selected.Time
        };

        Console.WriteLine(service.AddReservation(reservation));
    }

    static void ShowReservations(ReservationService service)
    {
        var reservations = service.GetReservations();

        Console.WriteLine("\n=== DATA RESERVASI ===");

        if (reservations.Count == 0)
        {
            Console.WriteLine("Belum ada reservasi.");
            return;
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
