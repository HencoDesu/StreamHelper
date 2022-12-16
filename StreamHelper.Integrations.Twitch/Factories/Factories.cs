using StreamHelper.Core.Commons;
using StreamHelper.Integrations.Twitch.Data;
using TwitchLib.Api.Core.Interfaces;
using TwitchLib.Api.Interfaces;

namespace StreamHelper.Integrations.Twitch.Factories;

public interface ITwitchApiFactory
    : IFactory<ITwitchAPI>,
      IFactory<AuthInfo, ITwitchAPI>
{
}

public interface ITwitchApiSettingsFactory
    : IFactory<IApiSettings>,
      IFactory<AccessToken, IApiSettings>
{
}