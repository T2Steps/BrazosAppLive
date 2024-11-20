using BrazosApp.DataAccess.Repository.IRepository;
using BrazosApp.Models;
using BrazosApp.Models.ViewModels;
using BrazosApp.RenewalCronApp.Models;
using BrazosApp.Utility.Helpers;
using BrazosApp.Utility.Services;
using DinkToPdf;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
//using Microsoft.Reporting.Map.WebForms.BingMaps;
//using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Wkhtmltopdf.NetCore;
using System.Runtime.InteropServices;
//using PuppeteerSharp.Media;
//using PuppeteerSharp;

namespace BrazosApp.RenewalCronApp
{
    public class Renewal
    {
        private readonly IEmailSenderService _emailSender;
        private readonly IConfiguration _configuration;
        private readonly IGeneratePdf _generatePdf;
        private readonly IRepository<Establishment> _establishment;
        private readonly IRepository<Payment> _paymentRepo;
        private readonly IRepository<PaymentDetailsTable> _paymentDetailsTable;
        private readonly IRepository<Users> _users;
        private readonly IEncrypt _encrypt;
        private readonly IRepository<Fees> _fees;
        private readonly IRepository<FeesDetailsTable> _feesDetailsTable;
        private readonly IRepository<JurisdictionAccounts> _jurisdictionaccounts;
        private readonly IRepository<EstablishmentTypes> _estTypes;
        private readonly IRepository<RenewalTempHistory> _renewalTempHistory;

        public Renewal(IEmailSenderService emailSender,
            IConfiguration configuration,
            IGeneratePdf generatePdf,
            IRepository<Payment> paymentRepo,
            IRepository<PaymentDetailsTable> paymentDetailsTable,
            IRepository<Users> users,
            IRepository<Fees> fees,
            IRepository<FeesDetailsTable> feesDetailsTable,
            IRepository<Establishment> establishment,
            IRepository<JurisdictionAccounts> jurisdictionaccounts,
            IRepository<EstablishmentTypes> estTypes,
            IRepository<RenewalTempHistory> renewalTempHistory,
            IEncrypt encrypt
            )
        {
            _emailSender = emailSender;
            _configuration = configuration;
            _generatePdf = generatePdf;
            _paymentRepo = paymentRepo;
            _paymentDetailsTable = paymentDetailsTable;
            _users = users;
            _establishment = establishment;
            _fees = fees;
            _feesDetailsTable = feesDetailsTable;
            _jurisdictionaccounts = jurisdictionaccounts;
            _estTypes = estTypes;
            _renewalTempHistory = renewalTempHistory;
            _encrypt = encrypt;
        }

        //[STAThread]
        ////public async Task Run(string[] args)
        //public void Run(string[] args)
        //{
        //      try
        //      {
        //            //OwnerEmailVM model = new OwnerEmailVM();
        //            //model.ToMail = "nd.niladri001@gmail.com";
        //            //model.CCMail = "santanu@tech2steps.com";
        //            //model.Subject = "Cron Mail Test";
        //            //model.Body = "Cron Mail Body Test";
        //            //SendEmail(model);
        //            //Console.WriteLine(System.Configuration.ConfigurationManager.ConnectionStrings["BrazosAppCronJobs"].ConnectionString);
        //            //Console.ReadKey();

        //            //SqlConnection myConnection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["BrazosAppCronJobs"].ConnectionString);

        //            using (SqlConnection myConnection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["BrazosAppCronJobs"].ConnectionString))
        //            {
        //                  myConnection.Open();
        //                  // to get the location the assembly is executing from
        //                  string path = System.Reflection.Assembly.GetExecutingAssembly().Location;

        //                  //once you have the path you get the directory with:
        //                  var directory = System.IO.Path.GetDirectoryName(path);

        //                  //Console.WriteLine(directory);

        //                  //Console.ReadKey();
        //                  var dir = directory + "\\Log";

        //                  if (!Directory.Exists(dir))
        //                  {
        //                        Directory.CreateDirectory(dir);
        //                  }

        //                  string[] files = Directory.GetFiles(dir);

