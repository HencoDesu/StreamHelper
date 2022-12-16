using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using StreamHelper.Core.Auth;
using StreamHelper.Core.Data;

namespace BlazorHost.Authentication;

public class IdentityProvider
    : IClaimsProvider,
      ILoginProvider,
      ITokenProvider
{
    private readonly UserManager<User> _userManager;

    public IdentityProvider(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Claim?> GetClaim(User user, ClaimType claimType)
    {
        var claims = await _userManager.GetClaimsAsync(user);
        return claims.FirstOrDefault(c => c.Type == claimType);
    }

    public Task StoreClaims(User user, IEnumerable<Claim> claims)
        => _userManager.AddClaimsAsync(user, claims);


    public Task StoreClaim(User user, Claim claim)
        => _userManager.AddClaimAsync(user, claim);

    public async Task<UserLoginInfo?> GetLogin(User user, LoginProvider loginProvider)
    {
        var userLogins = await _userManager.GetLoginsAsync(user);
        return userLogins.FirstOrDefault(l => l.LoginProvider == loginProvider);
    }

    public Task StoreLogin(User user, UserLoginInfo loginInfo)
        => _userManager.AddLoginAsync(user, loginInfo);

    public Task<string?> GetToken(User user, LoginProvider loginProvider, TokenType tokenType) 
        => _userManager.GetAuthenticationTokenAsync(user, loginProvider, tokenType);

    public Task StoreToken(User user, LoginProvider loginProvider, TokenType tokenType, string tokenValue)
        => _userManager.SetAuthenticationTokenAsync(user, loginProvider, tokenType, tokenValue);
}