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
    internal class MedicalServices
    {
        private MedicalApiClient api;

        public MedicalServices(MedicalApiClient api)
        {
            this.api = api;
        }

        public Response<MedicalHistory> AddMedicalHistory(MedicalHistory h)
        {
            return api.AddMedicalHistory(h);
        }

        public Response<List<MedicalHistory>> GetHistory(int pasien)
        {
            return api.GetHistory(pasien);
        }

        public Response<PatientCard> AddPatientCard(PatientCard card)
        {
            return api.AddPatientCard(card);
        }

        public Response<PatientCard> GetPatientCard(int patient)
        {
            return api.GetPatientCard(patient);
        }

        public Response<MedicalRecord> AddMedicalRecord(MedicalRecord record)
        {
            return api.AddMedicalRecord(record);
        }

        public Response<List<MedicalRecord>> GetMedicalRecords(int pI)
        {
            return api.GetMedicalRecords(pI);
        }
    }
}
