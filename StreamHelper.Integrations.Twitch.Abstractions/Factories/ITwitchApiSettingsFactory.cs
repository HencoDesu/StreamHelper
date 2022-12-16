using StreamHelper.Core.Commons;
using StreamHelper.Integrations.Twitch.Abstractions.Data;
using TwitchLib.Api.Core.Interfaces;

namespace StreamHelper.Integrations.Twitch.Abstractions.Factories;

public interface ITwitchApiSettingsFactory
    : IFactory<IApiSettings>,
      IFactory<AccessToken, IApiSettings>
{
}