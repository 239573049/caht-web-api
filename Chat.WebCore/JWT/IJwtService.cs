namespace Chat.WebCore.JWT;
public interface IJwtService
{
    Task<string> CreateTokenAsync(string username);
}