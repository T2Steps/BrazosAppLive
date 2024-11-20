using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Utility.Helpers
{
    public class Encrypt: IEncrypt
    {
        private readonly IConfiguration _configuration;

        public Encrypt(IConfiguration configuration)
        {
            _configuration = configuration;
        }
            //public string Encrypt256(string text)
            //{
            //    //AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            //    using var aes = Aes.Create();
            //    aes.BlockSize = 128;
            //    aes.KeySize = 256;
            //    aes.IV = Encoding.UTF8.GetBytes(_configuration["AES256:IVKey"]);
            //    aes.Key = Encoding.UTF8.GetBytes(_configuration["AES256:Key"]);
            //    aes.Mode = CipherMode.CBC;
            //    aes.Padding = PaddingMode.PKCS7;

            //    byte[] src = Encoding.Unicode.GetBytes(text);

            //    using (ICryptoTransform encrypt = aes.CreateEncryptor())
            //    {
            //        byte[] dest = encrypt.TransformFinalBlock(src, 0, src.Length);
            //        return Convert.ToBase64String(dest);
            //    }
            //}

            //public string Decrypt256(string text)
            //{
            //    //AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            //    using var aes = Aes.Create();
            //    aes.BlockSize = 128;
            //    aes.KeySize = 256;
            //    aes.IV = Encoding.UTF8.GetBytes(_configuration["AES256:IVKey"]);
            //    aes.Key = Encoding.UTF8.GetBytes(_configuration["AES256:Key"]);
            //    aes.Mode = CipherMode.CBC;
            //    aes.Padding = PaddingMode.PKCS7;

            //    byte[] src = System.Convert.FromBase64String(text);

            //    using (ICryptoTransform decrypt = aes.CreateDecryptor())
            //    {
            //        byte[] dest = decrypt.TransformFinalBlock(src, 0, src.Length);
            //        return Encoding.Unicode.GetString(dest);
            //    }
            //}

            public string Encrypt256(string text)
            {
                  using var aes = Aes.Create();
                  aes.BlockSize = 128;
                  aes.KeySize = 256;
                  aes.IV = Encoding.UTF8.GetBytes(_configuration["AES256:IVKey"]);
                  aes.Key = Encoding.UTF8.GetBytes(_configuration["AES256:Key"]);
                  aes.Mode = CipherMode.CBC;
                  aes.Padding = PaddingMode.PKCS7;

                  byte[] src = Encoding.Unicode.GetBytes(text);

                  using (ICryptoTransform encrypt = aes.CreateEncryptor())
                  {
                        byte[] dest = encrypt.TransformFinalBlock(src, 0, src.Length);
                        return Base64UrlEncode(dest);
                  }
            }

            public string Decrypt256(string text)
            {
                  using var aes = Aes.Create();
                  aes.BlockSize = 128;
                  aes.KeySize = 256;
                  aes.IV = Encoding.UTF8.GetBytes(_configuration["AES256:IVKey"]);
                  aes.Key = Encoding.UTF8.GetBytes(_configuration["AES256:Key"]);
                  aes.Mode = CipherMode.CBC;
                  aes.Padding = PaddingMode.PKCS7;

                  byte[] src = Base64UrlDecode(text);

                  using (ICryptoTransform decrypt = aes.CreateDecryptor())
                  {
                        byte[] dest = decrypt.TransformFinalBlock(src, 0, src.Length);
                        return Encoding.Unicode.GetString(dest);
                  }
            }

            private string Base64UrlEncode(byte[] input)
            {
                  return Convert.ToBase64String(input)
                      .Replace('+', '-')
                      .Replace('/', '_')
                      .Replace("=", "");
            }

            private byte[] Base64UrlDecode(string input)
            {
                  string base64 = input
                      .Replace('-', '+')
                      .Replace('_', '/');

                  switch (base64.Length % 4)
                  {
                        case 2: base64 += "=="; break;
                        case 3: base64 += "="; break;
                  }

                  return Convert.FromBase64String(base64);
            }
      }
}
