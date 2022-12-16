using Microsoft.Extensions.Logging;
using StreamHelper.Core.Auth;
using StreamHelper.Core.Providers;
using StreamHelper.Integrations.Twitch.Abstractions.Services;
using StreamHelper.Integrations.Twitch.Data;
using StreamHelper.Integrations.Twitch.Services;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;

namespace StreamHelper.Integrations.Twitch.Providers;

public class ChatServiceProvider
    : ProviderBase<IChatService>
{
    private readonly IProvider<AuthInfo> _authInfoProvider;
    private readonly ILoggerFactory _loggerFactory;

    public ChatServiceProvider(
        IProvider<AuthInfo> authInfoProvider, 
        ILoggerFactory loggerFactory)
    {
        _authInfoProvider = authInfoProvider;
        _loggerFactory = loggerFactory;
    }

    protected override async Task<IChatService> CreateNew(User user)
    {
        var authInfo = await _authInfoProvider.Get(user);
        var credentials = new ConnectionCredentials(authInfo.DisplayName, authInfo.AccessToken);
        var clientOptions = new ClientOptions
        {
            MessagesAllowedInPeriod = 750,
            ThrottlingPeriod = TimeSpan.FromSeconds(30)
        };
        var customClient = new WebSocketClient(clientOptions);
        var logger = _loggerFactory.CreateLogger<ChatService>();

        return new ChatService(authInfo, credentials, customClient, logger);
    }
}