using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication_core_api_test.Service;

namespace WebApplication_core_api_test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpPost("GetToken")]
        public IActionResult GetToken(string userID)
        {
            //if (model.Username == "012344" && model.Password == "1122")
            //{
            var token = new MySerive().GenerateJwtToken(userID, _configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], _configuration["Jwt:Key"]);
            return Ok(new { token });
            //}
            //return Unauthorized();
        }
        //private string GenerateJwtToken(string userId, string issuer, string audience, string key)
        //{
        //    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        //    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        //    var claims = new[] { new Claim(JwtRegisteredClaimNames.Sub, userId), new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) };
        //    var token = new JwtSecurityToken(issuer: issuer, audience: audience, claims: claims, expires: DateTime.Now.AddMinutes(30), signingCredentials: credentials);
        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}
    }
    public class LoginModel
    {
        public string? Username { get; set; }
        //public string? Password { get; set; }
    }
}
