using BrazosApp.DataAccess.Repository.IRepository;
using BrazosApp.Extensions;
using BrazosApp.Models;
using BrazosApp.Models.Jetpay;
using BrazosApp.Utility.Helpers;
using BrazosApp.Utility.Services;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static BrazosApp.Models.Jetpay.RedirectRequestModel;

namespace BrazosApp.Services
{
    public class JetPayService: IJetPayService
    {
        private readonly IConfiguration _configuration;
        private readonly IRepository<Payment> _paymentRepo;
        private readonly IRepository<EstablishmentOwner> _owner;
        private readonly IRepository<Fees> _fees;
        private readonly IRepository<FeesDetailsTable> _feesDetailsTable;
        private readonly IRepository<PaymentDetailsTable> _paymentDetailsTable;
        private readonly IEmailSenderService _emailSenderService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEncrypt _encrypt;
        public JetPayService(IConfiguration configuration, IRepository<Payment> paymentRepo, IRepository<EstablishmentOwner> owner, IRepository<Fees> fees,
            IRepository<FeesDetailsTable> feesDetailsTable, IRepository<PaymentDetailsTable> paymentDetailsTable,
            IEmailSenderService emailSenderService, IWebHostEnvironment webHostEnvironment, IEncrypt encrypt)
        {
            _configuration = configuration;
            _paymentRepo = paymentRepo;
            _owner = owner;
            _fees = fees;
            _feesDetailsTable = feesDetailsTable;
            _paymentDetailsTable = paymentDetailsTable;
            _emailSenderService = emailSenderService;
            _webHostEnvironment = webHostEnvironment;
            _encrypt = encrypt;
        }

