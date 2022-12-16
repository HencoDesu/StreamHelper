using Microsoft.Extensions.Logging;
using StreamHelper.Integrations.Twitch.Abstractions.Services;
using StreamHelper.Integrations.Twitch.Data;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Interfaces;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Interfaces;

namespace StreamHelper.Integrations.Twitch.Services;

public class ChatService : IChatService
{
    private readonly ILogger<ChatService> _logger;
    private readonly ITwitchClient _twitchClient;
    private readonly AuthInfo _authInfo;

    public ChatService(
        AuthInfo authInfo,
        ConnectionCredentials credentials,
        IClient webSocketClient, 
        ILogger<ChatService> logger)
    {
        _authInfo = authInfo;
        _logger = logger;
        _twitchClient = new TwitchClient(webSocketClient);
        
        _twitchClient.Initialize(credentials);
        
        _twitchClient.OnConnected += OnConnected;
        _twitchClient.OnJoinedChannel += OnJoinedChannel;

        _twitchClient.Connect();
        
        _twitchClient.JoinChannel(_authInfo.DisplayName);
    }

    private void OnJoinedChannel(object? sender, OnJoinedChannelArgs e)
    {
        _logger.LogInformation("Joined to channel {ChannelName}", e.Channel);
    }

    private void OnConnected(object? sender, OnConnectedArgs e)
    {
        _logger.LogInformation("Connected to Twitch chat");
    }

    public void SendMessage(string message)
    {
        try
        {
            _twitchClient.SendMessage(_authInfo.DisplayName, message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ошибка при отправке сообщщения");
            throw;
        }
    }
}