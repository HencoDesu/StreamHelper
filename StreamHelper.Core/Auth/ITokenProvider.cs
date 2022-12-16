using Microsoft.AspNetCore.Authentication;
using StreamHelper.Core.Data;

namespace StreamHelper.Core.Auth;

public interface ITokenProvider
{
    Task<string?> GetToken(User user, LoginProvider loginProvider, TokenType tokenType);

    Task StoreTokens(User user, LoginProvider loginProvider, IEnumerable<AuthenticationToken> tokens);
    
    Task StoreToken(User user, LoginProvider loginProvider, AuthenticationToken token);
}