using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Chat.WebCore.Base;

public interface IPrincipalAccessor
{
    string Name { get; }

    Guid ID { get; }

    bool? IsAuthenticated();

    IEnumerable<Claim> GetClaimsIdentity();

    List<string> GetClaimValueByType(string ClaimType);

    string GetToken();

    string? GetTenantId();

    List<string> GetUserInfoFromToken(string ClaimType);
}