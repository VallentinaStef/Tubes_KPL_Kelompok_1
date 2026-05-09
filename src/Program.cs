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
                    ShowOperationalSchedules(
                        service);
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
                    Console.WriteLine(
                        "Menu tidak valid!");
                    break;
            }
        }
    }

    // OPERATIONAL
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
        Console.WriteLine("\n=== KELOLA JADWAL DOKTER ===");
        Console.WriteLine("1. Lihat Jadwal");
        Console.WriteLine("2. Tambah Jadwal");
        Console.WriteLine("3. Edit Jadwal");
        Console.WriteLine("4. Hapus Jadwal");
        Console.Write("Pilih menu: ");

        int.TryParse(Console.ReadLine(),out int choice);

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
        }
    }

    static void ShowDoctorSchedules(ReservationService service)
    {
        var schedules = service.GetDoctorSchedules();

        Console.WriteLine("\n=== JADWAL DOKTER ===");

        for (int i = 0; i < schedules.Count;  i++)
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

    static void AddDoctorSchedule(
        ReservationService service)
    {
        Console.Write("Nama Dokter: ");
        string doctor = Console.ReadLine() ?? "";

        Console.Write("Hari: ");
        string day =Console.ReadLine() ?? "";

        Console.Write("Jam: ");
        string time = Console.ReadLine() ?? "";

        Console.Write("Kuota: ");

        int.TryParse(Console.ReadLine(), out int quota);

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
        ShowDoctorSchedules(service);

        Console.Write("Index jadwal: ");
        int.TryParse(Console.ReadLine(), out int index);
        index--;

        Console.Write("Nama Dokter Baru: ");
        string doctor = Console.ReadLine() ?? "";

        Console.Write("Hari Baru: ");
        string day = Console.ReadLine() ?? "";

        Console.Write("Jam Baru: ");
        string time = Console.ReadLine() ?? "";

        Console.Write("Kuota Baru: ");

        int.TryParse(Console.ReadLine(), out int quota);

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
        ShowDoctorSchedules(service);

        Console.Write("Index jadwal: ");
        int.TryParse(Console.ReadLine(),out int index);
        index--;

        Console.WriteLine(service.DeleteDoctorSchedule(index));
    }

    static void ReservationMenu(ReservationService service)
    {
        Console.WriteLine("\n=== KELOLA RESERVASI ===");
        Console.WriteLine("1. Tambah Reservasi");
        Console.WriteLine("2. Lihat Reservasi");
        Console.WriteLine("3. Approve Reservasi");
        Console.WriteLine("4. Cancel Reservasi");
        Console.Write("Pilih menu: ");

        int.TryParse(Console.ReadLine(),out int choice);

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
        }
    }

    static void AddReservation(ReservationService service)
    {
        Console.Write("Nama Pasien: ");
        string patient = Console.ReadLine() ?? "";

        Console.Write("Nama Dokter: ");
        string doctor = Console.ReadLine() ?? "";

        Console.Write("Hari: ");
        string day = Console.ReadLine() ?? "";

        Console.Write("Jam: ");
        string time = Console.ReadLine() ?? "";

        Reservation reservation = new Reservation
            {
                PatientName = patient,
                DoctorName = doctor,
                Day = day,
                Time = time
            };

        Console.WriteLine(service.AddReservation(reservation));
    }

    static void ShowReservations(ReservationService service)
    {
        var reservations = service.GetReservations();

        Console.WriteLine("\n=== DATA RESERVASI ===");

        if(reservations.Count == 0)
        {
            Console.WriteLine("Belum ada reservasi.");
            return;
        }

        foreach (var r in reservations)
        {
            Console.WriteLine(
                $"ID: {r.Id}");

            Console.WriteLine(
                $"Booking: " +
                $"{r.BookingNumber}");

            Console.WriteLine(
                $"Pasien: " +
                $"{r.PatientName}");

            Console.WriteLine(
                $"Dokter: " +
                $"{r.DoctorName}");

            Console.WriteLine(
                $"Hari: {r.Day}");

            Console.WriteLine(
                $"Jam: {r.Time}");

            Console.WriteLine(
                $"Status: {r.Status}");

            Console.WriteLine(
                $"Tanggal: " +
                $"{DateHelper.Format(
                    r.ReservationDate)}");

            Console.WriteLine("--------------------");
        }
    }

    static void ApproveReservation(ReservationService service)
    {
        Console.Write("ID Reservasi: ");
        int.TryParse(Console.ReadLine(),out int id);

        Console.WriteLine(service.ApproveReservation(id));
    }

    static void CancelReservation(ReservationService service)
    {
        Console.Write("ID Reservasi: ");

        int.TryParse(Console.ReadLine(), out int id);

        Console.WriteLine(service.CancelReservation(id));
    }
}