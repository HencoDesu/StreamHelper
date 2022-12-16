using Autofac;
using BlazorHost.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using StreamHelper.Core;
using StreamHelper.Core.Auth;

namespace BlazorHost;

public class HostAutoFacModule : AutofacModuleBase
{
    public HostAutoFacModule(IConfiguration configuration)
        : base(configuration)
    {
    }

    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<AuthStateProvider>()
               .As<AuthenticationStateProvider>()
               .As<IAuthProvider>()
               .InstancePerLifetimeScope();

        builder.RegisterType<IdentityProvider>()
               .AsImplementedInterfaces();
    }
}