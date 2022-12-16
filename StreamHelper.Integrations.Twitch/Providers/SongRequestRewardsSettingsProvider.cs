using JetBrains.Annotations;
using StreamHelper.Core.Auth;
using StreamHelper.Core.Providers;
using StreamHelper.Integrations.Twitch.Configuration.User;

namespace StreamHelper.Integrations.Twitch.Providers;

[UsedImplicitly]
public class SongRequestRewardsSettingsProvider : ProviderBase<SongRequestRewardsSettings>
{
    protected override Task<SongRequestRewardsSettings> CreateNew(User user)
        => Task.FromResult(new SongRequestRewardsSettings());
}