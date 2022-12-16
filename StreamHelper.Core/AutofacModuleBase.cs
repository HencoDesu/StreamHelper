using System.Reflection;
using Autofac;
using Microsoft.Extensions.Configuration;
using StreamHelper.Core.Providers;
using Module = Autofac.Module;

namespace StreamHelper.Core;

public abstract class AutofacModuleBase : Module
{
    protected AutofacModuleBase(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    protected IConfiguration Configuration { get; }

    protected override void Load(ContainerBuilder builder)
    {
        var assembly = Assembly.GetCallingAssembly();

        builder.RegisterAssemblyTypes(assembly)
               .Where(IsProvider)
               .AsImplementedInterfaces()
               .SingleInstance();
    }

    private bool IsProvider(Type type)
    {
        var interfaces = type.GetInterfaces();
        var isProvider = interfaces.Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IProvider<>));
        return isProvider;
    }
}