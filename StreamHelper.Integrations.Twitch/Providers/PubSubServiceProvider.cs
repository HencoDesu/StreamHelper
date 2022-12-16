using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using StreamHelper.Core.Auth;
using StreamHelper.Core.Providers;
using StreamHelper.Integrations.Twitch.Data;
using StreamHelper.Integrations.Twitch.PubSub;
using TwitchLib.PubSub;

namespace StreamHelper.Integrations.Twitch.Providers;

[UsedImplicitly]
public class PubSubServiceProvider
    : ProviderBase<IPubSubService>
{
    private readonly ILoggerFactory _loggerFactory;
    private readonly IProvider<AuthInfo> _authInfoProvider;

    public PubSubServiceProvider(
        IProvider<AuthInfo> authInfoProvider, 
        ILoggerFactory loggerFactory)
    {
        _authInfoProvider = authInfoProvider;
        _loggerFactory = loggerFactory;
    }

    protected override async Task<IPubSubService> CreateNew(User user)
    {
        var pubSub = new TwitchPubSub(_loggerFactory.CreateLogger<TwitchPubSub>());
        var authInfo = await _authInfoProvider.Get(user);
        return new PubSubService(pubSub, _loggerFactory.CreateLogger<PubSubService>(), authInfo);
    }
}