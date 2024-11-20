using BrazosApp.DataAccess.Data;
using BrazosApp.DataAccess.Repository.IRepository;
using BrazosApp.Extensions;
using BrazosApp.Models;
using BrazosApp.Models.DTOs;
using BrazosApp.Models.ViewModels;
using BrazosApp.Utility;
using BrazosApp.Utility.Helpers;
using BrazosApp.Utility.Services;
//using BrazosApp.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Data;
using System.Diagnostics;
using System.Security.Claims;
using System.Text;
using Wkhtmltopdf.NetCore;

namespace BrazosApp.Controllers
{
    public class HomeController : Controller
      {
            private readonly ILogger<HomeController> _logger;
            private readonly ApplicationDbContext _context;
            private readonly IRepository<Users> _user;
            private readonly IRepository<Role> _role;
            private readonly IRepository<ApplicationType> _applicationtype;
            private readonly IRepository<ApplicationFor> _applicationfor;
            private readonly IRepository<Application> _application;
            private readonly IRepository<Establishment> _establishment;
            private readonly IPasswordHasher _passwordHasher;
            private readonly IPasswordGenerator _randomPassword;
            private readonly IEmailSenderService _emailSenderService;
            private readonly IWebHostEnvironment _webHostEnvironment;
            private readonly IConfiguration _configuration;
            private readonly IEncrypt _encrypt;

            private readonly IRepository<Fees> _feesRepo;
            private readonly IRepository<FeesDetailsTable> _feesDetailsTable;
            private readonly IRepository<Payment> _paymentRepo;
            private readonly IRepository<PaymentDetailsTable> _paymentDetailsTable;
        private readonly IGeneratePdf _generatePdf;
            public HomeController(ILogger<HomeController> logger,
                IPasswordHasher passwordHasher,
                ApplicationDbContext context,
                IPasswordGenerator randomPassword,
                IEmailSenderService emailSenderService,
                IWebHostEnvironment webHostEnvironment,
                IConfiguration configuration,
                IRepository<Users> user,
                IRepository<Role> role,
                IRepository<ApplicationType> applicationtype,
                IRepository<ApplicationFor> applicationfor,
                IRepository<Application> application,
                IRepository<Establishment> establishment,
                IEncrypt encrypt,
                IRepository<Fees> feesRepo,
                IRepository<FeesDetailsTable> feesDetailsTable,
                IRepository<Payment> paymentRepo,
                IRepository<PaymentDetailsTable> paymentDetailsTable,
                IGeneratePdf generatePdf
                )
            {
                  _logger = logger;
                  _context = context;
                  _passwordHasher = passwordHasher;
                  _randomPassword = randomPassword;
                  _emailSenderService = emailSenderService;
                  _webHostEnvironment = webHostEnvironment;
                  _configuration = configuration;
                  _user = user;
                  _role = role;
                  _applicationtype = applicationtype;
                  _applicationfor = applicationfor;
                  _application = application;
                  _establishment = establishment;
                  _encrypt = encrypt;
                  _feesRepo = feesRepo;
                  _feesDetailsTable = feesDetailsTable;
                  _paymentRepo = paymentRepo;
                  _paymentDetailsTable = paymentDetailsTable;
                  _generatePdf = generatePdf;
            }

            public async Task<IActionResult> Index()
            {
                  ApplicationVM model = new ApplicationVM();
                  model.Application = new Application();
                  var Applications = await _applicationfor.GetAllAsync();
                  model.ApplicationForList = Applications.Where(x => x.IsActive == true).Select(x => new SelectListItem
                  {
                        Text = x.Name,
                        Value = x.Id.ToString(),
                  }).ToList();
                  return View(model);
            }

            [Route("/Test")]
            public IActionResult Test()
            {
                  return View();
            }