        /// JET PAY START        
        public async Task<bool> PaymentProcess(string id)
        {
            //var paymentStatus = false;
            ////var payment = await _paymentRepo.GetById(id);
            ////if (payment == null)
            ////{
            ////    paymentStatus = false;
            ////}

            //var feesId = Convert.ToInt32(_encrypt.Decrypt256(id));
            //var fees = await _fees.GetFirstOrDefaultAsync(filter: x => x.Id == feesId);
            //if (fees == null)
            //{
            //    paymentStatus = false;
            //    return paymentStatus;
            //}

            //var feesDetails = await _feesDetailsTable.GetAllAsync(filter: x => x.FeesId == fees.Id);


            ////var fees = await _fees.GetFirstOrDefaultAsync(filter: x => x.Id == payment.FeesId);
            //var owner = await _owner.GetFirstOrDefaultAsync(filter: x => x.EstablishmentId == fees.EstablishmentId);

            //Payment payment = new Payment();
            //payment.InvoiceNo = fees!.InvoiceNo!;
            //payment.FeesId = fees.Id;
            //payment.Amount = fees!.Amount;
            //payment.PaymentStatus = 1;
            ////payment.PaymentType = 1;
            //await _paymentRepo.AddAsync(payment);

            //foreach (var feesDets in feesDetails)
            //{
            //    var paymentDets = new PaymentDetailsTable();
            //    paymentDets.PaymentId = payment.Id;
            //    paymentDets.Amount = feesDets.Amount;
            //    paymentDets.Title = feesDets.Title;
            //    paymentDets.PaymentStatus = payment.PaymentStatus;
            //    await _paymentDetailsTable.AddAsync(paymentDets);
            //}

            //var PaymentDetails = await _paymentDetailsTable.GetAllAsync(filter: x => x.PaymentId == payment.Id);

            ////create pending payment
            //RedirectRequestModel redirectRequest = new RedirectRequestModel();

            //redirectRequest.clientKey = _configuration["JetPayStage:APIClientKey"];
            //redirectRequest.transactionIdentifier = payment!.InvoiceNo;
            //redirectRequest.collectionMode = Convert.ToInt32(_configuration["JetPayStage:CollectionMode"]);
            //redirectRequest.amount = payment.Amount;

            //redirectRequest.billing = new RedirectRequestModel.Address();
            //redirectRequest.billing.name = owner.Name!;
            //redirectRequest.billing.address = owner.MailingAddress!;
            //redirectRequest.billing.city = owner.City!;
            //redirectRequest.billing.state = owner.State!;
            //redirectRequest.billing.zip = owner.Zip!;
            //redirectRequest.billing.email = owner.EmailId!;
            ////redirectRequest.billing.phone = owner.ContactNo;
            //redirectRequest.billing.phone = "";

            ////redirectRequest.lineItems = new RedirectRequestModel.LineItem[PaymentDetails.Count()];

            ////if (PaymentDetails.Any())
            ////{
            ////    int index = 0;
            ////    foreach (var item in PaymentDetails)
            ////    {
            ////        redirectRequest.lineItems[index++] = new RedirectRequestModel.LineItem
            ////        {
            ////            identifiers = new string[] { item.Title!, item.Title! },
            ////            amount = Math.Abs(item.Amount),
            ////            paymentType = _configuration["JetPayStage:PaymentType"]
            ////        };
            ////    }
            ////}
            ////redirectRequest.lineItems = new RedirectRequestModel.LineItem[] {
            ////    foreach(var items in PaymentDetails)
            ////    {
            ////        new RedirectRequestModel.LineItem()
            ////        {
            ////            identifiers = new string[] { payment.InvoiceNo },
            ////            amount = payment.Amount,
            ////            paymentType = _configuration["JetPayStage:PaymentType"]
            ////        }
            ////    }
            ////};

            //redirectRequest.lineItems = new RedirectRequestModel.LineItem[] {
            //      new RedirectRequestModel.LineItem()
            //      {
            //            identifiers = new string[] { payment.InvoiceNo },
            //            amount = payment.Amount,
            //            paymentType = _configuration["JetPayStage:PaymentType"]
            //      }
            //};

            //redirectRequest.csiUserId = Convert.ToInt32(_configuration["JetPayStage:csiUserId"]);
            //redirectRequest.notes = "NCR Test";
            //redirectRequest.urlReturnPost = _configuration.GetValue<string>("BaseUrl") + "/PaymentStatus?Id=" + _encrypt.Encrypt256(payment.Id.ToString());
            //redirectRequest.urlSilentPost = "";

            //// The payment method that will be enabled for the transaction. 0 = Block(eCheck and Credit Card), 1 = Accept(eChecks Only), 2 = Accept(Credit Cards Only), 3 = Accept(eCheck and Credit Card) 
            //redirectRequest.allowedPaymentMethod = 2;

            ////Serialize the request model into a Json string for the web request
            //string jsonInput = JsonConvert.SerializeObject(redirectRequest);

            ////Setup the WebRequest with the API process method to be used
            //WebRequest request = WebRequest.Create("https://stage.collectorsolutions.com/magic-api/api/transaction/redirect");

            ////Set timeout, content type and http method type
            //request.Timeout = 120000;
            //request.ContentType = "application/json";
            //request.Method = "POST";

            ////Setup a stream writer to push the information to the server
            //using (StreamWriter requestStream = new StreamWriter(request.GetRequestStream()))
            //{
            //    //Write the data
            //    requestStream.Write(jsonInput);
            //}

            ////Create the string for the returning json string object
            //string jsonOutput = string.Empty;

            ////Setup the WebResponse object to accept the returning information
            //using (WebResponse response = request.GetResponse())
            //{
            //    //Setup a StreamReader object to read the inbound information
            //    StreamReader responseStream = new StreamReader(response.GetResponseStream());

            //    // Read it to a string
            //    jsonOutput = responseStream.ReadToEnd();
            //}

            ////Deserialize it to a response model
            //RedirectResponseModel redirectResponse = JsonConvert.DeserializeObject<RedirectResponseModel>(jsonOutput);

            //if (redirectResponse == null)
            //{
            //    paymentStatus = false;
            //}
            //else
            //{
            //    payment.RedirectApiCallStatus = redirectResponse.status;
            //    payment.RedirectApiCallOn = System.DateTime.Now;

            //    if (redirectResponse.status.ToLower() == "error")
            //    {
            //        payment.RedirectApiMessage = redirectResponse.errors[0].message.ToString();

            //        paymentStatus = false;
            //    }
            //    else
            //    {
            //        string emailContent = new EmailFilePathGenerate(_webHostEnvironment).EmailFilePath(@"EmailTemplates\EmailPayment.html");
            //        emailContent = emailContent.Replace("{{OwnerName}}", owner.Name);
            //        emailContent = emailContent.Replace("{{PaymentRedirectUrl}}", "http://stage.collectorsolutions.com/magic-ui/PaymentRedirect/" + _configuration["JetPayStage:WebClientKey"] + "/" + payment.InvoiceNo);
            //        emailContent = emailContent.Replace("{{ServerUrl}}", _configuration.GetValue<string>("ServerUrl"));
            //        await _emailSenderService.SendEmail(owner!.EmailId!, null, "Payment", emailContent, null, null);

            //        paymentStatus = true;
            //    }
            //}

            //await _paymentRepo.UpdateAsync(payment);

            //return paymentStatus;
            return true;
        }
        /// JET PAY END
    }
}
