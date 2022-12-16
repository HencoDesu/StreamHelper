using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Xml;
using BlazorHost.Features.SongRequests.Data;
using BlazorHost.Features.SongRequests.EventArgs;
using Google.Apis.YouTube.v3;
using JetBrains.Annotations;
using StreamHelper.Core.Extensions;
using StreamHelper.Integrations.Twitch.Abstractions.Services;
using StreamHelper.Integrations.Youtube.Helpers;
using TwitchLib.Api.Core.Enums;
using TwitchLib.PubSub.Models.Responses.Messages.Redemption;

namespace BlazorHost.Features.SongRequests;

[UsedImplicitly]
public class RequestService
    : IRequestService
{
    private readonly Subject<RedemptionProcessedEventArgs> _redemptionProcessed = new();

    private readonly SongRequestConfiguration _configuration;
    private readonly RequestHistory _history;
    private readonly YouTubeService _youTubeService;

    public RequestService(
        IPubSubService pubSubService,
        SongRequestConfiguration configuration, 
        RequestHistory history, 
        YouTubeService youTubeService)
    {
        _configuration = configuration;
        _history = history;
        _youTubeService = youTubeService;

        pubSubService.RewardRedeemed
                     .Where(r => r.Reward.Id == _configuration.RequestRewardId)
                     .SubscribeAsync(OnSongRequested);

        pubSubService.RewardRedeemed
                      .Where(r => r.Reward.Id == _configuration.SkipRewardId)
                      .Subscribe(OnSongSkipped);
    }

    public IObservable<RedemptionProcessedEventArgs> RedemptionProcessed => _redemptionProcessed;

    private async Task OnSongRequested(Redemption requestInfo)
    {
        var videoId = YoutubeLinkHelper.GetYoutubeVideoId(requestInfo.UserInput);

        if (string.IsNullOrEmpty(videoId))
        {
            DeclineRedemption(requestInfo, "Ты точно не забыл ссылку на видео?");
            return;
        }
        
        var videoInfoRequest = _youTubeService.Videos.List(new[] { "snippet", "status", "contentDetails" });
        videoInfoRequest.Id = videoId;
        var videoResponse = await videoInfoRequest.ExecuteAsync();
        var video = videoResponse.Items.FirstOrDefault();

        if (video is null)
        {
            DeclineRedemption(requestInfo, "Ты уверен что это видео вообще существует?");
            return;
        }

        if (video.Status.Embeddable is false)
        {
            DeclineRedemption(requestInfo, "У этого видео отключено встраивание, попробуй другое видео.");
            return;
        }
        
        if (video.ContentDetails.ContentRating.YtRating is "ytAgeRestricted")
        {
            DeclineRedemption(requestInfo, "У этого видео стоит возрастное ограничение, попробуй другое видео.");
            return;
        }

        if (XmlConvert.ToTimeSpan(video.ContentDetails.Duration).TotalMinutes > _configuration.SongMaxLengthInMinutes)
        {
            DeclineRedemption(requestInfo, $"Видео слишком длинное, попробуй найти видео не длиннее {_configuration.SongMaxLengthInMinutes} минут.");
            return;
        }

        var request = new RequestInfo
        {
            RequesterName = requestInfo.User.DisplayName,
            RequestTime = requestInfo.RedeemedAt,
            SongAuthor = video.Snippet.ChannelTitle,
            SongName = video.Snippet.Title,
            YouTubeVideoId = videoId
        };
        
        var eventArgs = new RedemptionProcessedEventArgs
        {
            Redemption = requestInfo,
            Result = CustomRewardRedemptionStatus.FULFILLED,
            Message = $"Твой трек {request.SongAuthor} - {request.SongName} добавлен в очередь"
        };
        _redemptionProcessed.OnNext(eventArgs);
        
        _history.AppendHistory(request);
    }

    private void OnSongSkipped()
    {
        
    }

    private void DeclineRedemption(Redemption redemption, string message)
    {
        var eventArgs = new RedemptionProcessedEventArgs
        {
            Redemption = redemption,
            Result = CustomRewardRedemptionStatus.CANCELED,
            Message = message
        };
        _redemptionProcessed.OnNext(eventArgs);
    }
}