            [Route("/Success/{id?}")]
            [HttpGet]
            public async Task<IActionResult> Success(string id)
            {
                  var AppId = "";
                  try
                  {
                        AppId = _encrypt.Decrypt256(id);
                  }
                  catch (Exception ex)
                  {
                        return Redirect("/Not_Found");
                  }

                  var Application = await _application.GetFirstOrDefaultAsync(filter: x => x.Id == Convert.ToInt32(AppId), includeProperties: "ApplicationFor");
                  if (Application == null) { return NotFound(); }
                  else
                  {
                        //var Est = await _establishment.GetFirstOrDefaultAsync(filter:x=>x.ApplicationId==id);
                        //Est.PermitStatusId = 2;
                        //Est.OldPermitStatusId = 1;
                        //await _establishment.UpdateAsync(Est);
                        ViewBag.Lang = Application.ApplicationFor!.LanguageTypeId;
                        string emailContent = "";
                        var subject = "";
                        if (Application.ApplicationFor!.LanguageTypeId == 1)
                        {
                              emailContent = new EmailFilePathGenerate(_webHostEnvironment).EmailFilePath(@"EmailTemplates\English\ApplicationSubmittedSuccessMail.html");
                              subject = "Your Application is successfully submitted";
                        }
                        else if (Application.ApplicationFor!.LanguageTypeId == 2)
                        {
                              emailContent = new EmailFilePathGenerate(_webHostEnvironment).EmailFilePath(@"EmailTemplates\Spanish\ApplicationSubmittedSuccessMail.html");
                              subject = "Su solicitud se envió exitosamente";
                        }
                        emailContent = emailContent.Replace("{{OwnerName}}", Application.OwnerName);
                        emailContent = emailContent.Replace("{{ApplicationNo}}", Convert.ToString(Application.ApplicationNo));
                        emailContent = emailContent.Replace("{{ServerUrl}}", _configuration.GetValue<string>("ServerUrl"));
                        await _emailSenderService.SendEmail(Application.EmailId!, null, subject, emailContent, Array.Empty<Byte[]>(), Array.Empty<String>());
                  }
                  return View();
            }


            [Route("/GetApplicationLink")]
            [HttpPost]
            public async Task<IActionResult> GetApplicationLink(ApplicationVM model)
            {
                  if (ModelState.IsValid)
                  {
                        if (model.Application != null)
                        {
                              model.Application.ApplicationDate = DateTime.Now;
                              var ApplicationCount = await _application.GetAllAsync();
                              model.Application.ApplicationNo = "BCHD" + "-" + (ApplicationCount.Count() + 1).ToString("D6");
                              try
                              {
                                    model.Application.Status = 1;
                                    await _application.AddAsync(model.Application);
                              }
                              catch (Exception ex) { }

                              string emailContent = new EmailFilePathGenerate(_webHostEnvironment).EmailFilePath(@"EmailTemplates\EmailVerification.html");
                              emailContent = emailContent.Replace("{{OwnerName}}", model.Application.OwnerName);
                              //emailContent = emailContent.Replace("{{Id}}", Convert.ToString(model.Application!.Id));
                              emailContent = emailContent.Replace("{{Id}}", _encrypt.Encrypt256(Convert.ToString(model.Application!.Id)));
                              emailContent = emailContent.Replace("{{BaseUrl}}", _configuration.GetValue<string>("BaseUrl"));
                              emailContent = emailContent.Replace("{{ServerUrl}}", _configuration.GetValue<string>("ServerUrl"));
                              try 
                              {
                                await _emailSenderService.SendEmail(model.Application.EmailId!, null, "Email Verification", emailContent, Array.Empty<Byte[]>(), Array.Empty<String>());
                              }
                              catch(Exception ex) { }

                              return Json(new { success = true, msg = "A confirmation link has been sent to your email. Please check to proceed further" });
                        }
                  }
                  return Json(new { success = false, msg = "An Unexpected Error Occurred" });
            }

            [Route("Verification/{id?}")]
            [HttpGet]
            public async Task<IActionResult> Verification(string id)
            {
                  if (id != null)
                  {
                        var appId = "";
                        try
                        {
                              appId = _encrypt.Decrypt256(id);
                        }
                        catch (Exception ex)
                        {
                              return Redirect("/Not_Found");
                        }
                        var Application = await _application.GetFirstOrDefaultAsync(filter: x => x.Id == Convert.ToInt32(appId), includeProperties: "ApplicationFor");
                        if (Application == null)
                        {
                              return NotFound();
                        }
                        else
                        {
                              var ApplicationId = _encrypt.Encrypt256(Convert.ToString(Application.Id));
                              if (Application.Status == 3)
                              {
                                    TempData["Code"] = Application.ApplicationFor!.Code;
                                    TempData["Purpose"] = Application.ApplicationFor!.Purpose;
                                    TempData["ApplicationId"] = ApplicationId;
                                    ViewBag.flg = 3;
                              }
                              else
                              {
                                    TempData["Code"] = Application.ApplicationFor!.Code;
                                    TempData["Purpose"] = Application.ApplicationFor!.Purpose;
                                    TempData["ApplicationId"] = ApplicationId;
                                    if (Application.Status == 1)
                                    {
                                          ViewBag.flg = 1;
                                          Application.Status = 2;
                                          await _application.UpdateAsync(Application);
                                    }
                                    else
                                    {
                                          ViewBag.flg = 2;
                                    }
                              }
                              return View();
                        }
                  }
                  return NotFound();
            }

