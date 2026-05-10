using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tubes_KPL_Kelompok_1.src.API;
using Tubes_KPL_Kelompok_1.src.Models;
using Tubes_KPL_Kelompok_1.src.Utils;

namespace Tubes_KPL_Kelompok_1.src.Services
{
    public class ReservationService
    {
        private ReservationApiClient api;

        public ReservationService(ReservationApiClient api)
        {
            this.api = api;
        }

        //operasionall
        public List<OperationalSchedule>
            GetOperationalSchedules()
        {
            return api.GetOperationalSchedules();
        }

        //DoctorSchedule
        public List<DoctorSchedule>
            GetDoctorSchedules()
        {
            return api.GetDoctorSchedules();
        }

        public string AddDoctorSchedule(DoctorSchedule schedule)
        {
            if (schedule == null)
            {
                return "Data tidak valid!";
            }

            if (!ValidationHelper.IsValidDoctorSchedule(schedule))
            {
                return "Data jadwal dokter tidak lengkap!";
            }

            return api.AddDoctorSchedule(schedule);
        }
        public string UpdateDoctorSchedule(int index,DoctorSchedule schedule)
        {
            return api.UpdateDoctorSchedule(index, schedule);
        }
        public string DeleteDoctorSchedule(int index)
        {
            return api.DeleteDoctorSchedule(index);
        }

        //reservatiopn
        public string AddReservation(Reservation reservation)
        {
            // Defensive Programming
            if (reservation == null)
            {
                return "Data reservasi tidak valid!";
            }

            if (!ValidationHelper.IsValidReservation(reservation))
            {
                return "Data reservasi tidak lengkap!";
            }

            reservation.Status = ReservationStatus.Pending.ToString();
            reservation.ReservationDate = DateTime.Now;

            return api.AddReservation(reservation);
        }


        public List<Reservation>
            GetReservations()
        {
            return api.GetReservations();
        }

        public string ApproveReservation(int id)
        {
            return api.ApproveReservation(id);
        }

        public string CancelReservation(int id)
        {
            return api.CancelReservation(id);
        }
    }
}
