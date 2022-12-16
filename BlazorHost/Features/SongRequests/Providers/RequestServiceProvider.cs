using BlazorHost.Features.SongRequests.Data;
using BlazorHost.Features.SongRequests.EventArgs;
using Google.Apis.YouTube.v3;
using JetBrains.Annotations;
using StreamHelper.Core.Auth;
using StreamHelper.Core.Extensions;
using StreamHelper.Core.Providers;
using StreamHelper.Integrations.Twitch.Abstractions.Services;
using StreamHelper.Integrations.Twitch.Data;
using TwitchLib.Api.Core.Enums;
using TwitchLib.Api.Helix.Models.ChannelPoints.UpdateCustomRewardRedemptionStatus;
using TwitchLib.Api.Interfaces;
using TwitchLib.PubSub.Models.Responses.Messages.Redemption;

namespace BlazorHost.Features.SongRequests.Providers;

[UsedImplicitly]
public class RequestServiceProvider : ProviderBase<IRequestService>
{
    private readonly IProvider<IPubSubService> _pubSubServiceProvider;
    private readonly IProvider<IChatService> _chatServiceProvider;
    private readonly IProvider<ITwitchAPI> _twitchApiProvider;
    private readonly IProvider<SongRequestConfiguration> _configurationProvider;
    private readonly IProvider<RequestHistory> _historyProvider;
    private readonly IProvider<YouTubeService> _youTubeProvider;
    private readonly IProvider<AuthInfo> _authInfoProvider;

    public RequestServiceProvider(
        IProvider<IPubSubService> pubSubServiceProvider, 
        IProvider<SongRequestConfiguration> configurationProvider, 
        IProvider<RequestHistory> historyProvider, 
        IProvider<YouTubeService> youTubeProvider, 
        IProvider<ITwitchAPI> twitchApiProvider, 
        IProvider<IChatService> chatServiceProvider, 
        IProvider<AuthInfo> authInfoProvider)
    {
        _pubSubServiceProvider = pubSubServiceProvider;
        _configurationProvider = configurationProvider;
        _historyProvider = historyProvider;
        _youTubeProvider = youTubeProvider;
        _twitchApiProvider = twitchApiProvider;
        _chatServiceProvider = chatServiceProvider;
        _authInfoProvider = authInfoProvider;
    }

    protected override async Task<IRequestService> CreateNew(User user)
    {
        var pubSub = await _pubSubServiceProvider.Get(user);
        var configuration = await _configurationProvider.Get(user);
        var youTube = await _youTubeProvider.Get(user);
        var history = await _historyProvider.Get(user);
        
        var requestService = new RequestService(pubSub, configuration, history, youTube);
        requestService.RedemptionProcessed.SubscribeAsync(redemption => OnRewardProcessed(user, redemption));
        
        return requestService;
    }

    private async Task OnRewardProcessed(User user, RedemptionProcessedEventArgs eventArgs)
    {
        var redemption = eventArgs.Redemption;
        
        var authInfo = await _authInfoProvider.Get(user);
        var twitchApi = await _twitchApiProvider.Get(user);
        var chatService = await _chatServiceProvider.Get(user);

        var message = $"@{redemption.User.DisplayName} {eventArgs.Message}";
        chatService.SendMessage(message);
        
        await twitchApi.Helix.ChannelPoints.UpdateRedemptionStatusAsync(
            authInfo.UserId, 
            redemption.Reward.Id,
            redemption.Id.AddToNewList(),
            new UpdateCustomRewardRedemptionStatusRequest
            {
                Status = eventArgs.Result
            });
    }
}