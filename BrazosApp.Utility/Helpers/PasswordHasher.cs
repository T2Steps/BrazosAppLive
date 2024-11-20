using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Utility.Helpers
{
    public class PasswordHasher : IPasswordHasher
    {
        private readonly int keySize = 64;
        private readonly int Iterations = 10000;

        public HashSalt HashPassword(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(keySize);

            string encryptedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: Iterations,
                numBytesRequested: 256 / 8
            ));

            return new HashSalt { Hash = encryptedPassword, Salt = salt };
        }

        public bool VerifyPassword(string enteredPassword, byte[] salt, string storedPassword)
        {
            string encryptedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: enteredPassword,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: Iterations,
                numBytesRequested: 256 / 8
            ));

            return encryptedPassword == storedPassword;
        }
    }

    public class HashSalt
    {
        public string Hash { get; set; }
        public byte[] Salt { get; set; }
    }
}
