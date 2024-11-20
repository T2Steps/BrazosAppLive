using BrazosAPI.Extensions;
using BrazosAPI.Models;
using BrazosAPI.Models.DTOs;
using BrazosApp.DataAccess.Repository.IRepository;
using BrazosApp.Models;
using BrazosApp.Utility.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Net;
using System.Reflection;
using System.Security.Claims;
using System.Text;

namespace BrazosAPI.Controllers
{
      [Route("api/[controller]")]
      [ApiController]
      [Authorize]
      public class AccountController : Controller
      {
            private readonly IRepository<Users> _users;
            private readonly IWebHostEnvironment _environment;
            private readonly IConfiguration _configuration;
            private readonly IPasswordHasher _passwordHasher;
            private readonly IEncrypt _encrypt;
            protected APIResponse _response;
            private readonly ILogger<AccountController> _logger;
            //private readonly ILogger _logger;
            private readonly string Container = @"\Images\UsersSignature\";

            public AccountController(IRepository<Users> users, IPasswordHasher passwordHasher, IEncrypt encrypt, IWebHostEnvironment environment, IConfiguration configuration, ILogger<AccountController> logger)
            {
                  _users = users;
                  _passwordHasher = passwordHasher;
                  _encrypt = encrypt;
                  _response = new();
                  _environment = environment;
                  _configuration = configuration;
                  _logger = logger;
            }

            [HttpPost("/Inspector/Reset_Password", Name = "ResetPassword")]
            public async Task<ActionResult<APIResponse>> ResetPassword([FromForm] ResetPasswordRequestDTO model)
            {
                  if (model == null)
                  {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSuccess = false;
                        _response.Message = string.Empty;
                        return BadRequest(_response);
                  }
                  var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                  var str = TokenValidator.Validation(token);
                  if (str == "Authorized")
                  {
                        var userId = TokenValidator.GetUserId(token);
                        var user = await _users.GetFirstOrDefaultAsync(filter: x => x.Id == Convert.ToInt32(_encrypt.Decrypt256(userId)));
                        if (user == null)
                        {
                              _response.StatusCode = HttpStatusCode.OK;
                              _response.IsSuccess = false;
                              _response.Message = "User not found!";
                              _response.Result = "";
                              return Json(new { Success = false, response = _response });
                        }
                        bool passwordmatch = _passwordHasher.VerifyPassword(model.CurrentPassword!, user.Salt!, user.Password!);
                        if (passwordmatch)
                        {
                              if (model.NewPassword == model.ConfirmPassword)
                              {
                                    var newPassword = _passwordHasher.HashPassword(model.NewPassword!);
                                    user.Password = newPassword.Hash;
                                    user.Salt = newPassword.Salt;
                                    await _users.UpdateAsync(user);
                                    _response.StatusCode = HttpStatusCode.OK;
                                    _response.IsSuccess = true;
                                    _response.Message = "Password Successfully Changed";
                                    _response.Result = "";
                                    return Json(new { Success = true, response = _response });
                              }
                              _response.StatusCode = HttpStatusCode.OK;
                              _response.IsSuccess = false;
                              _response.Message = "Failed!! The new password you entered does not match the confirmation password";
                              _response.Result = "";
                              return Json(new { Success = false, response = _response });
                        }
                        _response.StatusCode = HttpStatusCode.OK;
                        _response.IsSuccess = false;
                        _response.Message = "Failed!! The current password you entered is incorrect";
                        _response.Result = "";
                        return Json(new { Success = false, response = _response });
                  }

                  _response.StatusCode = HttpStatusCode.Unauthorized;
                  _response.IsSuccess = false;
                  _response.Message = "Access Denied!";
                  _response.Result = "";
                  return Json(new { Success = false, response = _response });
            }


            //[HttpPost("/Inspector/SignatureUpload", Name = "SignatureUpload")]
            //public async Task<ActionResult<APIResponse>> SignatureUpload(IFormFile? SignFile)
            //{
            //      if (SignFile == null)
            //      {
            //            _response.StatusCode = HttpStatusCode.BadRequest;
            //            _response.IsSuccess = false;
            //            _response.Message = string.Empty;
            //            return BadRequest(_response);
            //      }

            //      var base64 = "";

            //      using (var memoryStream = new MemoryStream())
            //      {
            //            await SignFile.CopyToAsync(memoryStream);
            //            var fileBytes = memoryStream.ToArray();
            //            base64 = Convert.ToBase64String(fileBytes);
            //      }

