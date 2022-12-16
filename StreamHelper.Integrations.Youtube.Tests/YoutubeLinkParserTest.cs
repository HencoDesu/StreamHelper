using FluentAssertions;
using StreamHelper.Integrations.Youtube.Helpers;

namespace StreamHelper.Integrations.Youtube.Tests;

public class YoutubeLinkParserTest
{
    [Theory]
    [InlineData("nCQ_zZIiGLA", "https://www.youtube.com/watch?v=nCQ_zZIiGLA")]
    [InlineData("nCQ_zZIiGLA", "https://youtu.be/nCQ_zZIiGLA")]
    [InlineData("nCQ_zZIiGLA", "https://m.youtube.com/watch?v=nCQ_zZIiGLA")]
    [InlineData("nCQ_zZIiGLA", "Чекни трек, прям ваще каеф https://youtu.be/nCQ_zZIiGLA")]
    [InlineData("nCQ_zZIiGLA", "https://www.youtube.com/watch?v=nCQ_zZIiGLA&list=PL0i7oXSyOs7laUEUYzm4R7vlFzWOJRD1r&index=1")]
    public void GetYouTubeVideoIdTest(string expectedVideoId, string input)
    {
        var videoId = YoutubeLinkHelper.GetYoutubeVideoId(input);
        videoId.Should().Be(expectedVideoId);
    }
}