using Autofac;
using Microsoft.Extensions.Configuration;

namespace StreamHelper.Core.SongRequests;

public class SongRequestsAutofacModule : AutofacModuleBase
{
    public SongRequestsAutofacModule(IConfiguration configuration) 
        : base(configuration)
    {
    }
    
    // ReSharper disable once RedundantOverriddenMember
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);
    }
}