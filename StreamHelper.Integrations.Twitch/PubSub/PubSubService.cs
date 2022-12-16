using System.Reactive.Subjects;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using StreamHelper.Integrations.Twitch.Data;
using TwitchLib.PubSub.Events;
using TwitchLib.PubSub.Interfaces;
using TwitchLib.PubSub.Models.Responses.Messages.Redemption;

namespace StreamHelper.Integrations.Twitch.PubSub;

[UsedImplicitly]
public sealed class PubSubService : IPubSubService
{
    private readonly Subject<Redemption> _rewardRedeemedSubject = new();
    private readonly Subject<ConnectionStatus> _connectionStatusSubject = new();

    private readonly ITwitchPubSub _twitchPubSub;
    private readonly ILogger<PubSubService> _logger;
    private readonly AuthInfo _authInfo;
    
    private ConnectionStatus _connectionStatus = ConnectionStatus.Disconnected;

    public PubSubService(
        ITwitchPubSub twitchPubSub,
        ILogger<PubSubService> logger,
        AuthInfo authInfo)
    {
        _twitchPubSub = twitchPubSub;
        _logger = logger;
        _authInfo = authInfo;

        _twitchPubSub.OnPubSubServiceConnected += OnPubSubServiceConnected;
        _twitchPubSub.OnPubSubServiceClosed += OnPubSubServiceClosed;
        _twitchPubSub.OnPubSubServiceError += OnPubSubServiceError;
        
        _twitchPubSub.OnListenResponse += OnListenResponse;
        _twitchPubSub.OnChannelPointsRewardRedeemed += OnRewardRedeemed;
        
        _twitchPubSub.ListenToChannelPoints(authInfo.UserId);
    }

    public ConnectionStatus ConnectionStatus
    {
        get => _connectionStatus;
        private set
        {
            _connectionStatus = value;
            _connectionStatusSubject.OnNext(value);
        }
    }

    public IObservable<ConnectionStatus> ConnectionStatusChanged => _connectionStatusSubject;

    public IObservable<Redemption> RewardRedeemed => _rewardRedeemedSubject;

    public void ConnectToChannel()
    {
        ConnectionStatus = ConnectionStatus.Connecting;
        _twitchPubSub.Connect();
    }

    public void DisconnectFromChannel()
    {
        _twitchPubSub.Disconnect();
        ConnectionStatus = ConnectionStatus.Disconnected;
    }

    private void OnPubSubServiceConnected(object? sender, EventArgs e)
    {
        if (ConnectionStatus is ConnectionStatus.Disconnected)
        {
            return;
        }

        _twitchPubSub.SendTopics(_authInfo.AccessToken);
        
        _logger.LogInformation("Connected to TwitchPubSub");
        ConnectionStatus = ConnectionStatus.Connected;
    }
    
    private void OnPubSubServiceClosed(object? sender, EventArgs e)
    {
        _logger.LogInformation("Disconnected from TwitchPubSub");
        ConnectionStatus = ConnectionStatus.Disconnected;
    }
    
    private void OnPubSubServiceError(object? sender, OnPubSubServiceErrorArgs e)
    {
        if (e.Exception is OperationCanceledException)
        {
            return;
        }
        
        _logger.LogInformation("TwitchPubSub error...");
        ConnectionStatus = ConnectionStatus.Disconnected;
    }

    private void OnListenResponse(object? sender, OnListenResponseArgs e)
    {
        if (e.Successful is false)
        {
            _logger.LogError("Error ({Reason}) during connection to TwitchPubSub", e.Response.Error);
            ConnectionStatus = ConnectionStatus.Disconnected;
        }
        else
        {
            _logger.LogInformation("Successful connection to TwitchPubSub");
            ConnectionStatus = ConnectionStatus.Connected;
        }
    }

    private void OnRewardRedeemed(object? sender, OnChannelPointsRewardRedeemedArgs e)
    {
        _rewardRedeemedSubject.OnNext(e.RewardRedeemed.Redemption);
    }
}