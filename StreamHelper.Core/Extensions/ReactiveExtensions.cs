using System.Reactive.Linq;

namespace StreamHelper.Core.Extensions;

public static class ReactiveExtensions
{
    public static IDisposable SubscribeAsync<TResult>(this IObservable<TResult> source, Func<Task> action)
        => source.Select(async _ => await action())
                 .Subscribe();
    
    public static IDisposable SubscribeAsync<TResult>(this IObservable<TResult> source, Func<TResult, Task> action) 
        => source.Select(async result => await action(result))
                 .Subscribe();
    
    public static IDisposable Subscribe<TResult>(this IObservable<TResult> source, Action action) 
        => source.Subscribe(_ => action());
}