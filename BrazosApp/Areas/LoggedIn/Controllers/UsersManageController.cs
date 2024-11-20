using BrazosApp.DataAccess.Repository.IRepository;
using BrazosApp.Models;
using BrazosApp.Models.ViewModels;
using BrazosApp.Utility;
using BrazosApp.Utility.Helpers;
using BrazosApp.Utility.Services;
using BrazosApp.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Security.Claims;

namespace BrazosApp.Areas.LoggedIn.Controllers
{
      [Area("LoggedIn")]
      [Authorize("UserManagePolicy")]
      public class UsersManageController : Controller
      {
            private readonly IRepository<Users> _users;
            private readonly IRepository<Role> _roles;
            private readonly IPasswordHasher _passwordEncrypt;
            private readonly IPasswordGenerator _random;
            private readonly IWebHostEnvironment _environment;
            private readonly IEmailSenderService _emailSender;
            private readonly IConfiguration _configuration;
            private readonly IEncrypt _encrypt;
            private readonly string Container = @"\Images\UsersSignature\";

            public UsersManageController(IRepository<Users> users,
                IRepository<Role> roles,
                IPasswordHasher passwordEncrypt,
                IPasswordGenerator random,
                IWebHostEnvironment environment,
                IEmailSenderService emailSender,
                IEncrypt encrypt,
                IConfiguration configuration)
            {
                  _users = users;
                  _roles = roles;
                  _passwordEncrypt = passwordEncrypt;
                  _random = random;
                  _environment = environment;
                  _emailSender = emailSender;
                  _encrypt = encrypt;
                  _configuration = configuration;
            }

            [Route("/Users")]
            [HttpGet]
            public async Task<IActionResult> Index()
            {
                  var roles = await _roles.GetAllAsync();
                  UserRegistrationVM model = new UserRegistrationVM();
                  model.user = new Users();
                  if (User.FindFirstValue(ClaimTypes.Role) == SD.SuperAdmin)
                  {
                        model.RoleList = roles.Where(x => x.IsActive /*&& x.Name != SD.SuperAdmin*/).Select(x => new SelectListItem
                        {
                              Text = x.Name,
                              Value = x.Id.ToString(),
                        }).ToList();
                  }
                  else if (User.FindFirstValue(ClaimTypes.Role) == SD.AdminInspector || User.FindFirstValue(ClaimTypes.Role) == SD.Admin)
                  {
                        model.RoleList = roles.Where(x => x.IsActive && x.Name != SD.SuperAdmin && x.Name != SD.Admin && x.Name != SD.AdminInspector).Select(x => new SelectListItem
                        {
                              Text = x.Name,
                              Value = x.Id.ToString(),
                        }).ToList();
                  }
                  return View(model);
            }

            [Route("/GetAllUsers")]
            [HttpGet]
            public async Task<IActionResult> GetAllUsers()
            {
                  IEnumerable<Users> userList;
                  userList = await _users.GetAllAsync(filter: x => x.IsDelete == false /*&& x.Id != Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier))*/ /*&& x.Role!.Name!=SD.SuperAdmin*/, includeProperties: "Role");
                  if (User.FindFirstValue(ClaimTypes.Role) == SD.Admin || User.FindFirstValue(ClaimTypes.Role) == SD.AdminInspector)
                  {
                        userList = userList.Where(x => x.RoleId != 1 && x.RoleId != 2 && x.RoleId != 3);
                  }
                  foreach(var user in userList)
                  {
                        user.EncryptedId = _encrypt.Encrypt256(Convert.ToString(user.Id));
                  }
                  return Json(new { data = userList.ToList() });
            }

            [Route("/ChangeUserState/{id?}")]
            [HttpPost]
            public async Task<IActionResult> ActiveInactive(string id)
            {
                  string msg = "";
                  var uId = _encrypt.Decrypt256(id);
                  var user = await _users.GetById(Convert.ToInt32(uId));
                  if (user.IsActive == true)
                  {
                        user.IsActive = false;
                        msg = "Inactivated";
                  }
                  else
                  {
                        user.IsActive = true;
                        msg = "Activated";
                  }
                  user.SyncDate = DateTime.Now;
                  await _users.UpdateAsync(user);
                  return Json(new { success = true, msg = msg });
            }


            [Route("/GetUser/{id?}")]
            [HttpGet]
            public async Task<IActionResult> GetUser(string id)
            {
                  var uId = _encrypt.Decrypt256(id);
                  var User = await _users.GetById(Convert.ToInt32(uId));
                  return Json(new { success = true, user = User });
            }

