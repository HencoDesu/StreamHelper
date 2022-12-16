using StreamHelper.Integrations.Twitch.Data;

namespace StreamHelper.Integrations.Twitch.Configuration.User;

public class SongRequestRewardsSettings
{
    public RewardId RequestRewardId { get; set; } = RewardId.Default;
    public RewardId SkipRewardId { get; set; } = RewardId.Default;

    public int SongMaxLengthInMinutes { get; set; } = 10;

    public string Blacklist { get; set; } = "shadowraze";
}