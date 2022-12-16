using StreamHelper.Core.Commons;

namespace StreamHelper.Integrations.Twitch.Data;

public record UserId(string Value) : ValueBase<string>(Value)
{
    public static UserId Default { get; } = string.Empty;

    public static implicit operator UserId(string userId) => new(userId);
}