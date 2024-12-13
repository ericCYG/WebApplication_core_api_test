using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using WebApplication_core_api_test.Service;

namespace WebApplication_core_api_test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Token2Controller : ControllerBase
    {
        private const string SharedKey = "userIndexFake";

        private readonly IConfiguration _configuration;
        public Token2Controller(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult GetToken2(
            [FromHeader(Name = "X-Custom-Signature")] string signature,
            [FromHeader(Name = "X-Timestamp")] string timestamp,
            [FromHeader(Name = "X-UserId")] string userID
            )
        {
            if (string.IsNullOrEmpty(signature) || string.IsNullOrEmpty(timestamp) || string.IsNullOrEmpty(userID))
            {
                return Unauthorized(" headers Null.");
            }

            // 驗證簽名
            string expectedSignature = GenerateHmacSignature(timestamp, SharedKey);
            if (signature != expectedSignature)
            {
                return Unauthorized("Invalid signature.");
            }

            // 簽名過期檢查（1分鐘內有效）
            var requestTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(timestamp));
            if (DateTimeOffset.UtcNow - requestTime > TimeSpan.FromMinutes(1))
            {
                return Unauthorized("Signature expired.");
            }
            var token = new MySerive().GenerateJwtToken(userID, _configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], _configuration["Jwt:Key"]);
            return Ok(new { token });
            // 生成並返回 token2
            //string token2 = Guid.NewGuid().ToString();
            //return Ok(token2);
        }
        private string GenerateHmacSignature(string data, string key)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key)))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
                return Convert.ToBase64String(hash);
            }
        }
    }
}
