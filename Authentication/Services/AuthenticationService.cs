using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
using TUBES_KPL.Authentication.Config;
using TUBES_KPL.Authentication.Model;
using TUBES_KPL.Authentication.Requests;
using TUBES_KPL.Authentication.Validator;

namespace TUBES_KPL.Authentication.Services
{
    public class AuthenticationService
    {
        private readonly string _userFile = "users.json";
        private IDictionary<string, UserData> _users;
        private readonly AuthenticationConfig _config;

        public AuthenticationService(AuthenticationConfig config, string userFile = "users.json")
        {
            _config = config;
            _userFile = userFile;
            _users = LoadUsersFromFile();
        }

        public bool Register(RegisterRequest request)
        {
            // mengecek apakah username sudah ada
            if (_users.ContainsKey(request.Username))
            {
                Console.WriteLine("Username sudah ada.");
                return false;
            }

            // validasi role
            if (!UserRoles.IsValid(request.Role))
            {
                Console.WriteLine("Invalid role.");
                return false;
            }

            // validasi detail account 
            if (!AccountValidator.ValidateAccount(request.Email, request.Username, request.Password))
            {
                Console.WriteLine("Validasi akun gagal.");
                return false;
            }

            // buat data user baru
            var newUser = new UserData
            {
                Username = request.Username,
                Password = request.Password,
                Email = request.Email,
                Role = request.Role
            };

            _users[request.Username] = newUser;

            SaveUsersToFile(); // Simpan ke file
            Console.WriteLine($"User {request.Username} berhasil register.");
            return true;
        }

        public UserData Login(LoginRequest request)
        {
            // verify username ada
            if (!_users.TryGetValue(request.Username, out UserData user))
            {
                Console.WriteLine("User tidak ditemukan.");
                return null;
            }

            // verify ypassword
            if (user.Password != request.Password)
            {
                Console.WriteLine("Password salah.");
                return null;
            }

            Console.WriteLine($"User {request.Username} berhasil login.");
            return user;
        }

        private IDictionary<string, UserData> LoadUsersFromFile()
        {
            try
            {
                if (File.Exists(_userFile))
                {
                    string json = File.ReadAllText(_userFile);
                    var users = JsonSerializer.Deserialize<Dictionary<string, UserData>>(json);
                    if (users != null)
                        return users;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("[ERROR] Gagal memuat data user: " + e.Message);
            }

            return new Dictionary<string, UserData>();
        }

        private void SaveUsersToFile()
        {
            try
            {
                string json = JsonSerializer.Serialize(_users, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_userFile, json);
            }
            catch (Exception e)
            {
                Console.WriteLine("[ERROR] Gagal menyimpan data user: " + e.Message);
            }
        }
    }
}
