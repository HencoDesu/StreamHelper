using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using StreamHelper.Integrations.Twitch.Configuration;
using StreamHelper.Integrations.Twitch.Data;
using TwitchLib.Api.Core;
using TwitchLib.Api.Core.Interfaces;

namespace StreamHelper.Integrations.Twitch.Factories;

[UsedImplicitly]
public class TwitchApiSettingsFactory
    : ITwitchApiSettingsFactory
{
    private readonly TwitchClientSettings _clientSettings;

    public TwitchApiSettingsFactory(TwitchClientSettings clientSettings)
    {
        _clientSettings = clientSettings;
    }

    public IApiSettings Create()
        => new ApiSettings
        {
            ClientId = _clientSettings.ClientId,
            Secret = _clientSettings.ClientSecret
        };

    public IApiSettings Create(AccessToken accessToken)
        => new ApiSettings
        {
            ClientId = _clientSettings.ClientId,
            Secret = _clientSettings.ClientSecret,
            AccessToken = accessToken
        };
}