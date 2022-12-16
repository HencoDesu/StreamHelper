using StreamHelper.Core.Commons;

namespace StreamHelper.Integrations.Twitch.Data;

public record RewardId(string Value) : ValueBase<string>(Value)
{
    public static RewardId Default { get; } = string.Empty;

    public static implicit operator RewardId(string rewardId) => new(rewardId);
}