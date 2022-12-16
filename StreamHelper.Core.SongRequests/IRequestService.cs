using StreamHelper.Core.SongRequests.Data;

namespace StreamHelper.Core.SongRequests;

public interface IRequestService
{
    RequestInfo? CurrentRequest { get; }
    IReadOnlyList<RequestInfo> Queue { get; }
    void AddRequest(RequestInfo request);
    void PlayNext();
}