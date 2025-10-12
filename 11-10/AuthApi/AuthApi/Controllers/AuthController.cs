using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthApi.Models;

namespace AuthApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private static string refreshToken = "";
        private readonly string secretKey = "this_is_my_secret_key_12345";

        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {
            if (request.Username != "Test" || request.Password != "1")
                return Unauthorized("Sai tài khoản hoặc mật khẩu");

            var accessToken = GenerateToken(request.Username, 1);
            refreshToken = GenerateToken(request.Username, 525600);

            return Ok(new TokenResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            });
        }

        private string GenerateToken(string username, int expireMinutes)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new[] { new Claim(ClaimTypes.Name, username) };
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(expireMinutes),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost("refresh")]
        public IActionResult Refresh(TokenResponse token)
        {
            if (token.RefreshToken != refreshToken)
                return Unauthorized("Refresh token không hợp lệ");

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token.RefreshToken);
            var username = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var newAccessToken = GenerateToken(username, 1);

            return Ok(new TokenResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = token.RefreshToken
            });
        }
    }
}
