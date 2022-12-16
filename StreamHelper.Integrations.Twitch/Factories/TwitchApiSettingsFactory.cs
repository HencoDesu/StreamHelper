using JetBrains.Annotations;
using StreamHelper.Integrations.Twitch.Abstractions.Configuration;
using StreamHelper.Integrations.Twitch.Abstractions.Data;
using StreamHelper.Integrations.Twitch.Abstractions.Factories;
using TwitchLib.Api.Core;
using TwitchLib.Api.Core.Interfaces;

namespace StreamHelper.Integrations.Twitch.Factories;

[UsedImplicitly]
public class TwitchApiSettingsFactory
    : ITwitchApiSettingsFactory
{
    private readonly TwitchClientConfiguration _clientConfiguration;

    public TwitchApiSettingsFactory(TwitchClientConfiguration clientConfiguration)
    {
        _clientConfiguration = clientConfiguration;
    }

    public IApiSettings Create()
        => new ApiSettings
        {
            ClientId = _clientConfiguration.ClientId,
            Secret = _clientConfiguration.ClientSecret
        };

    public IApiSettings Create(AccessToken accessToken)
        => new ApiSettings
        {
            ClientId = _clientConfiguration.ClientId,
            Secret = _clientConfiguration.ClientSecret,
            AccessToken = accessToken
        };
}