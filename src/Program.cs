using System;
using Tubes_KPL_Kelompok_1.src.Services;
using Tubes_KPL_Kelompok_1.src.States;

public class Program
{
    static void Main()
    {
        AuthService auth = new AuthService();
        bool isRunning = true;

        while (isRunning)
        {
            Console.WriteLine("\n=== MENU ===");

            if (auth.State == AuthState.LoggedOut)
            {
                Console.WriteLine("1. Login");
                Console.WriteLine("0. Keluar");
            }
            else
            {
                Console.WriteLine($"Login sebagai: {auth.CurrentUser?.Name} ({auth.CurrentUser?.Role})");
                Console.WriteLine("1. Lihat Profil");
                Console.WriteLine("2. Edit Profil");
                Console.WriteLine("3. Logout");
                Console.WriteLine("0. Keluar");
            }

            Console.Write("Pilih: ");
            int.TryParse(Console.ReadLine(), out int choice);

            if (choice == 0)
            {
                Console.WriteLine("Keluar dari program...");
                isRunning = false;
                continue;
            }

            if (auth.State == AuthState.LoggedOut)
            {
                if (choice == 1)
                {
                    Console.Write("Username: ");
                    string user = Console.ReadLine() ?? "";

                    Console.Write("Password: ");
                    string pass = ReadPassword();

                    var result = auth.Login(user, pass);
                    Console.WriteLine(result.Message);
                }
            }
            else
            {
                switch (choice)
                {
                    case 1:
                        Console.WriteLine("\n=== PROFIL ===");
                        Console.WriteLine($"Username: {auth.CurrentUser?.Username}");
                        Console.WriteLine($"Nama: {auth.CurrentUser?.Name}");
                        Console.WriteLine($"Role: {auth.CurrentUser?.Role}");
                        break;

                    case 2:
                        Console.Write("Nama baru: ");
                        auth.CurrentUser!.Name = Console.ReadLine() ?? "";
                        Console.WriteLine("Profil berhasil diupdate!");
                        break;

                    case 3:
                        Console.WriteLine(auth.Logout().Message);
                        break;
                }
            }
        }
    }

    static string ReadPassword()
    {
        string password = "";
        ConsoleKeyInfo key;

        do
        {
            key = Console.ReadKey(true);

            if (key.Key != ConsoleKey.Enter && key.Key != ConsoleKey.Backspace)
            {
                password += key.KeyChar;
                Console.Write("*");
            }
            else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password = password.Substring(0, password.Length - 1);
                Console.Write("\b \b");
            }
        }
        while (key.Key != ConsoleKey.Enter);

        Console.WriteLine();
        return password;
    }
}