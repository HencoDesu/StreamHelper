using JetBrains.Annotations;
using StreamHelper.Core.Auth;
using StreamHelper.Core.Providers;

namespace StreamHelper.Core.SongRequests.Providers;

[UsedImplicitly]
public class RequestServiceProvider : ProviderBase<IRequestService>
{
    protected override Task<IRequestService> CreateNew(User user)
        => Task.FromResult<IRequestService>(new RequestService());
}