            //      var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            //      var str = TokenValidator.Validation(token);
            //      if (str == "Authorized")
            //      {
            //            var userId = TokenValidator.GetUserId(token);
            //            var user = await _users.GetFirstOrDefaultAsync(filter: x => x.Id == Convert.ToInt32(_encrypt.Decrypt256(userId)));
            //            if (user == null)
            //            {
            //                  _response.StatusCode = HttpStatusCode.OK;
            //                  _response.IsSuccess = false;
            //                  _response.Message = "User not found!";
            //                  _response.Result = "";
            //                  return Json(new { Success = false, response = _response });
            //            }
            //            using (var client = new HttpClient())
            //            {
            //                  //var form = new MultipartFormDataContent
            //                  //{
            //                  //      { new StreamContent(SignFile.OpenReadStream()), "file", SignFile.FileName },
            //                  //      { new StringContent(_encrypt.Encrypt256(user.Id.ToString())), "userId" }
            //                  //};

            //                  var request = new SignatureFileRequestDTO
            //                  {
            //                        base64String = base64,
            //                        userId = _encrypt.Encrypt256(user.Id.ToString())
            //                  };

            //                  //StringContent stringContent = new StringContent(JsonConvert.SerializeObject(base64), Encoding.UTF8, "application/json");
            //                  //var response = await client.PostAsync("https://localhost:44357/saveSignatureFile", request);

            //                  var jsonContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            //                  var response = await client.PostAsync("https://localhost:44357/saveSignatureFile", jsonContent);

            //                  if (response.IsSuccessStatusCode)
            //                  {
            //                        var result = await response.Content.ReadAsStringAsync();
            //                        _logger.LogInformation("Response Content: {ResponseContent}", result);
            //                        user.SignFileName = user.Id + "-" + "sign" + Path.GetExtension(SignFile.FileName);
            //                        user.UpdatedBy = Convert.ToInt32(user.Id);
            //                        user.UpdatedOn = DateTime.Now;
            //                        await _users.UpdateAsync(user);

            //                        _response.StatusCode = HttpStatusCode.OK;
            //                        _response.IsSuccess = true;
            //                        _response.Message = "Signature Uploaded Successfully";
            //                        _response.Result = result;
            //                        return Json(new { success = true, response = _response });
            //                  }
            //                  else
            //                  {
            //                        _response.StatusCode = HttpStatusCode.InternalServerError;
            //                        _response.IsSuccess = false;
            //                        _response.Message = "Failed to upload signature";
            //                        _response.Result = "";
            //                        return Json(new { success = false, response = _response });
            //                  }
            //            }
            //            //var uploadPath = _environment.EnvironmentName + Container + user.Id;
            //            //var uploadPath = _configuration.GetSection("FilePath").Value + Container + user.Id;
            //            //var uploadPath = "C:\\Niladri\\Black Swan\\Office\\Projects\\Brazos\\Working\\BrazosApp\\BrazosApp\\wwwroot" + Container + user.Id;

            //            //var uploadPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + Container + user.Id;

            //            //if (!Directory.Exists(uploadPath))
            //            //{
            //            //    Directory.CreateDirectory(uploadPath);
            //            //}
            //            //else
            //            //{
            //            //    foreach (string files in Directory.GetFiles(uploadPath))
            //            //    {
            //            //        try
            //            //        {
            //            //            System.IO.File.Delete(files);
            //            //        }
            //            //        catch (Exception ex) { }

            //            //    }
            //            //}
            //            //var filename = user.Id + "-" + "sign" + Path.GetExtension(SignFile.FileName);
            //            //var filePath = Path.Combine(uploadPath, filename);
            //            //if (System.IO.File.Exists(filePath))
            //            //{
            //            //    System.IO.File.Delete(filePath);
            //            //}
            //            //var fs = new FileStream(filePath, FileMode.Create);
            //            //SignFile.CopyTo(fs);
            //            //fs.Close();
            //            //user.SignFileName = filename;
            //            //user.UpdatedBy = Convert.ToInt32(user.Id);
            //            //user.UpdatedOn = DateTime.Now;
            //            //await _users.UpdateAsync(user);

            //            //_response.StatusCode = HttpStatusCode.OK;
            //            //_response.IsSuccess = true;
            //            //_response.Message = "Signature Uploaded Successfully";
            //            //_response.Result = "/Images/UsersSignature/" + user.Id + "/" + user.SignFileName;
            //            ////return Json(new { success = true, msg = "Signature Uploaded Successfully", src = "/Images/UsersSignature/" + user.Id + "/" + user.SignFileName });
            //            //return Json(new { success = true, response = _response });
            //      }

            //      _response.StatusCode = HttpStatusCode.Unauthorized;
            //      _response.IsSuccess = false;
            //      _response.Message = "Access Denied!";
            //      _response.Result = "";
            //      return Json(new { Success = false, response = _response });

            //}
      }
}
