using Microsoft.Extensions.DependencyInjection;

namespace CatEars.HappyBuild.Providers;

internal static class ServiceCollectionExtensions
{
    public static void RegisterBasicValueProviders(this IServiceCollection collection)
    {
        collection.AddScoped<EnumProvider>();
        collection.AddScoped<StringProvider>();
        collection.AddScoped<PrimitiveValueProvider>();
    }
}