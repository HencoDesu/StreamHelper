using JetBrains.Annotations;

namespace StreamHelper.Integrations.Twitch.Abstractions.Configuration;

[UsedImplicitly]
public class TwitchClientConfiguration
{
    public required string ClientId { get; set; }
    
    public required string ClientSecret { get; set; }
}