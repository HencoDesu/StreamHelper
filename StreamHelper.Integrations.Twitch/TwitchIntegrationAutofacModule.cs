using Autofac;
using Microsoft.Extensions.Configuration;
using StreamHelper.Core;
using StreamHelper.Core.Commons;
using StreamHelper.Core.Extensions;
using StreamHelper.Integrations.Twitch.Abstractions.Configuration;
using StreamHelper.Integrations.Twitch.Factories;
using TwitchLib.Api.Interfaces;

namespace StreamHelper.Integrations.Twitch;

public class TwitchIntegrationAutofacModule : AutofacModuleBase
{
    public TwitchIntegrationAutofacModule(IConfiguration configuration)
        : base(configuration)
    {
    }

    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);

        builder.Register(_ => Configuration.GetRequiredSection<TwitchClientConfiguration>("TwitchClientSettings"))
               .SingleInstance();

        builder.RegisterType<TwitchApiFactory>()
               .AsImplementedInterfaces();

        builder.RegisterType<TwitchApiSettingsFactory>()
               .AsImplementedInterfaces();

        builder.Register(p => p.Resolve<IFactory<ITwitchAPI>>().Create());
    }
}