        //                  for (int i = 0; i < files.Count(); i++)
        //                  {
        //                        var creationdt = File.GetCreationTime(files[i]);
        //                        var dtdiff = (DateTime.Now - creationdt).Days;
        //                        if (dtdiff > 3)
        //                        {
        //                              File.Delete(files[i]);
        //                        }
        //                  }

        //                  var cdt = DateTime.Now;
        //                  var cdtDay = cdt.Day;
        //                  var cdtMonth = cdt.Month;
        //                  var cdtYear = cdt.Year;
        //                  var cdtMonthStr = "";
        //                  var cdtDayStr = "";
        //                  if (cdtMonth < 10)
        //                  {
        //                        cdtMonthStr = '0' + cdtMonth.ToString();
        //                  }
        //                  else
        //                  {
        //                        cdtMonthStr = cdtMonth.ToString();
        //                  }
        //                  if (cdtDay < 10)
        //                  {
        //                        cdtDayStr = '0' + cdtDay.ToString();
        //                  }
        //                  else
        //                  {
        //                        cdtDayStr = cdtDay.ToString();
        //                  }

        //                  string fileName = dir + "\\" + cdtMonthStr + '-' + cdtDayStr + '-' + cdtYear.ToString() + ".txt";

        //                  if (File.Exists(fileName))
        //                  {
        //                        File.Delete(fileName);
        //                  }

        //        using (StreamWriter sw = File.CreateText(fileName))
        //        {

        //            sw.WriteLine(DateTime.Now.ToString() + ": Job Start");
        //            sw.WriteLine();

        //            var myRenewalCommand = new SqlCommand("PermitStatusChange", myConnection);
        //            myRenewalCommand.CommandType = CommandType.StoredProcedure;

        //            DataSet ds = new DataSet();
        //            SqlDataAdapter myrfDataAdapter = new SqlDataAdapter(myRenewalCommand);
        //            myrfDataAdapter.Fill(ds);

        //            sw.WriteLine(DateTime.Now.ToString() + ": Renewal Mail Send Start 1");
        //            try { 

        //                        //SendEmail();
        //                foreach (DataRow dRow in ds.Tables[0].Rows)
        //                {
        //                    //Console.WriteLine("Test 1");
        //                    //Console.ReadLine();
        //                    var RenewalId = dRow["Id"].ToString();
        //                    var EstId = dRow["EstId"].ToString();
        //                    var Est_Name = dRow["Est_Name"].ToString();
        //                    var PermitNumber = dRow["PermitNumber"].ToString();
        //                    var Owner_Name = dRow["Owner"].ToString();
        //                    var Email = dRow["Email"].ToString();
        //                    var FeesAmount = dRow["FeesAmount"].ToString();
        //                    var Title = dRow["Title"].ToString();
        //                    var PaymentId = dRow["PaymentId"].ToString();
        //                    //var FeesAmount = dRow["FeesAmount"];

        //                    InvoicePdfVM model = new InvoicePdfVM();
        //                    model.Payment = _paymentRepo.GetFirstOrDefault(filter: x => x.Id == Convert.ToInt32(PaymentId), includeProperties: "Fees");
        //                    model.Establishment = _establishment.GetFirstOrDefault(filter: x => x.Id == model.Payment.EstablishmentId);
        //                    TimeZoneInfo centralZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
        //                    model.InvoiceDate = model.Payment.InvoiceDate!.Value.ToShortDateString() + " " + TimeZoneInfo.ConvertTime(model.Payment.InvoiceDate.Value, centralZone).ToShortTimeString();
        //                    var user = _users.GetFirstOrDefaultAsync(filter: x => x.Id == model.Payment.InvoiceBy);
        //                    model.InvoiceCreatedBy = user.Result.FirstName + " " + user.Result.LastName;
        //                    model.FeesDetails = _feesDetailsTable.GetAll(filter: x => x.FeesId == model.Payment.Fees!.Id);
        //                    model.PaymentDetails = _paymentDetailsTable.GetAll(filter: x => x.PaymentId == model.Payment.Id);

