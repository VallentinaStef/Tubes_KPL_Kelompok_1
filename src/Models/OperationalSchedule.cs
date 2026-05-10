using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tubes_KPL_Kelompok_1.src.Models
{
    public class OperationalSchedule
    {
        public string Day { get; set; }

        public string OpenTime { get; set; }

        public string CloseTime { get; set; }

        public bool Is24Hours { get; set; }
    }
}
