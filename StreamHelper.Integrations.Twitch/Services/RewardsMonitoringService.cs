using System.Reactive.Linq;
using StreamHelper.Core.Auth;
using StreamHelper.Core.Providers;
using StreamHelper.Core.SongRequests;
using StreamHelper.Core.SongRequests.Data;
using StreamHelper.Integrations.Twitch.Configuration.User;
using StreamHelper.Integrations.Twitch.Data;
using StreamHelper.Integrations.Twitch.PubSub;
using TwitchLib.PubSub.Models.Responses.Messages.Redemption;

namespace StreamHelper.Integrations.Twitch.Services;

public class RewardsMonitoringService
    : IRewardsMonitoringService
{
    private readonly Dictionary<string, List<IDisposable>> _subscriptions = new();
    private readonly IProvider<IPubSubService> _pubSubProvider;
    private readonly IProvider<IRequestService> _requestServiceProvider;
    private readonly IProvider<RequestHistory> _requestHistoryProvider;
    private readonly IProvider<SongRequestRewardsSettings> _rewardSettingsProvider;

    public RewardsMonitoringService(
        IProvider<IPubSubService> pubSubProvider,
        IProvider<IRequestService> requestServiceProvider,
        IProvider<RequestHistory> requestHistoryProvider, 
        IProvider<SongRequestRewardsSettings> rewardSettingsProvider)
    {
        _pubSubProvider = pubSubProvider;
        _requestServiceProvider = requestServiceProvider;
        _requestHistoryProvider = requestHistoryProvider;
        _rewardSettingsProvider = rewardSettingsProvider;
    }

    public async Task SubscribeToSongRequestRewards(User user)
    {
        if (_subscriptions.ContainsKey(user.Id))
        {
            return;
        }
        
        var pubSub = await _pubSubProvider.Get(user);
        var requestService = await _requestServiceProvider.Get(user);
        var requestHistory = await _requestHistoryProvider.Get(user);
        var rewardSettings = await _rewardSettingsProvider.Get(user);

        var subscriptions = new List<IDisposable>
        {
            pubSub.RewardRedeemed
                  .Where(r => r.Reward.Id == rewardSettings.RequestRewardId)
                  .Subscribe(r => OnSongRewardRedeemed(r, requestService, requestHistory)),
            pubSub.RewardRedeemed
                  .Where(r => r.Reward.Id == rewardSettings.SkipRewardId)
                  .Subscribe(r => OnSkipRewardRedeemed(requestService)),
            pubSub.ConnectionStatusChanged
                  .Where(s => s is ConnectionStatus.Disconnected)
                  .Subscribe(_ => _subscriptions[user.Id].ForEach(d => d.Dispose()))
        };
        _subscriptions[user.Id] = subscriptions;
    }

    private void OnSongRewardRedeemed(Redemption r, IRequestService requestService, RequestHistory history)
    {
        var request = new RequestInfo
        {
            RequesterName = r.User.DisplayName,
            RequestTime = r.RedeemedAt,
            SongAuthor = r.Reward.Title,
            SongUrl = r.UserInput
        };

        requestService.AddRequest(request);
        history.AppendHistory(request);
    }

    private void OnSkipRewardRedeemed(IRequestService requestService)
    {
        requestService.PlayNext();
    }
}