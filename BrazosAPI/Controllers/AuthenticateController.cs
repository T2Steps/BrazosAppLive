using BrazosAPI.Extensions;
using BrazosAPI.Models;
using BrazosAPI.Models.DTOs;
using BrazosApp.DataAccess.Repository.IRepository;
using BrazosApp.Models;
using BrazosApp.Utility;
using BrazosApp.Utility.Helpers;
using BrazosApp.Utility.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace BrazosAPI.Controllers
{
      [Route("api/[controller]")]
      [ApiController]
      public class AuthenticateController : Controller
      {
            private readonly IPasswordHasher _passwordHasher;
            private readonly IEncrypt _encrypt;
            private readonly IConfiguration _configuration;
            private readonly IRepository<Users> _users;
            private readonly IPasswordGenerator _randomPassword;
            private readonly IEmailSenderService _emailSenderService;
            private readonly IWebHostEnvironment _webHostEnvironment;
            protected APIResponse _response;

            public AuthenticateController(IPasswordHasher passwordHasher, IRepository<Users> users, IEncrypt encrypt, IConfiguration configuration, IPasswordGenerator randomPassword, IEmailSenderService emailSenderService, IWebHostEnvironment webHostEnvironment)
            {
                  _passwordHasher = passwordHasher;
                  _response = new();
                  _users = users;
                  _encrypt = encrypt;
                  _configuration = configuration;
                  _randomPassword = randomPassword;
                  _emailSenderService = emailSenderService;
                  _webHostEnvironment = webHostEnvironment;
            }

            [HttpPost("/Login", Name = "Login")]
            public async Task<ActionResult<APIResponse>> Login([FromBody] LoginRequestDTO loginRequestDTO)
            {
                  if (loginRequestDTO == null)
                  {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSuccess = false;
                        _response.Message = string.Empty;
                        return BadRequest(_response);
                  }


                  var user = await _users.GetFirstOrDefaultAsync(
                          filter: x => x.BHCD == loginRequestDTO.BHCD, includeProperties: "Role");

                  //var user = new Users();

                  var loginResponse = new LoginResponseDTO()
                  {
                        Name = "",
                        Role = "",
                        Token = ""
                  };


                  if (user == null)
                  {
                        loginResponse.UserId = 0;
                        loginResponse.EncryptedId = "";
                        _response.StatusCode = HttpStatusCode.OK;
                        _response.IsSuccess = false;
                        _response.Message = "User not found!";
                        _response.Result = loginResponse;
                        return Ok(_response);
                  }

                  //var loginResponse = new LoginResponseDTO()
                  //{
                  //      UserId = user.Id,


                  //};

                  if (user.Role!.Name != SD.Inspector && user.Role.Name != SD.AdminInspector && user.Role.Name != SD.SuperAdmin)
                  {
                        loginResponse.UserId = 0;
                        loginResponse.EncryptedId = "";
                        _response.StatusCode = HttpStatusCode.OK;
                        _response.IsSuccess = false;
                        _response.Message = "User Not Allowed To Sign In! Try Signing In normally";
                        _response.Result = loginResponse;
                        return Ok(_response);
                  }

                  if (!_passwordHasher.VerifyPassword(loginRequestDTO.Password!, user.Salt!, user.Password!))
                  {
                        _response.StatusCode = HttpStatusCode.OK;
                        _response.IsSuccess = false;
                        _response.Message = "Password is incorrect!";
                        _response.Result = loginResponse;
                        return Ok(_response);
                  }


                  var encryptedId = _encrypt.Encrypt256((user.Id).ToString());
                  loginResponse.UserId = user.Id;
                  loginResponse.Name = $"{user.FirstName} {user.LastName}";
                  //loginResponse.Email = user.EmailId;
                  loginResponse.Role = user.Role!.Name;
                  loginResponse.EncryptedId = encryptedId;
                  loginResponse.Token = CreateJwtToken(user, encryptedId);
                  user.IsLoggedIn = true;
                  await _users.UpdateAsync(user);
                  //loginResponse.Token = "Bearer" + " " + CreateJwtToken(user, encryptedId);

                  _response.StatusCode = HttpStatusCode.OK;
                  _response.IsSuccess = true;
                  _response.Message = "Login success!";
                  _response.Result = loginResponse;
                  return Ok(_response);
            }

            private string CreateJwtToken(Users user, string encryptedId)
            {

                  var jwtTokenHandler = new JwtSecurityTokenHandler();
                  //var key = Encoding.ASCII.GetBytes("JFGBUImdjbcDFJHIFHSmalsdk");
                  var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JWTkey").Value));
                  var identity = new ClaimsIdentity(new Claim[]
                  {
                      new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                      new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName),
                      new Claim(ClaimTypes.Role, user.Role!.Name),
                      new Claim(ClaimTypes.Email, user.EmailId!),
                      new Claim("UserName", user.BHCD!),
                      new Claim("EncryptedId", encryptedId)
                  });

                  var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                  var tokenDescriptor = new SecurityTokenDescriptor
                  {
                        Subject = identity,
                        Expires = DateTime.Now.AddDays(1),
                        SigningCredentials = credentials
                  };
                  var token = jwtTokenHandler.CreateToken(tokenDescriptor);

                  return jwtTokenHandler.WriteToken(token);

                  //var tokenDescriptor = new JwtSecurityToken(
                  //          claims: identity,
                  //          expires
                  //          signingCredentials: credentials,
                  //    )TokenHandler;
            }

            [HttpPost("/ForgetPassword", Name = "ForgetPassword")]
            public async Task<ActionResult<APIResponse>> ForgetPassword([FromBody] ForgetPasswordRequestDTO model)
            {
                  if (model == null)
                  {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSuccess = false;
                        _response.Message = string.Empty;
                        return BadRequest(_response);
                  }
                  var User = await _users.GetFirstOrDefaultAsync(x => x.BHCD == model.BHCD);
                  if (User == null)
                  {
                        //TempData["Err"] = "User Not Registered";
                        //return View(model);
                        _response.StatusCode = HttpStatusCode.OK;
                        _response.IsSuccess = false;
                        _response.Message = "User Not Registered";
                        _response.Result = string.Empty;
                        return Json(new { Success = false, response = _response });
                  }

                  else
                  {
                        if (model.EmailId != User.EmailId)
                        {
                              //TempData["Err"] = "Entered Email Doesn't match the Registered Email Address";
                              //return View(model);
                              _response.StatusCode = HttpStatusCode.OK;
                              _response.IsSuccess = false;
                              _response.Message = "Entered Email Doesn't match the Registered Email Address";
                              _response.Result = string.Empty;
                              //return Ok(_response);
                              return Json(new { Success = false, response = _response });
                        }
                        var tempPassword = _randomPassword.TemporaryPassword();
                        var tempPasswordDigest = _passwordHasher.HashPassword(tempPassword);

                        User.Password = tempPasswordDigest.Hash;
                        User.Salt = tempPasswordDigest.Salt;
                        User.UpdatedOn = DateTime.Now;
                        await _users.UpdateAsync(User);

                        ForgetPasswordResponseDTO response = new ForgetPasswordResponseDTO();
                        response.Username = User.FirstName + " " + User.LastName;
                        response.TempPassword = tempPassword;
                        response.UserEmail = User.EmailId;

                        //string emailContent = "";
                        //try
                        //{
                        //      emailContent = new EmailFilePathGenerate(_webHostEnvironment).EmailFilePath(@"EmailTemplates\ForgotPasswordEmail.html");
                        //}
                        //catch(Exception ex)
                        //{

                        //}

                        //emailContent = emailContent.Replace("{{UserName}}", User.FirstName + " " + User.LastName);
                        //emailContent = emailContent.Replace("{{TempKey}}", tempPassword);
                        //emailContent = emailContent.Replace("{{BaseUrl}}", _configuration.GetValue<string>("BaseUrl"));
                        //emailContent = emailContent.Replace("{{ServerUrl}}", _configuration.GetValue<string>("ServerUrl"));
                        //await _emailSenderService.SendEmail(User.EmailId!, null, "Account Retrieval", emailContent, null, null);
                        //TempData["Msg"] = "Your New Password is sent to your Registered Email.. Please check your Email to Proceed Further";
                        //return View(model);
                        _response.StatusCode = HttpStatusCode.OK;
                        _response.IsSuccess = true;
                        _response.Message = "Your New Password is sent to your Registered Email.. Please check your Email to Proceed Further";
                        //_response.Result = string.Empty;
                        _response.Result = response;
                        return Ok(_response);
                        //return Json(new { Success = true, response = _response });
                  }
            }
      }
}
