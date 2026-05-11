using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Tubes_KPL_Kelompok_1.src.Models;

namespace Tubes_KPL_Kelompok_1.src.API
{
    // API implementation: this class acts as the local API layer for the app.
    // Instead of calling an external server, it reads and writes JSON files in the data folder.
    public class ReservationApiClient
    {
        private readonly string dataDirectory = Path.Combine(
            Directory.GetCurrentDirectory(),
            "data");

        private readonly string reservationsFile;
        private readonly string doctorSchedulesFile;
        private readonly string operationalSchedulesFile;

        private readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        public ReservationApiClient()
        {
            reservationsFile = Path.Combine(dataDirectory, "reservations.json");
            doctorSchedulesFile = Path.Combine(dataDirectory, "doctorSchedules.json");
            operationalSchedulesFile = Path.Combine(dataDirectory, "operationalSchedules.json");

            Directory.CreateDirectory(dataDirectory);
            EnsureFile(reservationsFile, new List<Reservation>());
            EnsureFile(doctorSchedulesFile, GetDefaultDoctorSchedules());
            EnsureFile(operationalSchedulesFile, GetDefaultOperationalSchedules());
        }

        private void EnsureFile<T>(string filePath, List<T> defaultData)
        {
            if (File.Exists(filePath))
            {
                return;
            }

            SaveData(filePath, defaultData);
        }

        private List<T> LoadData<T>(string filePath)
        {
            string json = File.ReadAllText(filePath);

            if (string.IsNullOrWhiteSpace(json))
            {
                return new List<T>();
            }

            return JsonSerializer.Deserialize<List<T>>(json, jsonOptions)
                ?? new List<T>();
        }

        private void SaveData<T>(string filePath, List<T> data)
        {
            string json = JsonSerializer.Serialize(data, jsonOptions);
            File.WriteAllText(filePath, json);
        }

        // Table-driven implementation: default doctor schedules are represented as data rows.
        private List<DoctorSchedule> GetDefaultDoctorSchedules()
        {
            return new List<DoctorSchedule>()
            {
                new DoctorSchedule
                {
                    DoctorName = "Dr. Andi",
                    Day = "Senin",
                    Time = "08:00 - 10:00",
                    AvailableQuota = 10
                },
                new DoctorSchedule
                {
                    DoctorName = "Dr. Clara",
                    Day = "Rabu",
                    Time = "13:00 - 15:00",
                    AvailableQuota = 5
                },
                new DoctorSchedule
                {
                    DoctorName = "Dr. Andre",
                    Day = "Kamis",
                    Time = "19:00 - 00:00",
                    AvailableQuota = 7
                }
            };
        }

        // Table-driven implementation: operational schedule is stored as configurable data.
        private List<OperationalSchedule> GetDefaultOperationalSchedules()
        {
            return new List<OperationalSchedule>()
            {
                new OperationalSchedule
                {
                    Day = "Senin - Minggu",
                    OpenTime = "24 Jam",
                    CloseTime = "-",
                    Is24Hours = true
                }
            };
        }

        // OPERATIONAL SCHEDULE
        public List<OperationalSchedule> GetOperationalSchedules()
        {
            return LoadData<OperationalSchedule>(operationalSchedulesFile);
        }

        // DOCTOR SCHEDULE
        public List<DoctorSchedule> GetDoctorSchedules()
        {
            return LoadData<DoctorSchedule>(doctorSchedulesFile);
        }

        public string AddDoctorSchedule(DoctorSchedule schedule)
        {
            List<DoctorSchedule> doctorSchedules = GetDoctorSchedules();

            doctorSchedules.Add(schedule);
            SaveData(doctorSchedulesFile, doctorSchedules);

            return "Jadwal dokter berhasil ditambahkan!";
        }

        public string UpdateDoctorSchedule(int index, DoctorSchedule newSchedule)
        {
            List<DoctorSchedule> doctorSchedules = GetDoctorSchedules();

            if (index < 0 || index >= doctorSchedules.Count)
            {
                return "Jadwal tidak ditemukan!";
            }

            doctorSchedules[index] = newSchedule;
            SaveData(doctorSchedulesFile, doctorSchedules);

            return "Jadwal berhasil diupdate!";
        }

        public string DeleteDoctorSchedule(int index)
        {
            List<DoctorSchedule> doctorSchedules = GetDoctorSchedules();

            if (index < 0 || index >= doctorSchedules.Count)
            {
                return "Jadwal tidak ditemukan!";
            }

            doctorSchedules.RemoveAt(index);
            SaveData(doctorSchedulesFile, doctorSchedules);

            return "Jadwal berhasil dihapus!";
        }

        //Reservation
        public string AddReservation(Reservation reservation)
        {
            List<Reservation> reservations = GetReservations();

            reservation.Id = reservations.Count == 0
                ? 1
                : reservations.Max(r => r.Id) + 1;

            reservation.BookingNumber = $"A-{reservation.Id:000}";

            reservations.Add(reservation);
            SaveData(reservationsFile, reservations);

            return "Reservasi berhasil dibuat!";
        }


        public List<Reservation> GetReservations()
        {
            return LoadData<Reservation>(reservationsFile);
        }


        public string ApproveReservation(int id)
        {
            List<Reservation> reservations = GetReservations();
            var reservation = reservations.FirstOrDefault(r => r.Id == id);

            if (reservation == null)
            {
                return "Reservasi tidak ditemukan!";
            }

            reservation.Status = ReservationStatus.Approved.ToString();
            SaveData(reservationsFile, reservations);

            return "Reservasi berhasil diapprove!";
        }


        public string CancelReservation(int id)
        {
            List<Reservation> reservations = GetReservations();
            var reservation = reservations.FirstOrDefault(r => r.Id == id);

            if (reservation == null)
            {
                return "Reservasi tidak ditemukan!";
            }

            reservation.Status = ReservationStatus.Cancelled.ToString();
            SaveData(reservationsFile, reservations);

            return "Reservasi berhasil dicancel!";
        }
    }
}