        //                    foreach (var item in model.FeesDetails)
        //                    {
        //                        if (item.EstablishmentTypeId != null)
        //                        {
        //                            var EstType = _estTypes.GetFirstOrDefault(filter: x => x.Id == item.EstablishmentTypeId, includeProperties: "Jurisdiction");
        //                            var Jurisdiction = new JurisdictionAccounts();
        //                            if (EstType != null)
        //                            {
        //                                Jurisdiction = _jurisdictionaccounts.GetFirstOrDefault(filter: x => x.Id == EstType.JurisdictionId, includeProperties: "Programs");
        //                            }
        //                            model.Jurisdiction = Jurisdiction.Name;
        //                            model.Program = Jurisdiction.Programs!.Name;
        //                        }
        //                    }

        //                    var invoiceFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"RenewalLetter\RenewalInvoice.html");
        //                    var invoiceFile = System.IO.File.ReadAllText(invoiceFilePath);
        //                    //string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"RenewalLetter\logoclient.png");
        //                    //byte[] imageBytes = File.ReadAllBytes(imagePath);
        //                    //string base64String = Convert.ToBase64String(imageBytes);
        //                    //invoiceFile = invoiceFile.Replace("{{ServerUrl}}", base64String);
        //                    invoiceFile = invoiceFile.Replace("{{Invoice_Number}}", model.Payment!.Fees!.InvoiceNo);
        //                    invoiceFile = invoiceFile.Replace("{{InvoiceDate}}", model.InvoiceDate);
        //                    invoiceFile = invoiceFile.Replace("{{Permit_Number}}", model.Establishment!.PermitNumber);
        //                    invoiceFile = invoiceFile.Replace("{{Est_Name}}", model.Establishment!.Name);

        //                    var cancelVoidMsg = "";
        //                    if (model.Payment.PaymentStatus == 3)
        //                    {
        //                        cancelVoidMsg = "<div style = 'color:Red; width:100%; text-align:center; padding-top:70px'>< h1 > CANCELLED </ h1 ></ div >";


        //                    }
        //                    else if (model.Payment.PaymentStatus == 6)
        //                    {
        //                        cancelVoidMsg = "<div style = 'color:Red; width:100%; text-align:center; padding-top:70px'>< h1 > VOIDED </ h1 ></ div >" +
        //                                        "< div style = 'color:Red; width:100%; text-align:center'><h3 > @Model.Payment!.ReasonForRefundVoid </ h3 ></ div >";
        //                    }

        //                    invoiceFile = invoiceFile.Replace("{{Void_Cancel_Mssge}}", cancelVoidMsg);


        //                    invoiceFile = invoiceFile.Replace("{{Payment_Amount}}", (model.Payment.Amount) < 0 ? "- $ " + System.Math.Abs((model.Payment.Amount)).ToString("N2") : "$ " + model.Payment.Amount.ToString("N2"));
        //                    invoiceFile = invoiceFile.Replace("{{Invoice_CreatedBy}}", model.InvoiceCreatedBy);
        //                    invoiceFile = invoiceFile.Replace("{{Program}}", model.Program);
        //                    invoiceFile = invoiceFile.Replace("{{Jurisdiction}}", model.Jurisdiction);

        //                    var PaymentBreakTableBody = "";
        //                    if (model.PaymentDetails!.Any())
        //                    {
        //                        foreach (var item in model.PaymentDetails!)
        //                        {
        //                            PaymentBreakTableBody += ("<tr> " +
        //                                          "<td>" + item.Title + "</td>" +
        //                                          "<td>" + item.Amount.ToString("N2") + "</td>" +
        //                                    "</tr>");
        //                        }
        //                    }

        //                    invoiceFile = invoiceFile.Replace("{{PaymentBreakTableBody}}", PaymentBreakTableBody);

        //                    OwnerEmailVM emailModel = new OwnerEmailVM();
        //                    emailModel.ToMail = Email;
        //                    emailModel.CCMail = "";
        //                    emailModel.Subject = "Renewal Letter";
        //                    emailModel.htmlCode = invoiceFile;
        //                    var flag = SendEmail(emailModel);
        //                    //var flag = await SendEmail(emailModel);

