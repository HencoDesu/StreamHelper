using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using StreamHelper.Core.Auth;
using StreamHelper.Core.Providers;
using StreamHelper.Integrations.Youtube.Abstractions.Configuration;

namespace StreamHelper.Integrations.Youtube.Providers;

public class YouTubeServiceProvider
    : IProvider<YouTubeService>
{
    private readonly YouTubeConfiguration _configuration;
    private readonly Lazy<YouTubeService> _instance;

    public YouTubeServiceProvider(YouTubeConfiguration configuration)
    {
        _configuration = configuration;
        
        _instance = new Lazy<YouTubeService>(CreateNew);
    }

    public Task<YouTubeService> Get(User user)
        => Task.FromResult(_instance.Value);

    private YouTubeService CreateNew()
    {
        var initializer = new BaseClientService.Initializer
        {
            ApplicationName = "StreamHelper",
            ApiKey = _configuration.ApiKey
        };

        return new YouTubeService(initializer);
    }
}