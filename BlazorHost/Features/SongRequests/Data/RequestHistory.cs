using System.Reactive.Subjects;

namespace BlazorHost.Features.SongRequests.Data;

public class RequestHistory
{
    private readonly Subject<RequestInfo> _appendedSubject = new();
    private readonly List<RequestInfo> _history = new();

    public IObservable<RequestInfo> HistoryAppended => _appendedSubject;

    public IReadOnlyCollection<RequestInfo> History => _history;

    public void AppendHistory(RequestInfo requestInfo)
    {
        _history.Add(requestInfo);
        _appendedSubject.OnNext(requestInfo);
    }
}