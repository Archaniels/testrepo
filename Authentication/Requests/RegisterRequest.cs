using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUBES_KPL.Authentication.Config;

namespace TUBES_KPL.Authentication.Requests
{
    public class RegisterRequest
    {
        public string Username { get; init; }
        public string Password { get; init; }
        public string Email { get; init; }
        public string Role { get; init; } = "Customer"; // role standard
    }
}
