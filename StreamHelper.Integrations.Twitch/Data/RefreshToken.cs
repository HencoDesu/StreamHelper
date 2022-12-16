using Microsoft.AspNetCore.Authentication;
using StreamHelper.Core.Commons;
using StreamHelper.Integrations.Twitch.Data.Exceptions;

namespace StreamHelper.Integrations.Twitch.Data;

public record RefreshToken(string Value) : ValueBase<string>(Value)
{
    public static RefreshToken Default { get; } = new(string.Empty);
    
    public static implicit operator RefreshToken(string value) => new(value);

    public static implicit operator RefreshToken(AuthenticationToken token)
    {
        if (token.Name != Constants.RefreshToken) 
            throw new TokenTypeException(Constants.RefreshToken, token.Name);

        return token.Value;
    }
}