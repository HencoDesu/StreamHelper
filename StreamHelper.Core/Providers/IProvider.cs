using JetBrains.Annotations;
using StreamHelper.Core.Auth;

namespace StreamHelper.Core.Providers;

[UsedImplicitly(ImplicitUseTargetFlags.WithInheritors)]
public interface IProvider<TProvided>
{
    Task<TProvided> Get(User user);
}