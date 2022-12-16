using AspNet.Security.OAuth.Twitch;
using StreamHelper.Core.Data;

namespace StreamHelper.Integrations.Twitch.Abstractions;

public static class Constants
{
    public static LoginProvider LoginProvider => TwitchAuthenticationDefaults.Issuer;

    public static TokenType AccessToken = "access_token";
    public static TokenType RefreshToken = "refresh_token";
    public static TokenType ExpiresAt = "expires_at";

    public static ClaimType DisplayName => TwitchAuthenticationConstants.Claims.DisplayName;
    public static ClaimType ProfileImageUrl => TwitchAuthenticationConstants.Claims.ProfileImageUrl;
}