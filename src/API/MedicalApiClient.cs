using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tubes_KPL_Kelompok_1.src.Models;
using Tubes_KPL_Kelompok_1.src.Utils;

namespace Tubes_KPL_Kelompok_1.src.API
{
    internal class MedicalApiClient
    {
        private readonly List<MedicalHistory> histories = new List<MedicalHistory>();
        private readonly List<PatientCard> patientCards = new List<PatientCard>();
        private readonly List<MedicalRecord> records = new List<MedicalRecord>();

        public Response<MedicalHistory> AddMedicalHistory(MedicalHistory history)
        {
            if (!ValidationHelper.IsValidMedicalHistory(history))
            {
                return new Response<MedicalHistory>
                {
                    Status = false,
                    Message = "Data riwayat layanan tidak lengkap."
                };
            }

            history.Id = histories.Count + 1;
            history.ServiceDate = DateTime.Now;
            histories.Add(history);

            return new Response<MedicalHistory>
            {
                Status = true,
                Message = "Riwayat layanan berhasil ditambahkan.",
                Data = history
            };
        }

        public Response<List<MedicalHistory>> GetHistory(int patientId)
        {
            if (!ValidationHelper.IsValidPatientId(patientId))
            {
                return new Response<List<MedicalHistory>>
                {
                    Status = false,
                    Message = "Patient ID tidak valid.",
                    Data = new List<MedicalHistory>()
                };
            }

            var result = histories
                .Where(h => h.PatientId == patientId)
                .ToList();

            return new Response<List<MedicalHistory>>
            {
                Status = true,
                Message = "Riwayat layanan berhasil diambil.",
                Data = result
            };
        }

        public Response<PatientCard> AddPatientCard(PatientCard card)
        {
            if (!ValidationHelper.IsValidPatientCard(card))
            {
                return new Response<PatientCard>
                {
                    Status = false,
                    Message = "Data kartu pasien tidak lengkap."
                };
            }   
            patientCards.Add(card);

            return new Response<PatientCard>
            {
                Status = true,
                Message = "Kartu pasien digital berhasil ditambahkan.",
                Data = card
            };
        }

        public Response<PatientCard> GetPatientCard(int patientId)
        {
            if (!ValidationHelper.IsValidPatientId(patientId))
            {
                return new Response<PatientCard>
                {
                    Status = false,
                    Message = "Patient ID tidak valid."
                };
            }

            var card = patientCards
                .FirstOrDefault(p => p.PatientId == patientId);

            if (card == null)
            {
                return new Response<PatientCard>
                {
                    Status = false,
                    Message = "Kartu pasien digital tidak ditemukan."
                };
            }

            return new Response<PatientCard>
            {
                Status = true,
                Message = "Kartu pasien digital berhasil diambil.",
                Data = card
            };
        }

        public Response<MedicalRecord> AddMedicalRecord(MedicalRecord record)
        {
            if (!ValidationHelper.IsValidMedicalRecord(record))
            {
                return new Response<MedicalRecord>
                {
                    Status = false,
                    Message = "Data rekam medis tidak lengkap."
                };
            }

            record.Id = records.Count + 1;
            record.RecordDate = DateTime.Now;
            records.Add(record);

            return new Response<MedicalRecord>
            {
                Status = true,
                Message = "Rekam medis digital berhasil ditambahkan.",
                Data = record
            };
        }

        public Response<List<MedicalRecord>> GetMedicalRecords(int patientId)
        {
            if (!ValidationHelper.IsValidPatientId(patientId))
            {
                return new Response<List<MedicalRecord>>
                {
                    Status = false,
                    Message = "Patient ID tidak valid.",
                    Data = new List<MedicalRecord>()
                };
            }

            var result = records
                .Where(r => r.PatientId == patientId)
                .ToList();

            return new Response<List<MedicalRecord>>
            {
                Status = true,
                Message = "Data rekam medis berhasil diambil.",
                Data = result
            };
        }
    }
}
