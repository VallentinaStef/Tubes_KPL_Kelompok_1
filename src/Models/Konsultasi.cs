namespace Tubes_KPL_Kelompok_1.Models
{
    public enum StatusKonsultasi
    {
        Menunggu,
        Berlangsung,
        Selesai,
        Dibatalkan
    }

    public class Konsultasi
    {
        public int Id { get; set; }
        public string NamaPasien { get; set; } = string.Empty;
        public string NamaDokter { get; set; } = string.Empty;
        public string Keluhan { get; set; } = string.Empty;
        public DateTime WaktuMulai { get; set; }
        public DateTime? WaktuSelesai { get; set; }
        public StatusKonsultasi Status { get; set; } = StatusKonsultasi.Menunggu;
        public string? CatatanDokter { get; set; }
    }
}
