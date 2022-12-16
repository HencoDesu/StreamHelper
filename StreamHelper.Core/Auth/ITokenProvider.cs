using StreamHelper.Core.Data;

namespace StreamHelper.Core.Auth;

public interface ITokenProvider
{
    Task<string?> GetToken(User user, LoginProvider loginProvider, TokenType tokenType);

    Task StoreToken(User user, LoginProvider loginProvider, TokenType tokenType, string tokenValue);
}