using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Utility.Helpers
{
    public class PasswordGenerator : IPasswordGenerator
    {
        public string numberGenerate()
        {
            var random = new Random();
            return random.Next(0, 999).ToString("D3");
        }

        public string letterGenerate()
        {
            Random random = new Random();
            const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            string randomString = new string(Enumerable.Repeat(letters, 3)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            return randomString;
        }

        public string charGenerate()
        {
            Random random = new Random();
            const string letters = "!@#$%^&*";
            string randomString = new string(Enumerable.Repeat(letters, 1)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            return randomString;
        }
        public string TemporaryPassword()
        {
            var Password = charGenerate() + letterGenerate() + numberGenerate()  + charGenerate();
            return Password;
        }
    }
}
