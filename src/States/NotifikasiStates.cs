using Tubes_KPL_Kelompok_1.Models;

namespace Tubes_KPL_Kelompok_1.States
{
    public class AntrianStateMachine
    {
        private StatusAntrian _state;
        public StatusAntrian State => _state;

        public AntrianStateMachine(StatusAntrian initialState = StatusAntrian.Menunggu)
        {
            _state = initialState;
        }

        public bool Panggil()
        {
            if (_state == StatusAntrian.Menunggu)
            {
                _state = StatusAntrian.Dipanggil;
                Console.WriteLine($"[Antrian] ✅ Transisi: Menunggu → Dipanggil");
                return true;
            }
            Console.WriteLine($"[Antrian] ❌ Tidak bisa memanggil dari state: {_state}");
            return false;
        }

        public bool Selesaikan()
        {
            if (_state == StatusAntrian.Dipanggil)
            {
                _state = StatusAntrian.Selesai;
                Console.WriteLine($"[Antrian] ✅ Transisi: Dipanggil → Selesai");
                return true;
            }
            Console.WriteLine($"[Antrian] ❌ Tidak bisa selesaikan dari state: {_state}");
            return false;
        }

        public bool Batalkan()
        {
            if (_state == StatusAntrian.Menunggu || _state == StatusAntrian.Dipanggil)
            {
                _state = StatusAntrian.Dibatalkan;
                Console.WriteLine($"[Antrian] ✅ Transisi: {_state} → Dibatalkan");
                return true;
            }
            Console.WriteLine($"[Antrian] ❌ Tidak bisa batalkan dari state: {_state}");
            return false;
        }
    }

    public class KonsultasiStateMachine
    {
        private StatusKonsultasi _state;
        public StatusKonsultasi State => _state;

        public KonsultasiStateMachine(StatusKonsultasi initialState = StatusKonsultasi.Menunggu)
        {
            _state = initialState;
        }

        public bool Mulai()
        {
            if (_state == StatusKonsultasi.Menunggu)
            {
                _state = StatusKonsultasi.Berlangsung;
                Console.WriteLine($"[Konsultasi] ✅ Transisi: Menunggu → Berlangsung");
                return true;
            }
            Console.WriteLine($"[Konsultasi] ❌ Tidak bisa mulai dari state: {_state}");
            return false;
        }

        public bool Selesaikan()
        {
            if (_state == StatusKonsultasi.Berlangsung)
            {
                _state = StatusKonsultasi.Selesai;
                Console.WriteLine($"[Konsultasi] ✅ Transisi: Berlangsung → Selesai");
                return true;
            }
            Console.WriteLine($"[Konsultasi] ❌ Tidak bisa selesaikan dari state: {_state}");
            return false;
        }

        public bool Batalkan()
        {
            if (_state == StatusKonsultasi.Menunggu || _state == StatusKonsultasi.Berlangsung)
            {
                var prev = _state;
                _state = StatusKonsultasi.Dibatalkan;
                Console.WriteLine($"[Konsultasi] ✅ Transisi: {prev} → Dibatalkan");
                return true;
            }
            Console.WriteLine($"[Konsultasi] ❌ Tidak bisa batalkan dari state: {_state}");
            return false;
        }
    }
}
