using StreamHelper.Core.Auth;
using StreamHelper.Core.Commons;
using StreamHelper.Core.Providers;
using StreamHelper.Integrations.Twitch.Data;
using TwitchLib.Api.Interfaces;

namespace StreamHelper.Integrations.Twitch.Providers;

public class TwitchApiProvider 
    : ProviderBase<ITwitchAPI>
{
    private readonly IProvider<AuthInfo> _authInfoProvider;
    private readonly IFactory<AuthInfo, ITwitchAPI> _twitchApiFactory;

    public TwitchApiProvider(
        IFactory<AuthInfo, ITwitchAPI> twitchApiFactory, 
        IProvider<AuthInfo> authInfoProvider)
    {
        _twitchApiFactory = twitchApiFactory;
        _authInfoProvider = authInfoProvider;
    }

    public override Task<ITwitchAPI> Get(User user) 
        => CreateNew(user);

    protected override async Task<ITwitchAPI> CreateNew(User user)
    {
        var authInfo = await _authInfoProvider.Get(user);
        return _twitchApiFactory.Create(authInfo);
    }
}