using StreamHelper.Core.Auth;
using StreamHelper.Core.Providers;

namespace BlazorHost.Features.SongRequests.Providers;

public class SongRequestConfigurationProvider : ProviderBase<SongRequestConfiguration>
{
    protected override Task<SongRequestConfiguration> CreateNew(User user)
        => Task.FromResult(new SongRequestConfiguration());
}