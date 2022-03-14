using Chat.WebCore.Base;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Chat.WebCore.JWT;
public class JwtService : IJwtService
{
    public TokenOptions TokenOptions { get; }

    public JwtService(IOptions<TokenOptions> options)
    {
        TokenOptions = options.Value;
    }

    public Task<string> CreateTokenAsync(string userId)
    {
        // 添加一些需要的键值对
        Claim[] claims = new[] { new Claim("user", userId) };
        var keyBytes = Encoding.UTF8.GetBytes(TokenOptions.SecretKey!);
        var creds = new SigningCredentials(new SymmetricSecurityKey(keyBytes),
                                        SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(
            issuer: TokenOptions.Issuer,// 签发者
            audience: TokenOptions.Audience,// 接收者
            claims: claims,// payload
            expires: DateTime.Now.AddMinutes(TokenOptions.ExpireMinutes),// 过期时间
            signingCredentials: creds);// 令牌
        var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        return Task.FromResult(token);
    }

    public async Task<string> CreateTokenAsync(Guid userId)=>
        await CreateTokenAsync(userId.ToString());
}