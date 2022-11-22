using Catears.EasyConstruct.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace Catears.EasyConstruct;

public static class ServiceProviderExtensions
{
    public static int RandomInt(this IServiceProvider provider) =>
        provider.GetRequiredService<IntProvider>().RandomInt();

    public static string RandomString(this IServiceProvider provider) =>
        provider.GetRequiredService<StringProvider>().RandomString();

    public static string RandomString(this IServiceProvider provider, StringProviderOptions options) =>
        provider.GetRequiredService<StringProvider>().RandomString(options);

    public static object RandomEnum<T>(this IServiceProvider provider) =>
        provider.GetRequiredService<EnumProvider>().RandomEnum<T>();

    public static object RandomEnum(this IServiceProvider provider, Type enumType) =>
        provider.GetRequiredService<EnumProvider>().RandomEnum(enumType);
}