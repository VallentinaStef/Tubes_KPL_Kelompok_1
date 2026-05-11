namespace Tubes_KPL_Kelompok_1.Models
{
    public class Reminder
    {
        public int Id { get; set; }
        public string NamaPasien { get; set; } = string.Empty;
        public string Pesan { get; set; } = string.Empty;
        public DateTime WaktuReminder { get; set; }
        public bool SudahTerkirim { get; set; } = false;
        public string? KonsultasiId { get; set; }
    }
}
