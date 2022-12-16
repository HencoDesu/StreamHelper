using StreamHelper.Integrations.Twitch.Data;

namespace BlazorHost.Features.SongRequests;

public class SongRequestConfiguration
{
    public RewardId RequestRewardId { get; set; } = RewardId.Default;
    public RewardId SkipRewardId { get; set; } = RewardId.Default;

    public int SongMaxLengthInMinutes { get; set; } = 10;

    public string Blacklist { get; set; } = "писька";
}