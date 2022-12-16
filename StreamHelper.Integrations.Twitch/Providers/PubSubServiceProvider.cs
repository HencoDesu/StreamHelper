using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using StreamHelper.Core.Auth;
using StreamHelper.Core.Providers;
using StreamHelper.Integrations.Twitch.Abstractions.Services;
using StreamHelper.Integrations.Twitch.Data;
using StreamHelper.Integrations.Twitch.Services;
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
        var authInfo = await _authInfoProvider.Get(user);
        var logger = _loggerFactory.CreateLogger<PubSubService>();
        
        return new PubSubService(logger, authInfo);
    }
}