            [Route("/UserUpsert")]
            [HttpPost]
            public async Task<IActionResult> Upsert(UserRegistrationVM model)
            {
                  var msg = "";
                  var info = "";
                  if (ModelState.IsValid)
                  {
                        var flg = 0;
                        var password = "";
                        //var UpsertedUser = new Users();
                        var user = new Users();
                        if (model.user != null)
                        {
                              if (model.user.Id == 0)
                              {
                                    var Existinguser = await _users.GetFirstOrDefaultAsync(filter: x => x.BHCD == model.user.BHCD && x.IsDelete != true);
                                    if (Existinguser != null)
                                    {
                                          return Json(new { success = false, msg = "User Already Exist" });
                                    }
                                    model.user.IsActive = true;
                                    model.user.CreatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                                    model.user.IsLoggedIn = false;
                                    password = _random.TemporaryPassword();
                                    var encryptedPassword = _passwordEncrypt.HashPassword(password);
                                    model.user.Password = encryptedPassword.Hash;
                                    model.user.Salt = encryptedPassword.Salt;
                                    model.user.SyncDate = DateTime.Now;
                                    await _users.AddAsync(model.user);
                                    msg = "User Successfully Created";
                                    info = "Credentials sent To User via Email";
                                    flg = 1;
                                    var url = "";
                                    if (model.user.RoleId==2|| model.user.RoleId == 4)
                                    {
                                          url = _configuration.GetValue<string>("BaseUrl") + "/InspectorLogin";
                                    }
                                    else
                                    {
                                          url = _configuration.GetValue<string>("BaseUrl") + "/Login";
                                    }
                                     
                                    string emailContent = new EmailFilePathGenerate(_environment).EmailFilePath(@"EmailTemplates\UserCreatedAlertEmail.html");
                                    emailContent = emailContent.Replace("{{UserName}}", model.user.FirstName + " " + model.user.LastName);
                                    emailContent = emailContent.Replace("{{TempKey}}", password);
                                    emailContent = emailContent.Replace("{{BaseUrl}}", /*_configuration.GetValue<string>("BaseUrl")*/ url);
                                    emailContent = emailContent.Replace("{{ServerUrl}}", _configuration.GetValue<string>("ServerUrl"));
                                    try
                                    {
                                          await _emailSender.SendEmail(model.user.EmailId!, null, "User Registration", emailContent, Array.Empty<Byte[]>(), Array.Empty<String>());
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                              }
                              else
                              {
                                    user = await _users.GetFirstOrDefaultAsync(filter: x => x.Id == model.user.Id);
                                    user.FirstName = model.user.FirstName;
                                    user.LastName = model.user.LastName;
                                    user.BHCD = model.user.BHCD;
                                    user.EmailId = model.user.EmailId;
                                    user.DesignatedRepresentative = model.user.DesignatedRepresentative;
                                    user.RegisteredSanitarian = model.user.RegisteredSanitarian;
                                    user.SanitarianInTrain = model.user.SanitarianInTrain;
                                    user.CertifiedPoolOperator = model.user.CertifiedPoolOperator;
                                    user.CertifiedPoolInspector = model.user.CertifiedPoolInspector;
                                    user.RoleId = model.user.RoleId;
                                    user.UpdatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                                    user.UpdatedOn = DateTime.Now;
                                    user.SyncDate = DateTime.Now;
                                    await _users.UpdateAsync(user);
                                    msg = "User Successfully Updated";
                                    flg = 2;
                              }
                              if (model.user.SignFile != null)
                              {
                                    if (flg == 1)
                                    {
                                          user = model.user;
                                    }
                                    
                                    var uploadPath = _environment.WebRootPath + Container + user.Id;
                                    if (!Directory.Exists(uploadPath))
                                    {
                                          Directory.CreateDirectory(uploadPath);
                                    }
                                    else
                                    {
                                          foreach (string files in Directory.GetFiles(uploadPath))
                                          {
                                                try
                                                {
                                                      System.IO.File.Delete(files);
                                                }
                                                catch (Exception ex) { }

                                          }
                                    }
                                    var filename = user.Id + "-" + "sign" + Path.GetExtension(model.user.SignFile.FileName);
                                    var filePath = Path.Combine(uploadPath, filename);
                                    if (System.IO.File.Exists(filePath))
                                    {
                                          System.IO.File.Delete(filePath);
                                    }
                                    var fs = new FileStream(filePath, FileMode.Create);
                                    model.user.SignFile.CopyTo(fs);
                                    fs.Close();
                                    user.SignFileName = filename;
                                    await _users.UpdateAsync(user);  
                              }
                              return Json(new { success = true, msg = msg, info= info });
                        }
                        return Json(new { success = false, msg = "Unexpected Error Occured" });
                  }
                  return Json(new { success = false, msg = "Unexpected Error Occured" });
            }
      }
}
