using BrazosApp.DataAccess.Repository.IRepository;
using BrazosApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BrazosApp.Areas.LoggedIn.Controllers
{
    [Area("LoggedIn")]
      [Authorize("CommonPolicy")]
      public class DashboardController : Controller
      {
            private readonly IRepository<Users> _users;
            private readonly IRepository<Role>  _roles;

            public DashboardController(IRepository<Users> users,
                IRepository<Role> roles)
            {
                  _users = users;
                  _roles = roles;
            }

            [Route("/Dashboard")]
            public IActionResult Index()
            {
                  return View();
            }

            [Authorize("UserManagePolicy")]
            [Route("/GetAllRegisteredUsers")]
            [HttpGet]
            public async Task<IActionResult> GetAllRegisteredUsers()
            {
                  var users = await _users.GetAllAsync(filter:x=> x.IsActive == true && x.IsDelete==false /*&& x.Role!.Name!=SD.SuperAdmin*/, includeProperties:"Role");
                  return Json(new {success= true, count = users.Count() });
            }
      }
}
