using Chat.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Chat.WebCore.Base;

public class PrincipalAccessor : IPrincipalAccessor
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly string X_TENANT_ID = Constants.TenantHeader;

    public PrincipalAccessor(IHttpContextAccessor accessor) => this._contextAccessor = accessor;

    public string Name => _contextAccessor.HttpContext.User.Identity?.Name??string.Empty;

    public Guid ID => Guid.Parse(GetClaimValueByType(Constants.User).FirstOrDefault() ?? Guid.Empty.ToString());

    public bool? IsAuthenticated() => _contextAccessor.HttpContext.User.Identity?.IsAuthenticated;

    public string GetToken() => _contextAccessor.HttpContext.Request.Headers[Constants.JwtHeader].ToString().Replace(Constants.JwtType, "");

    public IEnumerable<Claim> GetClaimsIdentity() => _contextAccessor.HttpContext.User.Claims;

    public List<string> GetClaimValueByType(string ClaimType)
    {
        return GetClaimsIdentity().Where(item => item.Type == ClaimType).Select(item => item.Value).ToList();
    }

    public List<string> GetUserInfoFromToken(string ClaimType)
    {
        JwtSecurityTokenHandler securityTokenHandler = new JwtSecurityTokenHandler();
        string token = GetToken();
        return !string.IsNullOrEmpty(token) ? securityTokenHandler.ReadJwtToken(token).Claims.Where(item => item.Type == ClaimType).Select(item => item.Value).ToList() : new List<string>();
    }
    public string GetUserId(string token)
    {
        JwtSecurityTokenHandler securityTokenHandler = new JwtSecurityTokenHandler();
        return securityTokenHandler.ReadJwtToken(token).Claims.Where(item => item.Type == "user").Select(item => item.Value).FirstOrDefault()??"";
    }
    public string? GetTenantId()
    {
        HttpContext httpContext = _contextAccessor.HttpContext;
        return httpContext != null && httpContext.Request.Headers.ContainsKey(X_TENANT_ID) ? ((IEnumerable<string>)(object)httpContext.Request.Headers[this.X_TENANT_ID]).FirstOrDefault() : null;
    }

    public Guid UserId()
    {
        var user = ID;
        if (user == Guid.Empty) throw new BusinessLogicException(401, "账号未授权");
        return user;
    }
}