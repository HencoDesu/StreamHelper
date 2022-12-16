using StreamHelper.Core.Commons;
using StreamHelper.Integrations.Twitch.Data;
using TwitchLib.Api.Interfaces;

namespace StreamHelper.Integrations.Twitch.Abstractions.Factories;

public interface ITwitchApiFactory
    : IFactory<ITwitchAPI>,
      IFactory<AuthInfo, ITwitchAPI>
{
}