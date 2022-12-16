using BlazorHost.Features.SongRequests.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using StreamHelper.Core.Extensions;
using StreamHelper.Core.Providers;
using StreamHelper.Integrations.Twitch.Abstractions.Services;
using StreamHelper.Integrations.Twitch.Data;
using TwitchLib.Api.Helix.Models.ChannelPoints;
using TwitchLib.Api.Helix.Models.ChannelPoints.CreateCustomReward;
using TwitchLib.Api.Interfaces;

namespace BlazorHost.Features.SongRequests.Pages;

[Authorize]
public partial class Settings
{
    [Inject] private IProvider<AuthInfo> AuthInfoProvider { get; set; } = null!;
    [Inject] private IProvider<IPubSubService> PubSubServiceProvider { get; set; } = null!;
    [Inject] private IProvider<RequestHistory> RequestHistoryProvider { get; set; } = null!;
    [Inject] private IProvider<ITwitchAPI> TwitchApiProvider { get; set; } = null!;
    [Inject] private IProvider<IRequestService> RequestServiceProvider { get; set; } = null!;
    [Inject] private IProvider<SongRequestConfiguration> SongRequestRewardSettingsProvider { get; set; } = null!;

    private IDisposable? _connectionStatusChangedSubscription;
    private IDisposable? _requestHistoryUpdatedSubscription;

    private IPubSubService PubSubService { get; set; } = null!;
    private SongRequestConfiguration SongRequestConfiguration { get; set; } = null!;

    private List<CustomReward> SongRewards { get; set; } = new();
    private List<CustomReward> SkipRewards { get; set; } = new();

    private CustomReward? CurrentSongReward { get; set; }
    private CustomReward? CurrentSkipReward { get; set; }

    private RequestHistory RequestHistory { get; set; } = null!;

    public void Dispose()
    {
        _connectionStatusChangedSubscription?.Dispose();
        _requestHistoryUpdatedSubscription?.Dispose();
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        if (CurrentUser is null)
        {
            return;
        }

        PubSubService = await PubSubServiceProvider.Get(CurrentUser);
        RequestHistory = await RequestHistoryProvider.Get(CurrentUser);
        SongRequestConfiguration = await SongRequestRewardSettingsProvider.Get(CurrentUser);
        _ = await RequestServiceProvider.Get(CurrentUser);

        _connectionStatusChangedSubscription = PubSubService.ConnectionStatusChanged.SubscribeAsync(Redraw);
        _requestHistoryUpdatedSubscription = RequestHistory.HistoryAppended.SubscribeAsync(Redraw);

        var authInfo = await AuthInfoProvider.Get(CurrentUser);
        var twitchApi = await TwitchApiProvider.Get(CurrentUser);
        var rewards =
            await twitchApi.Helix.ChannelPoints.GetCustomRewardAsync(authInfo.UserId, onlyManageableRewards: true);

        SongRewards = rewards.Data.Where(r => r.IsUserInputRequired).ToList();
        CurrentSongReward = SongRewards.FirstOrDefault(r => r.Id == SongRequestConfiguration.RequestRewardId);

        SkipRewards = rewards.Data.Where(r => r.IsUserInputRequired is false).ToList();
        CurrentSkipReward = SkipRewards.FirstOrDefault(r => r.Id == SongRequestConfiguration.SkipRewardId);

        await Redraw();
    }

    private void ChangePubSubServiceState()
    {
        if (PubSubService?.ConnectionStatus is not ConnectionStatus.Disconnected)
        {
            return;
        }

        PubSubService.ConnectToChannel();
    }

    private void OnSongRewardChanged(CustomReward selectedReward)
    {
        CurrentSongReward = selectedReward;
        SongRequestConfiguration.RequestRewardId = selectedReward.Id;
    }

    private void OnSkipRewardChanged(CustomReward selectedReward)
    {
        CurrentSkipReward = selectedReward;
        SongRequestConfiguration.SkipRewardId = selectedReward.Id;
    }

    private async Task CreateSongReward()
    {
        if (CurrentUser is null) return;
        
        var authInfo = await AuthInfoProvider.Get(CurrentUser);
        var twitchApi = await TwitchApiProvider.Get(CurrentUser);

        var rewardCreateResponse = await twitchApi.Helix.ChannelPoints.CreateCustomRewardsAsync(
            authInfo.UserId,
            new CreateCustomRewardsRequest()
            {
                Title = "Заказ музыки/клипа",
                IsUserInputRequired = true,
                Cost = 1
            });
        var reward = rewardCreateResponse.Data.FirstOrDefault();

        if (reward is null)
        {
            return;
        }

        OnSongRewardChanged(reward);
        SongRewards.Add(reward);
        await Redraw();
    }

    private async Task CreateSkipReward()
    {
        if (CurrentUser is null) return;
        
        var authInfo = await AuthInfoProvider.Get(CurrentUser);
        var twitchApi = await TwitchApiProvider.Get(CurrentUser);

        var rewardCreateResponse = await twitchApi.Helix.ChannelPoints.CreateCustomRewardsAsync(
            authInfo.UserId,
            new CreateCustomRewardsRequest()
            {
                Title = "Пропуск музыки/клипа",
                IsUserInputRequired = true,
                Cost = 1
            });
        var reward = rewardCreateResponse.Data.FirstOrDefault();

        if (reward is null)
        {
            return;
        }

        OnSkipRewardChanged(reward);
        SkipRewards.Add(reward);
        await Redraw();
    }

    private string GetConnectionStatusText()
        => PubSubService.ConnectionStatus switch
        {
            ConnectionStatus.Disconnected => "Неактивен",
            ConnectionStatus.Connecting => "Подключение...",
            ConnectionStatus.Connected => "Активен",
            _ => "Неизвестно"
        };

    private Color GetConnectionStatusColor()
        => PubSubService.ConnectionStatus switch
        {
            ConnectionStatus.Disconnected => Color.Default,
            ConnectionStatus.Connected => Color.Primary,
            _ => Color.Default
        };
}