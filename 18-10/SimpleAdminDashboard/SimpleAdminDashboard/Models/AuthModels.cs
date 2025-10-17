namespace Models
{
    public record LoginRequest(string Username,string Password);
    public record TokenResponse(string AccessToken,string RefreshToken,long ExpiresAt);
}
