using BrazosApp.DataAccess.Repository.IRepository;
using BrazosApp.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BrazosApp.Models.ViewModels;
using BrazosApp.Utility.Helpers;

namespace BrazosApp.Areas.LoggedIn.Controllers
{
    [Area("LoggedIn")]
    [Authorize("CommonPolicy")]
    public class AccountController : Controller
    {
        private readonly IRepository<Users> _users;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IWebHostEnvironment _environment;
        private readonly IEncrypt _encrypt;
        private readonly string Container = @"\Images\UsersSignature\";
        public AccountController(IRepository<Users> users, IPasswordHasher passwordHasher, IWebHostEnvironment environment, IEncrypt encrypt)
        {
            _users = users;
            _passwordHasher = passwordHasher;
            _environment = environment;
            _encrypt = encrypt;
        }

        [Route("/Account/Logout")]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var users = await _users.GetFirstOrDefaultAsync(filter: x => x.Id == Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            users.IsLoggedIn = false;
            users.LastSeenTime = DateTime.Now;
            await _users.UpdateAsync(users);
            return Redirect("/");
        }

        [Route("/Profile")]
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            ProfileVM model = new ProfileVM();
            model.User = await _users.GetFirstOrDefaultAsync(filter: x => x.Id == Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)), includeProperties: "Role");
            model.ResetPassword = new ResetPasswordVM();
            return View(model);
        }

        [Route("/Reset_Password")]
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM model)
        {
            if (ModelState.IsValid)
            {
                var user = await _users.GetFirstOrDefaultAsync(filter: x => x.Id == Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)));
                if (user == null) { return NotFound(); }
                bool passwordmatch = _passwordHasher.VerifyPassword(model.CurrentPassword, user.Salt, user.Password);
                if (passwordmatch)
                {
                    if (model.NewPassword == model.ConfirmPassword)
                    {
                        var newPassword = _passwordHasher.HashPassword(model.NewPassword);
                        user.Password = newPassword.Hash;
                        user.Salt = newPassword.Salt;
                        await _users.UpdateAsync(user);
                        return Json(new { success = true, msg = "Password Successfully Changed" });
                    }
                    return Json(new { success = false, msg = "Failed!! The new password you entered does not match the confirmation password" });
                }

                return Json(new { success = false, msg = "Failed!! The current password you entered is incorrect" });
            }

            return Json(new { success = false });
        }

        [Route("/SignatureUpload")]
        [HttpPost]
        public async Task<IActionResult> SignatureUpload(IFormFile? SignFile)
        {
            if (SignFile != null)
            {
                var user = await _users.GetById(Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)));
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
                            //var FStream = new FileStream(files, FileMode.Open);
                            //FStream.Close();
                            System.IO.File.Delete(files);
                        }
                        catch (Exception ex) { }

                    }

                    //Directory.CreateDirectory(uploadPath);
                }
                var filename = user.Id + "-" + "sign" + Path.GetExtension(SignFile.FileName);
                var filePath = Path.Combine(uploadPath, filename);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                var fs = new FileStream(filePath, FileMode.Create);
                //model.DocFile.CopyTo(fs);
                //fs.Close();
                SignFile.CopyTo(fs);
                fs.Close();

                user.SignFileName = filename;
                user.UpdatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                user.UpdatedOn = DateTime.Now;
                await _users.UpdateAsync(user);
                return Json(new { success = true, msg = "Signature Uploaded Successfully", src = "/Images/UsersSignature/" + user.Id + "/" + user.SignFileName });
            }

            return Json(new { success = false, msg = "Failed To Upload Signature" });
        }

        //[Route("/saveSignatureFile")]
        //[HttpPost]
        //public async Task<IActionResult> saveSignatureFile(/*[FromForm] IFormFile file, [FromForm] string userId*/ [FromBody] SignatureFileRequestDTO file)
        //{
        //    //if (file == null || file.Length == 0)
        //    //{
        //    //      return BadRequest("No file uploaded.");
        //    //}
        //    //var uId = _encrypt.Decrypt256(userId);
        //    //var uploadPath = Path.Combine(_environment.WebRootPath, "Images", "UsersSignature", uId);

        //    //if (!Directory.Exists(uploadPath))
        //    //{
        //    //      Directory.CreateDirectory(uploadPath);
        //    //}
        //    //else
        //    //{
        //    //      foreach (string files in Directory.GetFiles(uploadPath))
        //    //      {
        //    //            try
        //    //            {
        //    //                  //var FStream = new FileStream(files, FileMode.Open);
        //    //                  //FStream.Close();
        //    //                  System.IO.File.Delete(files);
        //    //            }
        //    //            catch (Exception ex) { }

        //    //      }
        //    //}
        //    //var fileName = uId + "-" + "sign" + Path.GetExtension(file.FileName);
        //    //var filePath = Path.Combine(uploadPath, fileName);

        //    //if (System.IO.File.Exists(filePath))
        //    //{
        //    //      System.IO.File.Delete(filePath);
        //    //}
        //    //using (var stream = new FileStream(filePath, FileMode.Create))
        //    //{
        //    //      await file.CopyToAsync(stream);
        //    //}

        //    //return Ok(new { filePath });
        //    return Ok();
        //}

    }
}
