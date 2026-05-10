using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tubes_KPL_Kelompok_1.src.Utils
{
    public static class DateHelper
    {
        public static string Format(DateTime d)
        {
            return d.ToString("dd/MM/yyyy");
        }
    }
}
