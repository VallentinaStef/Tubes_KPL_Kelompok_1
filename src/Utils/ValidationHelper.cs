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

        public static bool IsValidPatientId(int patientId)
        {
            return patientId > 0;
        }

        public static bool IsValidMedicalHistory(MedicalHistory history)
        {
            return history != null
                && history.PatientId > 0
                && !string.IsNullOrWhiteSpace(history.ServiceName)
                && !string.IsNullOrWhiteSpace(history.DoctorName);
        }

        public static bool IsValidPatientCard(PatientCard card)
        {
            return card != null
                && card.PatientId > 0
                && !string.IsNullOrWhiteSpace(card.PatientName)
                && !string.IsNullOrWhiteSpace(card.Gender);
        }

        public static bool IsValidMedicalRecord(MedicalRecord record)
        {
            return record != null
                && record.PatientId > 0
                && !string.IsNullOrWhiteSpace(record.PatientName)
                && !string.IsNullOrWhiteSpace(record.DoctorName)
                && !string.IsNullOrWhiteSpace(record.Diagnosis);
        }
        public static bool IsValidDoctorSchedule(DoctorSchedule schedule)
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
        public static bool IsValidReservation (Reservation reservation)
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
    }
}

