

using Autofac;
using Microsoft.Extensions.Configuration;
using StreamHelper.Core;
using StreamHelper.Core.Commons;
using StreamHelper.Core.Extensions;
using StreamHelper.Integrations.Twitch.Configuration;
using StreamHelper.Integrations.Twitch.Factories;
using StreamHelper.Integrations.Twitch.Services;
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
        
        builder.Register(_ => Configuration.GetRequiredSection<TwitchClientSettings>("TwitchClientSettings"))
               .SingleInstance();

        builder.RegisterType<TwitchApiFactory>()
               .AsImplementedInterfaces();

        builder.RegisterType<TwitchApiSettingsFactory>()
               .AsImplementedInterfaces();

        builder.RegisterType<RewardsMonitoringService>()
               .AsImplementedInterfaces()
               .SingleInstance();

        builder.Register(p => p.Resolve<IFactory<ITwitchAPI>>().Create());
    }
}