        //                    //Console.WriteLine(flag.Result);
        //                    //Console.ReadLine();
        //                    //if (flag.Result == true)
        //                    if (flag == true)
        //                    {
        //                        //Console.WriteLine(EstId);
        //                        //Console.ReadLine();

        //                        //try
        //                        //{
        //                        //      var Est = _establishment.GetFirstOrDefault(filter: x => x.Id == Convert.ToInt32(EstId));
        //                        //      Est.PermitStatusId = 10;
        //                        //      Est.SyncDate = DateTime.Now;
        //                        //      _establishment.Update(Est);
        //                        //}
        //                        //catch (Exception ex)
        //                        //{
        //                        //      Console.WriteLine(ex.Message);
        //                        //      Console.ReadLine();
        //                        //}


        //                        //Console.WriteLine("Test 4 " + flag.Result);
        //                        //Console.ReadLine();

        //                        //var pay = _paymentRepo.GetFirstOrDefault(filter: x => x.Id == Convert.ToInt32(PaymentId));
        //                        //pay.PaymentStatus = 1;
        //                        //_paymentRepo.Update(pay);

        //                        //Console.WriteLine("Test 5 " + flag.Result);
        //                        //Console.ReadLine();

        //                        //var payDetails = _paymentDetailsTable.GetFirstOrDefault(filter: x => x.PaymentId == Convert.ToInt32(PaymentId));
        //                        //payDetails.PaymentStatus = 1;
        //                        //_paymentDetailsTable.Update(payDetails);

        //                        //Console.WriteLine("Test 6 " + flag.Result);
        //                        //Console.ReadLine();

        //                        //var renewalHistory = _renewalTempHistory.GetFirstOrDefault(filter: x => x.Id == Convert.ToInt32(RenewalId));
        //                        //renewalHistory.PermitStatus = 10;
        //                        //_renewalTempHistory.Update(renewalHistory);

        //                        //Console.WriteLine("Test 7 " + flag.Result);
        //                        //Console.ReadLine();

        //                        var cmd = new SqlCommand("UpdateRenewalTable", myConnection);
        //                        cmd.CommandType = CommandType.StoredProcedure;
        //                        cmd.Parameters.AddWithValue("@RenewalId", RenewalId);
        //                        cmd.ExecuteNonQuery();
        //                    }
        //                }
        //            }catch(Exception ex){
        //                sw.WriteLine(DateTime.Now.ToString() + ": " + ex.Message);
        //            }


        //                        sw.WriteLine(DateTime.Now.ToString() + ": Renewal Mail Send End");
        //                        sw.WriteLine();

        //                        sw.WriteLine(DateTime.Now.ToString() + ": Job End");
        //                  }
        //                  Console.WriteLine("Executed");
        //                  Console.ReadKey();
        //            }


        //      }
        //      catch (Exception ex)
        //      {

        //            Console.WriteLine(ex.StackTrace.ToString());
        //            Console.ReadKey();
        //      }
        //      finally
        //      {
        //            using (SqlConnection myConnection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["BrazosAppCronJobs"].ConnectionString))
        //            {
        //                  myConnection.Open();
        //                  var cmd = new SqlCommand("CheckIfAnyMoreEstPresentForRenewal", myConnection);
        //                  cmd.CommandType = CommandType.StoredProcedure;
        //                  var count = cmd.ExecuteScalar().ToString();
        //                  if(Convert.ToInt32(count) > 0)
        //                  {
        //                        //Task task = Run(args);
        //                        Run(args);
        //                  }
        //            }
        //      }
        //}


        /// <summary>
        /// Temp

