using BrazosApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BrazosApp.Extensions
{
    public static class ClaimsPrincipalGenerate
    {
        public static ClaimsPrincipal Generate(Users user, Role roles, string encryptedId)
        {
            //var claims = new List<Claim>()
            //{
            //    new Claim("Username", user.BHCD),
            //    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            //    new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName),
            //    new Claim(ClaimTypes.Role, roles.Name.ToString()),
            //    new Claim("EncryptedUiD", encryptedId)
            //    //new Claim("SignFile", user.SignFileName=null??"")
            //};


            //authProp.ExpiresUtc = 
            //var ClaimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var ClaimsIdentity = GenerateClaimIdentity(user, encryptedId);
            var ClaimsPrincipal = new ClaimsPrincipal(ClaimsIdentity);
            return ClaimsPrincipal;
        }

        public static AuthenticationProperties authpropgenerate(bool ispersistent)
        {
            var authProp = new AuthenticationProperties
            {
                IsPersistent = ispersistent,
                ExpiresUtc = DateTime.Now.AddMinutes(120)
            };
            
            return authProp;
        }


        public static ClaimsIdentity GenerateClaimIdentity(Users user, string encryptedId)
        {
            var AuthScheme = CookieAuthenticationDefaults.AuthenticationScheme;            
            var identity = new ClaimsIdentity(new Claim[]
            {
                      new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                      new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName),
                      new Claim(ClaimTypes.Role, user.Role!.Name),
                      new Claim(ClaimTypes.Email, user.EmailId!),
                      new Claim("UserName", user.BHCD!),
                      new Claim("EncryptedId", encryptedId)
            }, AuthScheme);

            return identity;
        }

        //public static string CreateJwtToken(Users user, string encryptedId, string JwtSecret)
        //{

        //    var jwtTokenHandler = new JwtSecurityTokenHandler();
        //    //var key = Encoding.ASCII.GetBytes("JFGBUImdjbcDFJHIFHSmalsdk");
        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSecret));
        //    //var identity = new ClaimsIdentity(new Claim[]
        //    //{
        //    //          new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        //    //          new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName),
        //    //          new Claim(ClaimTypes.Role, user.Role!.Name),
        //    //          new Claim(ClaimTypes.Email, user.EmailId!),
        //    //          new Claim("UserName", user.BHCD!),
        //    //          new Claim("EncryptedId", encryptedId)
        //    //});

        //    var identity = GenerateClaimIdentity(user, encryptedId, "v2");

        //    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //    var tokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        Subject = identity,
        //        Expires = DateTime.Now.AddDays(1),
        //        SigningCredentials = credentials
        //    };
        //    var token = jwtTokenHandler.CreateToken(tokenDescriptor);

        //    //identity = new ClaimsIdentity(new Claim[]
        //    //{
        //    //          new Claim("Token", token.ToString()!)
        //    //}, JwtBearerDefaults.AuthenticationScheme);

        //    //var ClaimPrincipal = new ClaimsPrincipal(identity);

        //    //return ClaimPrincipal;
        //    return jwtTokenHandler.WriteToken(token);

        //    //var tokenDescriptor = new JwtSecurityToken(
        //    //          claims: identity,
        //    //          expires
        //    //          signingCredentials: credentials,
        //    //    )TokenHandler;
        //}
    }
}
