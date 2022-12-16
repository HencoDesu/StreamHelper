using StreamHelper.Core.Auth;

namespace StreamHelper.Integrations.Twitch.Services;

public interface IRewardsMonitoringService
{
    Task SubscribeToSongRequestRewards(User user);
}