using System.Text.RegularExpressions;

namespace StreamHelper.Integrations.Youtube.Helpers;

public static partial class YoutubeLinkHelper
{
    [GeneratedRegex("(?:https?:\\/\\/)?(?:youtu\\.be\\/|(?:www\\.|m\\.)?youtube\\.com\\/(?:watch|v|embed)(?:\\.php)?(?:\\?.*v=|\\/))([a-zA-Z0-9\\\\_-]+)")]
    private static partial Regex YouTubeVideoLinkRegEx();

    public static string GetYoutubeVideoId(string message)
    {
        var match = YouTubeVideoLinkRegEx().Match(message);
        var idGroup = match.Groups[1];
        return idGroup.Value;
    }
}