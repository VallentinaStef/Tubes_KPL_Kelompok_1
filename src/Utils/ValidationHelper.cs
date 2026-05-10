using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tubes_KPL_Kelompok_1.src.Models;

namespace Tubes_KPL_Kelompok_1.src.Utils
{
    public static class ValidationHelper
    {
        public static bool
            IsValidReservation(
            Reservation reservation)
        {
            return reservation != null
                && !string.IsNullOrWhiteSpace(
                    reservation.PatientName)
                && !string.IsNullOrWhiteSpace(
                    reservation.DoctorName)
                && !string.IsNullOrWhiteSpace(
                    reservation.Day)
                && !string.IsNullOrWhiteSpace(
                    reservation.Time);
        }


        public static bool
            IsValidDoctorSchedule(
            DoctorSchedule schedule)
        {
            return schedule != null
                && !string.IsNullOrWhiteSpace(
                    schedule.DoctorName)
                && !string.IsNullOrWhiteSpace(
                    schedule.Day)
                && !string.IsNullOrWhiteSpace(
                    schedule.Time)
                && schedule.AvailableQuota > 0;
        }
    }
}
