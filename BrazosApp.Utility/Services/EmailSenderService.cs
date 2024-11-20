using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace BrazosApp.Utility.Services
{
      public class EmailSenderService : IEmailSenderService
      {
            private readonly IConfiguration _configuration;
            public EmailSenderService(IConfiguration configuration)
            {
                  _configuration = configuration;
            }

            public async Task SendEmail(string toEmail, string? ccEmail, string subject, string body, Byte[][]? bytesArray, string[]? pdfName)
            {
                  var MailIds = toEmail.Split(',');
                  MailMessage message = new MailMessage();
                  message.From = new MailAddress(_configuration["Smtp:FromAddress"], _configuration["Smtp:DisplayMail"]);
                  for (int i = 0; i < MailIds.Length; i++)
                  {
                        message.To.Add(new MailAddress(MailIds[i]));
                  }
                  //message.To.Add(new MailAddress(toEmail));
                  if (ccEmail != null && ccEmail != "")
                  {
                        var ccMailIds = ccEmail.Split(',');
                        for (int i = 0; i < ccMailIds.Length; i++)
                        {
                              message.CC.Add(new MailAddress(ccMailIds[i]));
                        }
                  }

                  //Byte[][] bytes = new Byte[][] { bytesArray, bytesArray, bytesArray };

                  message.CC.Add(new MailAddress(_configuration["Smtp:CC"]));
                  //message.CC.Add(new MailAddress("roy@inspect2go.com"));
                  message.Subject = subject;
                  message.Body = body;
                  //message.Attachments = new List<>;
                  if (bytesArray!.Any() && bytesArray != null)
                  {
                        var i = 0; 
                        foreach (var b in bytesArray)
                        {
                            message.Attachments.Add(new Attachment(new MemoryStream(b), pdfName![i]));
                            i++;
                        }
                        
                        //Attachment attachment = new Attachment(new MemoryStream(bytesArray), pdfName);
                  }
                  //if (attachments.Length > 0)
                  //{
                  //    string fileName = Path.GetFileName(attachments.FileName);
                  //    message.Attachments.Add(new Attachment(attachments.OpenReadStream(), fileName));
                  //}
                  message.IsBodyHtml = true;

                  var smtpclient = new SmtpClient(_configuration["Smtp:Server"])
                  {
                        Port = Convert.ToInt32(_configuration["Smtp:Port"]),
                        Credentials = new NetworkCredential(_configuration["Smtp:Username"], _configuration["Smtp:Password"]),
                        EnableSsl = Convert.ToBoolean(_configuration["Smtp:SSL"]),
                  };
                  string Userstate = "Sent";
                  try
                  {
                        smtpclient.SendAsync(message, Userstate);
                  }
                  catch (Exception ex) { }

            }
      }
}
