namespace Tubes_KPL_Kelompok_1.Config
{

    public class NotifikasiConfig
    {
        private static NotifikasiConfig? _instance;
        private static readonly object _lock = new();

        public static NotifikasiConfig Instance
        {
            get
            {
                lock (_lock)
                {
                    _instance ??= new NotifikasiConfig();
                    return _instance;
                }
            }
        }

        public bool NotifikasiAktif { get; set; } = true;
        public int IntervalCekJadwalMenit { get; set; } = 30;

        public int MaksAntrianPerPoli { get; set; } = 50;
        public int IntervalUpdateAntrianDetik { get; set; } = 10;

        public int DurasiMaksKonsultasiMenit { get; set; } = 30;
        public bool KonsultasiOnlineAktif { get; set; } = true;

        public int MenitSebelumReminder { get; set; } = 60;
        public bool ReminderAktif { get; set; } = true;

        private NotifikasiConfig() { }

        public void UpdateRuntime(string key, string value)
        {
            switch (key.ToLower())
            {
                case "notifikasiaktif":
                    NotifikasiAktif = bool.Parse(value);
                    break;
                case "intervalcekjadwalmenit":
                    IntervalCekJadwalMenit = int.Parse(value);
                    break;
                case "maksbantrian":
                    MaksAntrianPerPoli = int.Parse(value);
                    break;
                case "intervalupdateantriandetik":
                    IntervalUpdateAntrianDetik = int.Parse(value);
                    break;
                case "durasimakskonsultasimenit":
                    DurasiMaksKonsultasiMenit = int.Parse(value);
                    break;
                case "konsultasionlineaktif":
                    KonsultasiOnlineAktif = bool.Parse(value);
                    break;
                case "menitsebelumreminder":
                    MenitSebelumReminder = int.Parse(value);
                    break;
                case "reminderaktif":
                    ReminderAktif = bool.Parse(value);
                    break;
                default:
                    Console.WriteLine($"[Config] Key '{key}' tidak dikenali.");
                    break;
            }
            Console.WriteLine($"[Config] {key} diperbarui menjadi: {value}");
        }

        public void TampilkanSemua()
        {
            Console.WriteLine("\n=== RUNTIME CONFIGURATION ===");
            Console.WriteLine($"[Notifikasi] Aktif            : {NotifikasiAktif}");
            Console.WriteLine($"[Notifikasi] Interval Cek     : {IntervalCekJadwalMenit} menit");
            Console.WriteLine($"[Antrian]    Maks Per Poli    : {MaksAntrianPerPoli}");
            Console.WriteLine($"[Antrian]    Interval Update  : {IntervalUpdateAntrianDetik} detik");
            Console.WriteLine($"[Konsultasi] Durasi Maks      : {DurasiMaksKonsultasiMenit} menit");
            Console.WriteLine($"[Konsultasi] Online Aktif     : {KonsultasiOnlineAktif}");
            Console.WriteLine($"[Reminder]   Menit Sebelum    : {MenitSebelumReminder}");
            Console.WriteLine($"[Reminder]   Aktif            : {ReminderAktif}");
            Console.WriteLine("==============================\n");
        }
    }
}
