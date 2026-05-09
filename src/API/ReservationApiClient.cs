using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tubes_KPL_Kelompok_1.src.Models;

namespace Tubes_KPL_Kelompok_1.src.API
{
        public class ReservationApiClient
        {
            private List<Reservation> reservations =
                new List<Reservation>();


            // TABLE-DRIVEN CONSTRUCTION
            private List<DoctorSchedule> doctorSchedules =
                new List<DoctorSchedule>()
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


            // TABLE-DRIVEN CONSTRUCTION
            private List<OperationalSchedule>
                operationalSchedules =
                new List<OperationalSchedule>()
            {
                new OperationalSchedule
                {
                    Day = "Senin - Minggu",
                    OpenTime = "24 Jam",
                    CloseTime = "-",
                    Is24Hours = true
                }
            };

            // OPERATIONAL SCHEDULE
            public List<OperationalSchedule>
                    GetOperationalSchedules()
                {
                    return operationalSchedules;
                }

            // DOCTOR SCHEDULE
            public List<DoctorSchedule>
                    GetDoctorSchedules()
                {
                    return doctorSchedules;
                }

                public string AddDoctorSchedule(DoctorSchedule schedule)
                {
                    doctorSchedules.Add(schedule);
                    return "Jadwal dokter berhasil ditambahkan!";
                }
                public string UpdateDoctorSchedule(int index, DoctorSchedule newSchedule)
                {
                    if (index < 0 || index >= doctorSchedules.Count)
                    {
                return "Jadwal tidak ditemukan!";
                    }
                    doctorSchedules[index] = newSchedule;

                    return "Jadwal berhasil diupdate!";
                }
                 public string DeleteDoctorSchedule(int index)
                    {
                        if (index < 0 ||
                            index >= doctorSchedules.Count)
                        {
                            return "Jadwal tidak ditemukan!";
                        }

                        doctorSchedules.RemoveAt(index);
                        return "Jadwal berhasil dihapus!";
                    }
        //Reservation
        public string AddReservation(Reservation reservation)
            {
                reservation.Id = reservations.Count + 1;

                reservation.BookingNumber = $"A-{reservation.Id:000}";

                reservations.Add(reservation);

                return "Reservasi berhasil dibuat!";
            }


            public List<Reservation>
                GetReservations()
            {
                return reservations;
            }


            public string ApproveReservation(int id)
            {
                var reservation = reservations.FirstOrDefault( r => r.Id == id);

                if (reservation == null)
                {
                    return "Reservasi tidak ditemukan!";
                }

                reservation.Status = ReservationStatus.Approved.ToString();

                return "Reservasi berhasil diapprove!";
            }


            public string CancelReservation(int id)
            {
                var reservation = reservations.FirstOrDefault(r => r.Id == id);

                if (reservation == null)
                {
                    return "Reservasi tidak ditemukan!";
                }

                reservation.Status = ReservationStatus.Cancelled.ToString();

                return "Reservasi berhasil dicancel!";
            }
        }
    }

