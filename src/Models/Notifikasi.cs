namespace Tubes_KPL_Kelompok_1.Models
{
    public enum TipeNotifikasi
    {
        LayananJadwal,
        AntreanRealTime,
        Reminder
    }

    public class Notifikasi
    {
        public int Id { get; set; }
        public string Judul { get; set; } = string.Empty;
        public string Pesan { get; set; } = string.Empty;
        public TipeNotifikasi Tipe { get; set; }
        public DateTime WaktuKirim { get; set; }
        public bool SudahDibaca { get; set; } = false;
        public string? UserId { get; set; }
    }
}