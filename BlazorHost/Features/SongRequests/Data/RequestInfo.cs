namespace BlazorHost.Features.SongRequests.Data;

public class RequestInfo
{
    public string RequesterName { get; set; } = string.Empty;
    public string SongAuthor { get; set; } = string.Empty;
    public string SongName { get; set; } = string.Empty;
    public string YouTubeVideoId { get; set; } = string.Empty;
    public DateTime RequestTime { get; set; }
}