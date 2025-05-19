using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUBES_KPL.Authentication.Config
{
    class UserRoles
    {
        private static readonly HashSet<string> ValidRoles = new() { "Customer", "Admin" };

        public static bool IsValid(string role)
        {
            return ValidRoles.Contains(role);
        }
    }
}
