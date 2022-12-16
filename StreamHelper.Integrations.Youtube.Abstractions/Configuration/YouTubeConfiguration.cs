using JetBrains.Annotations;

namespace StreamHelper.Integrations.Youtube.Abstractions.Configuration;

[UsedImplicitly]
public class YouTubeConfiguration
{
    public required string ApiKey { get; set; }
}