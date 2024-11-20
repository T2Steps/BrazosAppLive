using BrazosApp.Models;
using BrazosApp.Utility;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BrazosAPI.Extensions
{
      public static class TokenValidator
      {
            public static string Validation(string token)
            {
                  var jwtTokenHandler = new JwtSecurityTokenHandler();
                  var tokenValid = jwtTokenHandler.CanReadToken(token);

                  if (!tokenValid)
                  {
                        return "Unauthorized";
                  }

                  var jwtToken = jwtTokenHandler.ReadJwtToken(token);

                  var userClaims = jwtToken.Claims;
                  var roles = userClaims.Where(c => c.Type == "role").Select(c => c.Value).ToList();

                  if (roles.Contains(SD.Inspector) || roles.Contains(SD.AdminInspector) || roles.Contains(SD.SuperAdmin))
                  {
                        return "Authorized";
                  }

                  return "Invalid";
            }

            public static string GetUserId(string token)
            {
                if (token == null) 
                { 
                    return "";
                }
                var jwtTokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = jwtTokenHandler.ReadJwtToken(token);

                var userClaims = jwtToken.Claims;
                var userId = userClaims.Where(c => c.Type == "EncryptedId").Select(c => c.Value).ToList();
                return userId[0];
            }

            public static string GetUserRole(string token)
            {
                if (token == null)
                {
                    return "";
                }
                var jwtTokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = jwtTokenHandler.ReadJwtToken(token);

                var userClaims = jwtToken.Claims;
                var role = userClaims.Where(c => c.Type == "role").Select(c => c.Value).ToList();
                return role[0];
            }
      }
}
