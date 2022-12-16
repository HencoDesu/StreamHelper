using StreamHelper.Integrations.Twitch.Data;
using TwitchLib.Api;
using TwitchLib.Api.Auth;
using TwitchLib.Api.Interfaces;

namespace StreamHelper.Integrations.Twitch.Extensions;

public static class TwitchApiExtensions
{
    public static Task<RefreshResponse> RefreshAccessToken(this ITwitchAPI twitchApi, RefreshToken refreshToken)
    {
        if (twitchApi is not TwitchAPI api)
            throw new NotSupportedException("No way to refresh access token with current implementation");

        var clientSecret = api.Settings.Secret;
        return api.Auth.RefreshAuthTokenAsync(refreshToken, clientSecret);
    }
}