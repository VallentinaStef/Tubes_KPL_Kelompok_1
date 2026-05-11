using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tubes_KPL_Kelompok_1.src.Models;
using Tubes_KPL_Kelompok_1.src.States;
using Tubes_KPL_Kelompok_1.src.Utils;

namespace Tubes_KPL_Kelompok_1.src.Services
{
    public class AuthService
    {
        public AuthState State { get; private set; } = AuthState.LoggedOut;
        public User CurrentUser;

        private int loginAttempts = 0;
        private const int MAX_ATTEMPTS = 3;

        public Response<User> Login(string username, string password)
        {
            if (loginAttempts >= MAX_ATTEMPTS)
            {
                return new Response<User>
                {
                    Status = false,
                    Message = "Akun terkunci! Terlalu banyak percobaan login."
                };
            }

            // Admin
            if (username == "admin" && password == "123")
            {
                State = AuthState.LoggedIn;
                loginAttempts = 0;

                CurrentUser = new User
                {
                    Username = username,
                    Name = "Admin",
                    Role = "Admin"
                };

                return new Response<User>
                {
                    Status = true,
                    Data = CurrentUser,
                    Message = "Login berhasil sebagai Admin!"
                };
            }

            // Dokter
            if (username == "dokter" && password == "123")
            {
                State = AuthState.LoggedIn;
                loginAttempts = 0;

                CurrentUser = new User
                {
                    Username = username,
                    Name = "Dr. Budi",
                    Role = "Dokter"
                };

                return new Response<User>
                {
                    Status = true,
                    Data = CurrentUser,
                    Message = "Login berhasil sebagai Dokter!"
                };
            }

            // Pasien
            if (username == "pasien" && password == "123")
            {
                State = AuthState.LoggedIn;
                loginAttempts = 0;

                CurrentUser = new User
                {
                    Username = username,
                    Name = "Andi",
                    Role = "Pasien"
                };

                return new Response<User>
                {
                    Status = true,
                    Data = CurrentUser,
                    Message = "Login berhasil sebagai Pasien!"
                };
            }

            loginAttempts++;

            return new Response<User>
            {
                Status = false,
                Message = $"Login gagal! Percobaan ke-{loginAttempts}"
            };
        }

        public Response<string> Logout()
        {
            State = AuthState.LoggedOut;
            CurrentUser = null;

            return new Response<string>
            {
                Status = true,
                Message = "Logout berhasil!"
            };
        }
    }
}
