using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StreamHelper.Core.Extensions;
using StreamHelper.Integrations.Twitch.Abstractions.Configuration;

namespace StreamHelper.Integrations.Twitch.Abstractions.Extensions;

public static class AuthenticationBuilderExtensions
{
    public static AuthenticationBuilder AddTwitch(
        this AuthenticationBuilder builder,
        IConfiguration configuration)
    {
        var clientConfig = configuration.GetRequiredSection<TwitchClientConfiguration>("TwitchClientSettings");

        builder.AddTwitch(options =>
        {
            options.ClientId = clientConfig.ClientId;
            options.ClientSecret = clientConfig.ClientSecret;
            options.SaveTokens = true;
            
            options.Scope.Add("chat:edit");
            options.Scope.Add("chat:read");
            options.Scope.Add("channel:read:redemptions");
            options.Scope.Add("channel:manage:redemptions");
        });

        return builder;
    }
}