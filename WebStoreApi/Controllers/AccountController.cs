using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebStoreApi.Models;

namespace WebStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private string CreateJwtToken (User user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim("id",""+user.Id) ,
                new Claim("role", user.Role) 
            };
            string strkey = _configuration["JwtSettings:key"]!;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(strkey));
            var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"]!,
                audience: _configuration["JwtSettings:Audience"]!,
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials : creds
                );

            var JWt = new JwtSecurityTokenHandler().WriteToken(token);
            return JWt;

        }
    }
}
