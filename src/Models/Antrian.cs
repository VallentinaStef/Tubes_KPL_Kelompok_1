namespace Tubes_KPL_Kelompok_1.Models
{
    public enum StatusAntrian
    {
        Menunggu,
        Dipanggil,
        Selesai,
        Dibatalkan
    }

    public class Antrian
    {
        public int Id { get; set; }
        public string NomorAntrian { get; set; } = string.Empty;
        public string NamaPasien { get; set; } = string.Empty;
        public string Poli { get; set; } = string.Empty;
        public DateTime WaktuDaftar { get; set; }
        public StatusAntrian Status { get; set; } = StatusAntrian.Menunggu;
        public int PosisiAntrian { get; set; }
    }
}
