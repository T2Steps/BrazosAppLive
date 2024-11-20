using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Utility.Helpers
{
    public interface IPasswordHasher
    {
        HashSalt HashPassword(string password);
        bool VerifyPassword(string enteredPassword, byte[] salt, string storedPassword);
    }
}
