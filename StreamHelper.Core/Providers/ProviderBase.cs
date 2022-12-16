using Microsoft.Extensions.Caching.Memory;
using StreamHelper.Core.Auth;

namespace StreamHelper.Core.Providers;

public abstract class ProviderBase<TProvided> : IProvider<TProvided>
{
    private readonly IMemoryCache _cache = new MemoryCache(new MemoryCacheOptions());

    public virtual Task<TProvided> Get(User user)
        => _cache.GetOrCreateAsync(user.Id, _ => CreateNew(user))!;

    protected abstract Task<TProvided> CreateNew(User user);
}