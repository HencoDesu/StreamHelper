using System.Reactive.Subjects;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using StreamHelper.Integrations.Twitch.Abstractions.Services;
using StreamHelper.Integrations.Twitch.Data;
using TwitchLib.PubSub;
using TwitchLib.PubSub.Events;
using TwitchLib.PubSub.Interfaces;
using TwitchLib.PubSub.Models.Responses.Messages.Redemption;

namespace StreamHelper.Integrations.Twitch.Services;

[UsedImplicitly]
public sealed class PubSubService : IPubSubService
{
    private readonly Subject<Redemption> _rewardRedeemedSubject = new();
    private readonly Subject<ConnectionStatus> _connectionStatusSubject = new();

    private readonly ILogger<PubSubService> _logger;
    private readonly AuthInfo _authInfo;
    
    private ConnectionStatus _connectionStatus = ConnectionStatus.Disconnected;
    private ITwitchPubSub? _twitchPubSub;

    public PubSubService(
        ILogger<PubSubService> logger,
        AuthInfo authInfo)
    {
        _logger = logger;
        _authInfo = authInfo;
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
        
        _twitchPubSub = CreatePubSub();
        _twitchPubSub.Connect();
    }

    public void DisconnectFromChannel()
    {
        DisposePubSub();
        ConnectionStatus = ConnectionStatus.Disconnected;
    }

    private ITwitchPubSub CreatePubSub()
    {
        var twitchPubSub = new TwitchPubSub();

        twitchPubSub.OnPubSubServiceConnected += OnPubSubServiceConnected;
        twitchPubSub.OnListenResponse += OnListenResponse;
        twitchPubSub.OnChannelPointsRewardRedeemed += OnRewardRedeemed;
        
        twitchPubSub.ListenToChannelPoints(_authInfo.UserId);

        return twitchPubSub;
    }

    private void DisposePubSub()
    {
        if (_twitchPubSub is null)
        {
            return;
        }
        
        _twitchPubSub.OnPubSubServiceConnected -= OnPubSubServiceConnected;
        _twitchPubSub.OnListenResponse += OnListenResponse;
        _twitchPubSub.OnChannelPointsRewardRedeemed += OnRewardRedeemed;
        
        _twitchPubSub = null;
    }

    private void OnPubSubServiceConnected(object? sender, EventArgs e)
    {
        if (ConnectionStatus is ConnectionStatus.Disconnected || _twitchPubSub is null)
        {
            return;
        }

        _twitchPubSub.SendTopics(_authInfo.AccessToken);
        
        _logger.LogInformation("Connected to TwitchPubSub");
        ConnectionStatus = ConnectionStatus.Connected;
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