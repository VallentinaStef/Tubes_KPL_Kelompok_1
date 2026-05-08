using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tubes_KPL_Kelompok_1.src.Models
{
    public class MedicalHistory
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string ServiceName { get; set; }
        public string DoctorName { get; set; }
        public DateTime ServiceDate { get; set; }
        public string Description { get; set; }
    }
}
