using Microsoft.Extensions.DependencyInjection;

namespace Catears.EasyTestConstruct.Providers;

internal static class ServiceCollectionExtensions
{
    public static void RegisterBasicValueProviders(this IServiceCollection collection)
    {
        collection.AddScoped<EnumProvider>();
        collection.AddScoped<StringProvider>();
        collection.AddScoped<IntProvider>();
    }
}