using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tubes_KPL_Kelompok_1.src.Models
{
    public class DoctorSchedule
    {
        public string DoctorName { get; set; }

        public string Day { get; set; }

        public string Time { get; set; }

        public int AvailableQuota { get; set; }
    }
}