            //public IActionResult Verification()
            //{
            //    ViewBag.flg = 1;
            //    return View();
            //}



            [Route("/Login")]
            [HttpGet]
            public IActionResult Login(string? returnUrl = null)
            {
                  Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
                  Response.Headers["Pragma"] = "no-cache";
                  Response.Headers["Expires"] = "0";
                  if (User.Identity!.IsAuthenticated)
                  {
                        return Redirect("/Dashboard");
                  }
                  ViewData["ReturnUrl"] = returnUrl;
                  return View();
            }

            [Route("/Login")]
            [HttpPost]
            public async Task<IActionResult> Login(LoginVM model, string? returnUrl = null)
            {
                  ViewData["ReturnUrl"] = returnUrl;
                  if (ModelState.IsValid)
                  {
                        var user = await _user.GetFirstOrDefaultAsync(filter: x => x.BHCD == model.LoginRequestDTO!.BHCD && x.IsActive == true && x.IsDelete == false);
                        if (user == null)
                        {
                              TempData["Error"] = "User Not Registered";
                              return View(model);
                        }
                        bool passwordmatch = _passwordHasher.VerifyPassword(model.LoginRequestDTO!.Password!, user.Salt!, user.Password!);
                        if (passwordmatch)
                        {
                              var role = await _role.GetFirstOrDefaultAsync(filter: x => x.Id == user.RoleId);
                              var encryptedId = _encrypt.Encrypt256(Convert.ToString(user.Id));
                              //if (v == null)
                              //{

                              //}
                              //else
                              //{
                              //    var token = ClaimsPrincipalGenerate.CreateJwtToken(user, encryptedId, _configuration.GetSection("JWTkey").Value);
                              //    //return Ok(new { Token = token });
                              //    user.IsLoggedIn = true;
                              //    await _user.UpdateAsync(user);
                              //    var client = new HttpClient();
                              //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer ", token);

                              //    return Ok(token);
                              //}
                              await HttpContext.SignInAsync(ClaimsPrincipalGenerate.Generate(user, role, encryptedId), ClaimsPrincipalGenerate.authpropgenerate(model.IsPersistent));
                              user.IsLoggedIn = true;
                              await _user.UpdateAsync(user);

                              if (returnUrl == null)
                              {
                                    return Redirect("/Dashboard");
                              }
                              return Redirect(returnUrl);
                        }
                        else
                        {
                              TempData["Error"] = "Wrong Credential";
                              return View(model);
                        }
                  }
                  return View(model);
            }


            [Route("/InspectorLogin")]
            [HttpGet]
            public IActionResult InspectorLogin(string? returnUrl = null)
            {
                  if (User.Identity!.IsAuthenticated)
                  {
                        return Redirect("/Inspector/Dashboard");
                  }
                  ViewData["ReturnUrl"] = returnUrl;
                  return View();
            }

            [Route("/InspectorLogin")]
            [HttpPost]
            public async Task<IActionResult> InspectorLogin(LoginVM model, string? returnUrl = null)
            {
                  ViewData["ReturnUrl"] = returnUrl;

                  using (var httpClient = new HttpClient())
                  {
                        StringContent stringContent = new StringContent(JsonConvert.SerializeObject(model.LoginRequestDTO), Encoding.UTF8, "application/json");
                        using (var response = await httpClient.PostAsync(_configuration.GetSection("APIUrl").Value + "/Login", stringContent))
                        {
                              var responseResult = await response.Content.ReadAsStringAsync();
                              var apiResponse = JsonConvert.DeserializeObject<APIResponse>(responseResult);
                              if (apiResponse!.IsSuccess == true)
                              {
                                    var apiResultObj = JsonConvert.DeserializeObject<LoginResponseDTO>(Convert.ToString(apiResponse.Result)!);
                                    var identity = new ClaimsIdentity(new Claim[]
                                    {
                                  new Claim(ClaimTypes.NameIdentifier, apiResultObj!.UserId.ToString()),
                                  new Claim(ClaimTypes.Name, apiResultObj.Name!),
                                  new Claim(ClaimTypes.Role, apiResultObj.Role!),
                                  //new Claim(ClaimTypes.Email, apiResultObj.Email!),
                                  new Claim("EncryptedId", apiResultObj.EncryptedId!),
                                  new Claim("Token", apiResultObj.Token!)
                                    }, "InspectorLoginScheme");
                                    var ClaimsPrincipal = new ClaimsPrincipal(identity);
                                    await HttpContext.SignInAsync("InspectorLoginScheme", ClaimsPrincipal, ClaimsPrincipalGenerate.authpropgenerate(model.IsPersistent));

                                    if (returnUrl == null)
                                    {
                                          return Redirect("/Inspector/Dashboard");
                                    }
                                    return Redirect(returnUrl);

                              }
                              else
                              {
                                    TempData["Error"] = apiResponse.Message;
                              }
                        }
                  }

                  return View(model);
            }




