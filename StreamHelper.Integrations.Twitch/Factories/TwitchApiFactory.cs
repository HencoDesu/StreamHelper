using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using StreamHelper.Integrations.Twitch.Data;
using TwitchLib.Api;
using TwitchLib.Api.Interfaces;

namespace StreamHelper.Integrations.Twitch.Factories;

[UsedImplicitly]
public class TwitchApiFactory
    : ITwitchApiFactory
{
    private readonly ILoggerFactory _loggerFactory;
    private readonly ITwitchApiSettingsFactory _apiSettingsFactory;

    public TwitchApiFactory(
        ILoggerFactory loggerFactory,
        ITwitchApiSettingsFactory apiSettingsFactory)
    {
        _loggerFactory = loggerFactory;
        _apiSettingsFactory = apiSettingsFactory;
    }

    public ITwitchAPI Create()
        => new TwitchAPI(
            loggerFactory: _loggerFactory,
            settings: _apiSettingsFactory.Create());

    public ITwitchAPI Create(AuthInfo authInfo)
        => new TwitchAPI(
            loggerFactory: _loggerFactory,
            settings: _apiSettingsFactory.Create(authInfo.AccessToken));
}