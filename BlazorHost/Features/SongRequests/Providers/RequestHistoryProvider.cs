using System.Reactive.Subjects;
using BlazorHost.Features.SongRequests.Data;
using JetBrains.Annotations;
using StreamHelper.Core.Auth;
using StreamHelper.Core.Providers;

namespace BlazorHost.Features.SongRequests.Providers;

[UsedImplicitly]
public class RequestHistoryProvider 
    : ProviderBase<RequestHistory>
{
    private readonly Subject<RequestInfo> _appendedSubject = new();

    public IObservable<RequestInfo> HistoryAppended => _appendedSubject;

    protected override Task<RequestHistory> CreateNew(User user) 
        => Task.FromResult(new RequestHistory());
}