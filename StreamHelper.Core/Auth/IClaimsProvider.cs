using System.Security.Claims;
using StreamHelper.Core.Data;

namespace StreamHelper.Core.Auth;

public interface IClaimsProvider
{
    Task<Claim?> GetClaim(User user, ClaimType claimType);

    Task StoreClaims(User user, IEnumerable<Claim> claims);

    Task StoreClaim(User user, Claim claim);
}