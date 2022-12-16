using System.Reactive.Subjects;
using JetBrains.Annotations;
using StreamHelper.Core.Auth;
using StreamHelper.Core.Providers;
using StreamHelper.Core.SongRequests.Data;

namespace StreamHelper.Core.SongRequests.Providers;

[UsedImplicitly]
public class RequestHistoryProvider 
    : ProviderBase<RequestHistory>
{
    private readonly Subject<RequestInfo> _appendedSubject = new();

    public IObservable<RequestInfo> HistoryAppended => _appendedSubject;

    protected override Task<RequestHistory> CreateNew(User user) 
        => Task.FromResult(new RequestHistory());
}