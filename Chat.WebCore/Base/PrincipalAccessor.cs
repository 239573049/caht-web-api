using Chat.WebCore.JWT;
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

    public string? GetTenantId()
    {
        HttpContext httpContext = _contextAccessor.HttpContext;
        return httpContext != null && httpContext.Request.Headers.ContainsKey(this.X_TENANT_ID) ? ((IEnumerable<string>)(object)httpContext.Request.Headers[this.X_TENANT_ID]).FirstOrDefault() : null;
    }
}