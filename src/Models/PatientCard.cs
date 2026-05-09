using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tubes_KPL_Kelompok_1.src.Models
{
    public class PatientCard
    {
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public string Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public string Address { get; set; }
    }
}
