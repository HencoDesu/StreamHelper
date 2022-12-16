using StreamHelper.Core.Extensions;

namespace StreamHelper.Integrations.Twitch.Data;

public record AuthInfo
{
    public AccessToken AccessToken { get; init; } = AccessToken.Default;
    public RefreshToken RefreshToken { get; init; } = RefreshToken.Default;
    public DateTime? ExpiresAt { get; init; }
    
    public required UserId UserId { get; init; }
    public required string DisplayName { get; init; }
    public required string ProfileImage { get; init; }

    public bool AccessTokenExpired => ExpiresAt <= DateTime.UtcNow + 1.Minutes();
}