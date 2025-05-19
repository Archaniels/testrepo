using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TUBES_KPL.Authentication.Config;

namespace TUBES_KPL.Authentication.Validator
{
    class AccountValidator
    {
        public static bool ValidateAccount(string email, string username, string password)
        {
            try
            {
                var config = AuthenticationConfig.Instance;

                // validasi email
                if (string.IsNullOrWhiteSpace(email) || !IsValidEmail(email))
                {
                    throw new ArgumentException("Email tidak valid.");
                }

                // validasi username 
                if (string.IsNullOrWhiteSpace(username) || username.Length < config.MinUsernameLength || !Regex.IsMatch(username, @"^[a-zA-Z0-9]+$"))
                {
                    throw new ArgumentException($"Username tidak valid. Harus minimal {config.MinUsernameLength} karakter dan hanya boleh berisi huruf dan angka.");
                }

                // validasi password
                if (string.IsNullOrWhiteSpace(password) ||
                    password.Length < config.MinPasswordLength ||
                    !Regex.IsMatch(password, @"[A-Z]") ||
                    !Regex.IsMatch(password, @"[a-z]") ||
                    !Regex.IsMatch(password, @"\d"))
                {
                    throw new ArgumentException($"Password harus minimal panjang {config.MinPasswordLength} karakter dan memiliki paling sedikit satu huruf kapital, satu huruf kecil, and satu digit.");
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }

        private static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase);
            }
            catch
            {
                return false;
            }
        }
    }
}