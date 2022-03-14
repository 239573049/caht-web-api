namespace Chat.WebCore.JWT;
public interface IJwtService
{
    Task<string> CreateTokenAsync(string userId);
    Task<string> CreateTokenAsync(Guid userId);
}