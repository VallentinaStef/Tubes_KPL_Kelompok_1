using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tubes_KPL_Kelompok_1.src.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        public string PatientName { get; set; }

        public string DoctorName { get; set; }

        public string Day { get; set; }

        public string Time { get; set; }

        public string BookingNumber { get; set; }

        public DateTime ReservationDate { get; set; }

        public string Status { get; set; }
    }
}
