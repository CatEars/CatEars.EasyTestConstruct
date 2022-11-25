using Microsoft.Extensions.DependencyInjection;

namespace Catears.EasyConstruct.Providers;

internal static class ServiceCollectionExtensions
{
    public static void RegisterBasicValueProviders(this IServiceCollection collection)
    {
        collection.AddScoped<EnumProvider>();
        collection.AddScoped<StringProvider>();
        collection.AddScoped<IntProvider>();
        collection.AddScoped<FloatProvider>();
        collection.AddScoped<BoolProvider>();
        collection.AddScoped<ByteProvider>();
        collection.AddScoped<SByteProvider>();
        collection.AddScoped<CharProvider>();
        collection.AddScoped<DecimalProvider>();
        collection.AddScoped<DoubleProvider>();
        collection.AddScoped<UIntProvider>();
        collection.AddScoped<LongProvider>();
        collection.AddScoped<ULongProvider>();
        collection.AddScoped<ShortProvider>();
        collection.AddScoped<UShortProvider>();
    }
}