using BrazosAPI.Extensions;
using BrazosAPI.Models;
using BrazosApp.DataAccess.Repository.IRepository;
using BrazosApp.Models;
using BrazosApp.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;

namespace BrazosAPI.Controllers
{
      [Route("api/[controller]")]
      [ApiController]
      [Authorize]
      public class TestController : Controller
      {
            private readonly IRepository<Users> _users;
            protected APIResponse _response;

            public TestController(IRepository<Users> users)
            {
                  _users = users;
                  _response = new();
            }

            //public IActionResult Index()
            //{
            //    return View();
            //}

            [HttpGet("/GetApiUsers", Name = "GetApiUsers")]
            public async Task<ActionResult<APIResponse>> GetApiUsers()
            {
                  var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                  //var jwtTokenHandler = new JwtSecurityTokenHandler();
                  //var tokenValid = jwtTokenHandler.CanReadToken(token);

                  //if (!tokenValid)
                  //{
                  //      return Unauthorized();
                  //}

                  //var jwtToken = jwtTokenHandler.ReadJwtToken(token);

                  //var userClaims = jwtToken.Claims;
                  ////var roles = userClaims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
                  //var roles = userClaims.Where(c => c.Type == "role").Select(c => c.Value).ToList();

                  //// Log the roles to verify what roles are present in the token
                  //Console.WriteLine($"User Roles: {string.Join(", ", roles)}");

                  var str = TokenValidator.Validation(token);

                  //if (roles.Contains(SD.Inspector) || roles.Contains(SD.AdminInspector))
                  //{
                  //      var users = await _users.GetAllAsync();
                  //      return Json(new { Success = true, users = users });
                  //}

                  if(str == "Authorized")
                  {
                        var users = await _users.GetAllAsync();
                        _response.StatusCode = HttpStatusCode.OK;
                        _response.IsSuccess = true;
                        _response.Message = "Success";
                        _response.Result = users;
                        return Json(new { Success = true, users = users });
                  }

                  //else if(str == "Unauthorized")
                  //{
                  //      _response.StatusCode = HttpStatusCode.Unauthorized;
                  //      _response.IsSuccess = false;
                  //      _response.Message = "Access Denied!";
                  //      _response.Result = "";
                  //      //return Ok(_response);
                  //      return Json(new { Success = false, response = _response });
                  //}

                  _response.StatusCode = HttpStatusCode.Unauthorized;
                  _response.IsSuccess = false;
                  _response.Message = "Access Denied!";
                  _response.Result = "";
                  //return Ok(_response);
                  return Json(new { Success = false, response = _response });

                  //return Unauthorized();
                  //return Json(new { Success = true });
            }

            [HttpPost("/TestPost", Name = "PostTest")]
            public async Task<ActionResult<APIResponse>> TestPost([FromForm] int id, [FromForm] string str)
            {
                  var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                  var tok = TokenValidator.Validation(token);
                  return Json(new { success = true });
            }
      }
}
