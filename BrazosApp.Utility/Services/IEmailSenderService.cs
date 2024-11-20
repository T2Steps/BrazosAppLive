using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Utility.Services
{
      public interface IEmailSenderService
      {
            Task SendEmail(string toemail, string? ccEmail, string subject, string body, Byte[][]? bytesArray, string[]? pdfName);
      }
}
