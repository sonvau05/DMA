using Microsoft.AspNetCore.Mvc;
using Models;
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly TokenService _tokenService;
    public AuthController(TokenService tokenService){_tokenService=tokenService;}
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest req)
    {
        if(req.Username=="admin" && req.Password=="1")
        {
            var tokens = _tokenService.CreateTokens(req.Username);
            return Ok(tokens);
        }
        return Unauthorized();
    }
    [HttpPost("refresh")]
    public IActionResult Refresh([FromBody] object body)
    {
        var d = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string,string>>(body.ToString()!);
        if(d==null) return BadRequest();
        var rt = d.GetValueOrDefault("refreshToken");
        var newTokens = _tokenService.Refresh(rt);
        if(newTokens==null) return Unauthorized();
        return Ok(newTokens);
    }
}
