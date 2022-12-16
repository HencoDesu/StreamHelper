using Microsoft.AspNetCore.Authentication;
using StreamHelper.Core.Commons;
using StreamHelper.Integrations.Twitch.Data.Exceptions;

namespace StreamHelper.Integrations.Twitch.Abstractions.Data;

public record AccessToken(string Value) : ValueBase<string>(Value)
{
    public static AccessToken Default { get; } = new(string.Empty);
    
    public static implicit operator AccessToken(string value) => new(value);

    public static implicit operator AccessToken(AuthenticationToken token)
    {
        if (token.Name != Constants.AccessToken) 
            throw new TokenTypeException(Constants.AccessToken, token.Name);

        return token.Value;
    }
}