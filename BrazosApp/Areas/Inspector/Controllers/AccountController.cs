using BrazosApp.DataAccess.Repository.IRepository;
using BrazosApp.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using BrazosApp.Models.ViewModels;

namespace BrazosApp.Areas.Inspector.Controllers
{
    [Area("Inspector")]
    [Authorize(AuthenticationSchemes = "InspectorLoginScheme", Policy = "InspectorPolicy")]
    public class AccountController : Controller
    {
        private readonly IRepository<Users> _users;
        public AccountController(IRepository<Users> users)
        {
            _users = users;
        }

      [Route("/Inspector/Account/Profile")]
      [HttpGet]
      public async Task<IActionResult> Profile()
      {
            ProfileVM model = new ProfileVM();
            model.User = await _users.GetFirstOrDefaultAsync(filter: x => x.Id == Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)), includeProperties: "Role");
            model.ResetPassword = new ResetPasswordVM();
            return View(model);
      }

        [Route("/Inspector/Account/Logout")]
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

    }
}