        [STAThread]
        //public async Task Run(string[] args)
        public void Run(string[] args)
        {
            //PrivateSendEmail("santanu.roy334@gmail.com", "roy@inspect2go.com", "Test Test", "Test Body", Array.Empty<Byte[]>(), Array.Empty<String>());
            //Console.WriteLine("Executed");
            //Console.ReadKey();

            try
            {
                using (SqlConnection myConnection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["BrazosAppCronJobs"].ConnectionString))
                {
                    myConnection.Open();

                    var myRenewalCommand = new SqlCommand("RenewalPermitList", myConnection);
                    myRenewalCommand.CommandType = CommandType.StoredProcedure;

                    DataSet ds = new DataSet();
                    SqlDataAdapter myrfDataAdapter = new SqlDataAdapter(myRenewalCommand);
                    myrfDataAdapter.Fill(ds);
                    //Console.WriteLine("i am here 1");
                    //Console.ReadKey();
                    foreach (DataRow dRow in ds.Tables[0].Rows)
                    {
                        //Console.WriteLine("i am here 2");
                        //Console.ReadKey();
                        var RenewalId = dRow["Id"].ToString();
                        var EstId = dRow["EstId"].ToString();
                        var Est_Name = dRow["Est_Name"].ToString();
                        var Est_Address = dRow["Address"].ToString();
                        var Est_City = dRow["City"].ToString();
                        var Est_State = dRow["State"].ToString();
                        var Est_Zip = dRow["Zip"].ToString();
                        var PermitNumber = dRow["PermitNumber"].ToString();
                        var Owner_Name = dRow["Owner"].ToString();
                        var Email = dRow["Email"].ToString();
                        var FeesAmount = dRow["FeesAmount"].ToString();
                        var Title = dRow["Title"].ToString();
                        var PaymentId = dRow["PaymentId"].ToString();
                        //var FeesAmount = dRow["FeesAmount"];

                        InvoicePdfVM model = new InvoicePdfVM();
                        model.Payment = _paymentRepo.GetFirstOrDefault(filter: x => x.Id == Convert.ToInt32(PaymentId), includeProperties: "Fees");
                        // model.Establishment = _establishment.GetFirstOrDefault(filter: x => x.Id == model.Payment.EstablishmentId);
                        TimeZoneInfo centralZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
                        model.InvoiceDate = model.Payment.InvoiceDate!.Value.ToShortDateString() + " " + TimeZoneInfo.ConvertTime(model.Payment.InvoiceDate.Value, centralZone).ToShortTimeString();
                        var user = _users.GetFirstOrDefaultAsync(filter: x => x.Id == model.Payment.InvoiceBy);
                        model.InvoiceCreatedBy = user.Result.FirstName + " " + user.Result.LastName;
                        model.FeesDetails = _feesDetailsTable.GetAll(filter: x => x.FeesId == model.Payment.Fees!.Id);
                        model.PaymentDetails = _paymentDetailsTable.GetAll(filter: x => x.PaymentId == model.Payment.Id);

                        //Console.WriteLine("i am here 3 " + Email);
                        //Console.ReadKey();

                        foreach (var item in model.FeesDetails)
                        {
                            if (item.EstablishmentTypeId != null)
                            {
                                var EstType = _estTypes.GetFirstOrDefault(filter: x => x.Id == item.EstablishmentTypeId, includeProperties: "Jurisdiction");
                                var Jurisdiction = new JurisdictionAccounts();
                                if (EstType != null)
                                {
                                    Jurisdiction = _jurisdictionaccounts.GetFirstOrDefault(filter: x => x.Id == EstType.JurisdictionId, includeProperties: "Programs");
                                }
                                model.Jurisdiction = Jurisdiction.Name;
                                model.Program = Jurisdiction.Programs!.Name;
                            }
                        }
                        //Console.WriteLine("i am here 4");
                        //Console.ReadKey();

                        var invoiceFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"RenewalLetter\RenewalInvoice.html");
                        var invoiceFile = System.IO.File.ReadAllText(invoiceFilePath);
                        //string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"RenewalLetter\logoclient.png");
                        //byte[] imageBytes = File.ReadAllBytes(imagePath);
                        //string base64String = Convert.ToBase64String(imageBytes);
                        //invoiceFile = invoiceFile.Replace("{{ServerUrl}}", base64String);

                        //invoiceFile = invoiceFile.Replace("{{Invoice_Number}}", model.Payment!.Fees!.InvoiceNo);
                        //invoiceFile = invoiceFile.Replace("{{InvoiceDate}}", model.InvoiceDate);
                        //invoiceFile = invoiceFile.Replace("{{Permit_Number}}", model.Establishment!.PermitNumber);
                        //invoiceFile = invoiceFile.Replace("{{Est_Name}}", model.Establishment!.Name);

                        invoiceFile = invoiceFile.Replace("{{Invoice_Number}}", model.Payment!.Fees!.InvoiceNo);
                        invoiceFile = invoiceFile.Replace("{{InvoiceDate}}", model.InvoiceDate);
                        invoiceFile = invoiceFile.Replace("{{Permit_Number}}", PermitNumber);
                        invoiceFile = invoiceFile.Replace("{{Est_Name}}", Est_Name);
                        invoiceFile = invoiceFile.Replace("{{Est_Address}}", Est_Address + ", " + Est_City + ", " + Est_State + ", " + Est_Zip);

                        var cancelVoidMsg = "";
                        if (model.Payment.PaymentStatus == 3)
                        {
                            cancelVoidMsg = "<div style = 'color:Red; width:100%; text-align:center; padding-top:70px'>< h1 > CANCELLED </ h1 ></ div >";


                        }
                        else if (model.Payment.PaymentStatus == 6)
                        {
                            cancelVoidMsg = "<div style = 'color:Red; width:100%; text-align:center; padding-top:70px'>< h1 > VOIDED </ h1 ></ div >" +
                                            "< div style = 'color:Red; width:100%; text-align:center'><h3 > @Model.Payment!.ReasonForRefundVoid </ h3 ></ div >";
                        }

                        invoiceFile = invoiceFile.Replace("{{Void_Cancel_Mssge}}", cancelVoidMsg);


                        invoiceFile = invoiceFile.Replace("{{Payment_Amount}}", (model.Payment.Amount) < 0 ? "- $ " + System.Math.Abs((model.Payment.Amount)).ToString("N2") : "$ " + model.Payment.Amount.ToString("N2"));
                        invoiceFile = invoiceFile.Replace("{{Invoice_CreatedBy}}", model.InvoiceCreatedBy);
                        invoiceFile = invoiceFile.Replace("{{Program}}", model.Program);
                        invoiceFile = invoiceFile.Replace("{{Jurisdiction}}", model.Jurisdiction);

                        var PaymentBreakTableBody = "";
                        if (model.PaymentDetails!.Any())
                        {
                            foreach (var item in model.PaymentDetails!)
                            {
                                PaymentBreakTableBody += ("<tr> " +
                                              "<td>" + item.Title + "</td>" +
                                              "<td>" + item.Amount.ToString("N2") + "</td>" +
                                        "</tr>");
                            }
                        }

                        //Console.WriteLine("i am here 5");
                        //Console.ReadKey();

                        invoiceFile = invoiceFile.Replace("{{PaymentBreakTableBody}}", PaymentBreakTableBody);

                        OwnerEmailVM emailModel = new OwnerEmailVM();
                        emailModel.ToMail = Email;
                        emailModel.CCMail = "";
                        emailModel.Subject = "Renewal Letter";
                        emailModel.htmlCode = invoiceFile;

                        /////
                        var flag = SendEmail(emailModel);
                        /////

                        //Console.WriteLine("i am here 6: " + flag.ToString());
                        //Console.ReadKey();

                        //////
                        if (flag == true)
                        {
                            var cmd = new SqlCommand("UpdatePermitStatusForRenewal", myConnection);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@RenewalId", RenewalId);
                            cmd.ExecuteNonQuery();
                        }
                        /////

                    }

                    Console.WriteLine("Executed");
                    //Console.ReadKey();
                }


            }
            catch (Exception ex)
            {
                Run(args);
                //Console.WriteLine(ex.StackTrace!.ToString());
                //Console.ReadKey();

                //using (SqlConnection myConnection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["BrazosAppCronJobs"].ConnectionString))
                //{
                //    myConnection.Open();
                //    var cmd = new SqlCommand("RenewalPermitEsistsInTempHistory", myConnection);
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    var count = cmd.ExecuteScalar().ToString();
                //    if (Convert.ToInt32(count) > 0)
                //    {
                //        //Task task = Run(args);
                //        Run(args);
                //    }
                //}
                //Console.WriteLine(ex.StackTrace.ToString());
                //Console.ReadKey();
            }
            finally
            {
                using (SqlConnection myConnection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["BrazosAppCronJobs"].ConnectionString))
                {
                    myConnection.Open();
                    var cmd = new SqlCommand("RenewalPermitEsistsInTempHistory", myConnection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    var count = cmd.ExecuteScalar().ToString();
                    if (Convert.ToInt32(count) > 0)
                    {
                        //Task task = Run(args);
                        Run(args);
                    }
                }
            }
        }       

        //[STAThread]
        //async Task<bool> SendEmail(OwnerEmailVM model)
        public bool SendEmail(OwnerEmailVM model)
        {
            try
            {
                //Console.WriteLine("Test 2");
                //Console.ReadLine();
                var htmlFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"RenewalLetter\RenewalLetter.html");


                // Read HTML content from file
                var htmlContent = System.IO.File.ReadAllText(htmlFilePath);
                htmlContent = htmlContent.Replace("{{Year_New}}", DateTime.Now.Date < new DateTime(Convert.ToInt32(DateTime.Now.Year), 12, 31) ? (DateTime.Now.AddYears(1).Year).ToString() : DateTime.Now.Year.ToString());
                htmlContent = htmlContent.Replace("{{ServerUrl}}", _configuration["ServerUrl"]);


                //var generatedPdf = ConvertHtmlToPdfAsync(model.htmlCode!);
                //////
                var generatedPdf = ConvertHtmlToPdf(model.htmlCode!);
                if(generatedPdf != null)
                {
                    Byte[][] bytearray = new Byte[][] { generatedPdf };
                    string[] pdfName = new string[] { "Invoice.Pdf" };
                    //////
                    //Console.WriteLine("Email Test 6 " + model.ToMail);
                    //Console.ReadKey();

                    //await _emailSender.SendEmail(model.ToMail!, model.CCMail, model.Subject!, htmlContent, bytearray, pdfName);

                    PrivateSendEmail(model.ToMail!, model.CCMail, model.Subject!, htmlContent, bytearray, pdfName);
                    //PrivateSendEmail(model.ToMail!, model.CCMail, model.Subject!, htmlContent, Array.Empty<Byte[]>(), Array.Empty<String>());
                    return true;
                }
               
                return false;
            }
            catch (Exception ex)
            {
                //Console.WriteLine("SendEmail: "  + ex.Message);
                return false;
            }
        }

        public void PrivateSendEmail(string toEmail, string? ccEmail, string subject, string body, Byte[][]? bytesArray, string[]? pdfName)
        {

            var MailIds = toEmail.Split(',');
            MailMessage message = new MailMessage();
            message.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["FromAddress"]!.ToString(), System.Configuration.ConfigurationManager.AppSettings["DisplayMail"]!.ToString());

            for (int i = 0; i < MailIds.Length; i++)
            {
                message.To.Add(new MailAddress(MailIds[i]));
            }
            if (ccEmail != null && ccEmail != "")
            {
                var ccMailIds = ccEmail.Split(',');
                for (int i = 0; i < ccMailIds.Length; i++)
                {
                    message.CC.Add(new MailAddress(ccMailIds[i]));
                }
            }

            if(System.Configuration.ConfigurationManager.AppSettings["CC1"]!.ToString() != "")
            {
                message.CC.Add(new MailAddress(System.Configuration.ConfigurationManager.AppSettings["CC1"]!.ToString()));
            }
            if (System.Configuration.ConfigurationManager.AppSettings["CC2"]!.ToString() != "")
            {
                message.CC.Add(new MailAddress(System.Configuration.ConfigurationManager.AppSettings["CC2"]!.ToString()));
            }
            if (System.Configuration.ConfigurationManager.AppSettings["BCC"]!.ToString() != "")
            {
                message.Bcc.Add(new MailAddress(System.Configuration.ConfigurationManager.AppSettings["BCC"]!.ToString()));
            }

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
            }
            
            message.IsBodyHtml = true;            
            var smtpclient = new SmtpClient(System.Configuration.ConfigurationManager.AppSettings["Server"]!.ToString())
            {
                Port = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["Port"]!.ToString()),
                Credentials = new NetworkCredential(System.Configuration.ConfigurationManager.AppSettings["Username"]!.ToString(), System.Configuration.ConfigurationManager.AppSettings["Password"]!.ToString()),
                EnableSsl = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["SSL"]!.ToString()),
            };
            string Userstate = "Sent";
            try
            {
                smtpclient.SendAsync(message, Userstate);
            }
            catch (Exception ex) {
                Console.WriteLine("PrivateSendEmail: " + ex.Message);
            }

        }

        //private static byte[] ConvertHtmlToPdf(string htmlContent)
        //{
        //      //Console.WriteLine("Test 3");
        //      //Console.ReadLine();
        //      var converter = new BasicConverter(new PdfTools());
        //      var document = new HtmlToPdfDocument()
        //      {
        //            GlobalSettings = {
        //              ColorMode = ColorMode.Color,
        //              PaperSize = PaperKind.A4,
        //              Orientation = Orientation.Portrait,
        //            },
        //            Objects = {
        //            new ObjectSettings
        //                {
        //                    HtmlContent = htmlContent,
        //                    //WebSettings = {
        //                    //    DefaultEncoding = "utf-8",
        //                    //    LoadImages = true, 
        //                    //}
        //                }
        //            }
        //      };
        //      //Console.WriteLine("Test 7");
        //      //Console.ReadKey();



        //      return converter.Convert(document);
        //}

        private static readonly object _lock = new object();

        [DllImport("ole32.dll")]
        private static extern int CoInitialize(IntPtr pvReserved);

        [DllImport("ole32.dll")]
        private static extern void CoUninitialize();

        private static byte[] ConvertHtmlToPdf(string htmlContent)
        {

            CoInitialize(IntPtr.Zero);
            //lock (_lock)
            //{
            //    var converter = new BasicConverter(new PdfTools());
            //    var document = new HtmlToPdfDocument()
            //    {
            //        GlobalSettings = {
            //    ColorMode = ColorMode.Color,
            //    PaperSize = PaperKind.A4,
            //    Orientation = Orientation.Portrait,
            //},
            //        Objects = {
            //    new ObjectSettings { HtmlContent = htmlContent }
            //}
            //    };
            //    return converter.Convert(document);
            //}

            try
            {
                lock (_lock)
                {
                    var converter = new BasicConverter(new PdfTools());
                    var document = new HtmlToPdfDocument()
                    {
                        GlobalSettings = {
                        ColorMode = ColorMode.Color,
                        PaperSize = PaperKind.A4,
                        Orientation = Orientation.Portrait,
                    },
                        Objects = {
                        new ObjectSettings { HtmlContent = htmlContent }
                    }
                    };
                    return converter.Convert(document);
                }
                //var converter = new BasicConverter(new PdfTools());
                //    var document = new HtmlToPdfDocument()
                //    {
                //        GlobalSettings = {
                //            ColorMode = ColorMode.Color,
                //            PaperSize = PaperKind.A4,
                //            Orientation = Orientation.Portrait,
                //        },
                //        Objects = {
                //            new ObjectSettings { HtmlContent = htmlContent }
                //        }
                //    };

                //    return converter.Convert(document);
            }
            catch (Exception ex)
            { 
                Console.WriteLine(ex.Message);
                return null;
            }
            finally
            {
                CoUninitialize();  // Uninitialize OLE after conversion
            }
        }


        //public static async Task<byte[]> ConvertHtmlToPdfAsync(string htmlContent)
        //{
        //    // Automatically downloads the latest stable version of Chromium
        //    var browserFetcher = new BrowserFetcher();
        //    await browserFetcher.DownloadAsync(); // This will fetch the latest revision automatically.

        //    var browser = await Puppeteer.LaunchAsync(new LaunchOptions
        //    {
        //        Headless = true // Ensure the browser runs in headless mode.
        //    });

        //    var page = await browser.NewPageAsync();
        //    await page.SetContentAsync(htmlContent);

        //    // Generate PDF
        //    var pdfBytes = await page.PdfDataAsync(new PdfOptions
        //    {
        //        Format = PaperFormat.A4,
        //        PrintBackground = true // Prints background colors
        //    });

        //    await browser.CloseAsync();

        //    return pdfBytes;
        //}
    }
}