            [Route("/Accessed_Denied")]
            [HttpGet]
            public async Task<IActionResult> AccessDenied()
            {
                  ViewBag.Url = "";
                  string role = User.FindFirstValue(ClaimTypes.Role);
                  if (role == SD.Inspector || role == SD.AdminInspector)
                  {
                        ViewBag.Url = "/InspectorLogin";
                  }
                  else
                  {
                        ViewBag.Url = "/Login";
                  }
                  if (User.Identity!.IsAuthenticated)
                  {
                        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                        var users = await _user.GetFirstOrDefaultAsync(filter: x => x.Id == Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)));
                        users.IsLoggedIn = false;
                        users.LastSeenTime = DateTime.Now;
                        await _user.UpdateAsync(users);
                  }
                  return View();
            }

            [Route("/Not_Found")]
            [HttpGet]
            public async Task<IActionResult> Unfound()
            {
                  ViewBag.Url = "";
                  string role = User.FindFirstValue(ClaimTypes.Role);
                  if (role == SD.Inspector || role == SD.AdminInspector)
                  {
                        ViewBag.Url = "/InspectorLogin";
                  }
                  else
                  {
                        ViewBag.Url = "/Login";
                  }
                  if (User.Identity!.IsAuthenticated)
                  {
                        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                        var users = await _user.GetFirstOrDefaultAsync(filter: x => x.Id == Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)));
                        users.IsLoggedIn = false;
                        users.LastSeenTime = DateTime.Now;
                        await _user.UpdateAsync(users);
                  }
                  return View();
            }

            [Route("/ForgetPassword")]
            [HttpGet]
            public IActionResult ForgetPassword()
            {
                  return View();
            }

            [Route("/Inspector/ForgetPassword")]
            [HttpGet]
            public IActionResult InsForgetPassword()
            {
                  return View();
            }

            [HttpPost("/Inspector/ForgetPassword")]
            public async Task<IActionResult> InspectorForgetPassword(ForgetPasswordVM model)
            {
                  using (var httpClient = new HttpClient())
                  {
                        StringContent stringContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
                        using (var response = await httpClient.PostAsync(_configuration.GetSection("APIUrl").Value + "/ForgetPassword", stringContent))
                        {
                              var responseResult = await response.Content.ReadAsStringAsync();
                              var apiResponse = JsonConvert.DeserializeObject<APIResponse>(responseResult);
                              if (apiResponse!.IsSuccess == true)
                              {
                                    var baseUrl = _configuration.GetValue<string>("BaseUrl");
                                    var apiResultObj = JsonConvert.DeserializeObject<ForgetPasswordResponseDTO>(Convert.ToString(apiResponse.Result)!);
                                    string emailContent = new EmailFilePathGenerate(_webHostEnvironment).EmailFilePath(@"EmailTemplates\ForgotPasswordEmail.html");
                                    emailContent = emailContent.Replace("{{UserName}}", apiResultObj.Username);
                                    emailContent = emailContent.Replace("{{TempKey}}", apiResultObj.TempPassword);
                                    emailContent = emailContent.Replace("{{BaseUrl}}", /*_configuration.GetValue<string>("BaseUrl")*/ baseUrl + "/InspectorLogin");
                                    emailContent = emailContent.Replace("{{ServerUrl}}", _configuration.GetValue<string>("ServerUrl"));
                                    await _emailSenderService.SendEmail(apiResultObj.UserEmail!, null, "Account Retrieval", emailContent, Array.Empty<Byte[]>(), Array.Empty<String>());
                                    //TempData["Msg"] = "Your New Password is sent to your Registered Email.. Please check your Email to Proceed Further";
                                    //return View(model);
                                    return Json(new { success = apiResponse.IsSuccess, msg = apiResponse.Message }); 

                              }
                              else
                              {
                                    TempData["Error"] = apiResponse.Message;
                              }
                        }
                  }

                  return View(model);
            }

            [Route("/ForgetPassword")]
            [HttpPost]
            public async Task<IActionResult> ForgetPassword(ForgetPasswordVM model)
            {
                  if (ModelState.IsValid)
                  {
                        var User = await _user.GetFirstOrDefaultAsync(x => x.BHCD == model.BHCD);
                        if (User == null)
                        {
                              TempData["Err"] = "User Not Registered";
                              return View(model);
                        }

                        else
                        {
                              if (model.EmailId != User.EmailId)
                              {
                                    TempData["Err"] = "Entered Email Doesn't match the Registered Email Address";
                                    return View(model);
                              }
                              var tempPassword = _randomPassword.TemporaryPassword();
                              var tempPasswordDigest = _passwordHasher.HashPassword(tempPassword);

                              User.Password = tempPasswordDigest.Hash;
                              User.Salt = tempPasswordDigest.Salt;
                              User.UpdatedOn = DateTime.Now;
                              await _user.UpdateAsync(User);

                              var baseUrl = _configuration.GetValue<string>("BaseUrl");

                              string emailContent = new EmailFilePathGenerate(_webHostEnvironment).EmailFilePath(@"EmailTemplates\ForgotPasswordEmail.html");
                              emailContent = emailContent.Replace("{{UserName}}", User.FirstName + " " + User.LastName);
                              emailContent = emailContent.Replace("{{TempKey}}", tempPassword);
                              emailContent = emailContent.Replace("{{BaseUrl}}", /*_configuration.GetValue<string>("BaseUrl")*/ baseUrl + "/Login");
                              emailContent = emailContent.Replace("{{ServerUrl}}", _configuration.GetValue<string>("ServerUrl"));
                              await _emailSenderService.SendEmail(User.EmailId, null, "Account Retrieval", emailContent, Array.Empty<Byte[]>(), Array.Empty<String>());
                              TempData["Msg"] = "Your New Password is sent to your Registered Email.. Please check your Email to Proceed Further";
                              return View(model);
                        }
                  }
                  return View(model);
            }

        [HttpGet("/TestPdf")]
        public async Task<IActionResult> TestPdf()
        {
            var pdfBytes = await _generatePdf.GetByteArray("Views/PdfTemplates/TestPdf.cshtml");
            var pdfBase64 = Convert.ToBase64String(pdfBytes);
            return Json(new { pdfData = pdfBase64 });
        }

            [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
            public IActionResult Error()
            {
                  return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }

        /// Online Code Start ////
        //[Route("PaymentStatus")]
        //[HttpGet]
        //public async Task<IActionResult> PaymentStatus(string id, byte status)
        //{
        //      if (id != null)
        //      {
        //            var paymentId = _encrypt.Decrypt256(id);
        //            var payment = await _paymentRepo.GetFirstOrDefaultAsync(filter: x => x.Id == Convert.ToInt32(paymentId));
        //            var paymentDetails = await _paymentDetailsTable.GetAllAsync(filter: x => x.PaymentId == payment.Id);

        //            var fees = await _feesRepo.GetFirstOrDefaultAsync(filter: x => x.Id == payment.FeesId, includeProperties: "Establishment");
        //            var feesDetails = await _feesDetailsTable.GetAllAsync(filter: x => x.FeesId == fees.Id);

        //            if (payment == null)
        //            {
        //                  return NotFound();
        //            }
        //            else
        //            {
        //                  payment.RedirectUrlCallApiStatus = status;
        //                  payment.RedirectUrlCallOn = System.DateTime.Now;
        //                  await _paymentRepo.UpdateAsync(payment);

        //                  if (status == 2)
        //                  {
        //                        ViewBag.flg = 1;

        //                        const string REDIRECT_FORMAT = "https://stage.collectorsolutions.com:443/magic-api/api/transaction/redirect/{0}/{1}";

        //                        // your client web key
        //                        string clientKey = _configuration["JetPayStage:APIClientKey"];

        //                        // the transaction identifier to retrieve
        //                        string transactionId = payment.InvoiceNo;

        //                        //Setup the WebRequest with the API process method to be used
        //                        WebRequest request = WebRequest.Create(string.Format(REDIRECT_FORMAT, clientKey, transactionId));

        //                        //Set timeout, content type and http method type
        //                        request.Timeout = 120000;
        //                        request.Headers.Add("Accepts", "application/json");

        //                        //Create the string for the returning json string object
        //                        string jsonOutput = string.Empty;

        //                        //Setup the WebResponse object to accept the returning information
        //                        using (WebResponse response = request.GetResponse())
        //                        {
        //                              //Setup a StreamReader object to read the inbound information
        //                              StreamReader responseStream = new StreamReader(response.GetResponseStream());

        //                              // Read it to a string
        //                              jsonOutput = responseStream.ReadToEnd();
        //                        }

        //                        //Deserialize it to a response model
        //                        PaymentRedirectGetResponseModel redirectGetResponse = JsonConvert.DeserializeObject<PaymentRedirectGetResponseModel>(jsonOutput);

        //                        if (redirectGetResponse == null)
        //                        {
        //                              ViewBag.flg = 0;
        //                        }
        //                        else
        //                        {
        //                              payment.GetPaymentApiCallStatus = redirectGetResponse.status;
        //                              payment.GetPaymentApiCallOn = System.DateTime.Now;

        //                              if (redirectGetResponse.status.ToLower() == "error")
        //                              {
        //                                    payment.GetPaymentApiMessage = redirectGetResponse.errors[0].message.ToString();
        //                                    payment.PaymentStatus = 3;

        //                                    fees.Status = 3;
        //                              }
        //                              else
        //                              {
        //                                    payment.ApprovalStatus = Convert.ToByte(redirectGetResponse.approvalStatus);
        //                                    payment.PaymentReceiptConfirmation = redirectGetResponse.paymentReceiptConfirmation;
        //                                    payment.EffectiveDate = redirectGetResponse.effectiveDate;
        //                                    payment.PaymentMethod = Convert.ToByte(redirectGetResponse.paymentMethod);
        //                                    payment.PaymentType = 1;
        //                                    payment.CardType = Convert.ToByte(redirectGetResponse.cardType);
        //                                    payment.NameOnCard = redirectGetResponse.nameOnCard;
        //                                    payment.CardNumber = redirectGetResponse.cardNumber;
        //                                    payment.BankName = redirectGetResponse.bankName;
        //                                    payment.RoutingNumber = redirectGetResponse.routingNumber;
        //                                    payment.AccountNumber = redirectGetResponse.accountNumber;
        //                                    payment.CollectionMode = Convert.ToByte(redirectGetResponse.collectionMode);
        //                                    payment.HostTransactionId = redirectGetResponse.HostTransactionId;
        //                                    payment.HostAuthorizationCode = redirectGetResponse.HostAuthorizationCode;
        //                                    payment.VoidCredit = Convert.ToByte(redirectGetResponse.VoidCredit);
        //                                    payment.PaymentStatus = 2;
        //                                    payment.PaymentOn = DateTime.Now;
        //                                    fees.Status = 2;
        //                                    if (fees.Establishment!.PermitNumber!.StartsWith("TF"))
        //                                    {
        //                                          if (fees.Establishment!.PermitStatusId == 7)
        //                                          {
        //                                                fees.Establishment!.OldPermitStatusId = 7;
        //                                                fees.Establishment.PermitStatusId = 9;
        //                                          }
        //                                    }
        //                                    else
        //                                    {
        //                                          if (fees.Establishment!.PermitStatusId == 7)
        //                                          {
        //                                                fees.Establishment!.OldPermitStatusId = fees.Establishment.PermitStatusId;
        //                                                fees.Establishment.PermitStatusId += 1;
        //                                          }
        //                                    }
        //                                    await _establishment.UpdateAsync(fees.Establishment);
        //                              }
        //                        }
        //                  }
        //                  else
        //                  {
        //                        ViewBag.flg = 0;

        //                        payment.PaymentStatus = 3;
        //                        fees.Status = 3;
        //                  }

        //                  await _paymentRepo.UpdateAsync(payment);
        //                  await _feesRepo.UpdateAsync(fees);

        //                  foreach (var items in paymentDetails)
        //                  {
        //                        items.PaymentStatus = payment.PaymentStatus;
        //                        items.PaymentDate = payment.PaymentOn;
        //                        await _paymentDetailsTable.UpdateAsync(items);
        //                  }
        //                  foreach (var items in feesDetails)
        //                  {
        //                        items.Status = fees.Status;
        //                        items.UpdatedOn = DateTime.Now;
        //                        await _feesDetailsTable.UpdateAsync(items);
        //                  }

        //                  return View();
        //            }
        //      }
        //      return NotFound();
        //}
        /// Online Code END ////
    }
}
