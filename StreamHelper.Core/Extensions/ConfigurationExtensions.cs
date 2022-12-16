using Microsoft.Extensions.Configuration;

namespace StreamHelper.Core.Extensions;

public static class ConfigurationExtensions
{
    public static T GetRequiredSection<T>(this IConfiguration configuration, string key)
        => configuration.GetRequiredSection(key).Get<T>() 
           ?? throw new InvalidOperationException($"Required configuration section {key} is missed");
}