using System.Collections.Immutable;
using JetBrains.Annotations;
using StreamHelper.Core.SongRequests.Data;

namespace StreamHelper.Core.SongRequests;

[UsedImplicitly]
public class RequestService
    : IRequestService
{
    private readonly Queue<RequestInfo> _queue = new();
    
    public RequestInfo? CurrentRequest { get; private set; }

    public IReadOnlyList<RequestInfo> Queue => _queue.ToImmutableList();

    public void AddRequest(RequestInfo request)
    {
        _queue.Enqueue(request);
        
        if (CurrentRequest is null)
        {
            PlayNext();
        }
    }

    public void PlayNext()
    {
        CurrentRequest = _queue.Dequeue();
    }
}