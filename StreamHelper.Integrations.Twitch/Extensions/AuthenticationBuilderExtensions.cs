using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StreamHelper.Core.Extensions;
using StreamHelper.Integrations.Twitch.Configuration;

namespace StreamHelper.Integrations.Twitch.Extensions;

public static class AuthenticationBuilderExtensions
{
    public static AuthenticationBuilder AddTwitch(
        this AuthenticationBuilder builder,
        IConfiguration configuration)
    {
        var clientConfig = configuration.GetRequiredSection<TwitchClientSettings>("TwitchClientSettings");

        builder.AddTwitch(options =>
        {
            options.ClientId = clientConfig.ClientId;
            options.ClientSecret = clientConfig.ClientSecret;
            options.SaveTokens = true;
            
            options.Scope.Add("channel:read:redemptions");
        });

        return builder;
    }
}