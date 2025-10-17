using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Models;
public class TokenService
{
    private readonly string _key;
    public List<RefreshTokenEntry> RefreshStore = new();
    public TokenService(string key){_key=key;}
    public TokenResponse CreateTokens(string username)
    {
        var now = DateTime.UtcNow;
        var accessExp = now.AddMinutes(1);
        var refreshExp = now.AddDays(7);
        var claims = new[] {new Claim(ClaimTypes.Name,username)};
        var creds = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key)),SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(claims:claims,expires:accessExp,signingCredentials:creds);
        var access = new JwtSecurityTokenHandler().WriteToken(token);
        var refresh = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        RefreshStore.RemoveAll(x => x.Username==username);
        RefreshStore.Add(new RefreshTokenEntry(refresh,username,refreshExp));
        return new TokenResponse(access,refresh,new DateTimeOffset(accessExp).ToUnixTimeSeconds());
    }
    public TokenResponse? Refresh(string refreshToken)
    {
        var entry = RefreshStore.FirstOrDefault(x => x.Token==refreshToken);
        if(entry==null) return null;
        if(entry.ExpireAt < DateTime.UtcNow) return null;
        return CreateTokens(entry.Username);
    }
}
