using Microsoft.Extensions.DependencyInjection;

namespace Catears.EasyConstruct.Providers;

internal static class ServiceCollectionExtensions
{
    public static void RegisterBasicValueProviders(this IServiceCollection collection)
    {
        collection.AddScoped<EnumProvider>();
        collection.AddScoped<StringProvider>();
        collection.AddScoped<IntProvider>();
    }
}