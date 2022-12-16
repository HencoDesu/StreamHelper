using Autofac;
using Microsoft.Extensions.Configuration;
using StreamHelper.Core;
using StreamHelper.Core.Extensions;
using StreamHelper.Integrations.Youtube.Abstractions.Configuration;

namespace StreamHelper.Integrations.Youtube;

public class YouTubeIntegrationAutofacModule : AutofacModuleBase
{
    public YouTubeIntegrationAutofacModule(IConfiguration configuration)
        : base(configuration)
    {
    }

    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);

        builder.Register(_ => Configuration.GetRequiredSection<YouTubeConfiguration>("YouTubeSettings"))
               .SingleInstance();
    }
}