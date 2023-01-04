using CatEars.HappyBuild.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace CatEars.HappyBuild.Extensions;

public static class ServiceProviderExtensions
{
    public static int RandomInt(this IServiceProvider provider)
    {
        return provider.GetRequiredService<PrimitiveValueProvider>().RandomInt();
    }

    public static string RandomString(this IServiceProvider provider)
    {
        return provider.GetRequiredService<StringProvider>().RandomString();
    }

    public static string RandomString(this IServiceProvider provider, StringProviderOptions options)
    {
        return provider.GetRequiredService<StringProvider>().RandomString(options);
    }

    public static object RandomEnum<T>(this IServiceProvider provider)
    {
        return provider.GetRequiredService<EnumProvider>().RandomEnum<T>();
    }

    public static object RandomEnum(this IServiceProvider provider, Type enumType)
    {
        return provider.GetRequiredService<EnumProvider>().RandomEnum(enumType);
    }

    public static object RandomFloat(this IServiceProvider provider)
    {
        return provider.GetRequiredService<PrimitiveValueProvider>().RandomFloat();
    }

    public static object RandomBool(this IServiceProvider provider)
    {
        return provider.GetRequiredService<PrimitiveValueProvider>().RandomBool();
    }

    public static object RandomByte(this IServiceProvider provider)
    {
        return provider.GetRequiredService<PrimitiveValueProvider>().RandomByte();
    }

    public static object RandomChar(this IServiceProvider provider)
    {
        return provider.GetRequiredService<PrimitiveValueProvider>().RandomChar();
    }

    public static object RandomDecimal(this IServiceProvider provider)
    {
        return provider.GetRequiredService<PrimitiveValueProvider>().RandomDecimal();
    }

    public static object RandomDouble(this IServiceProvider provider)
    {
        return provider.GetRequiredService<PrimitiveValueProvider>().RandomDouble();
    }

    public static object RandomLong(this IServiceProvider provider)
    {
        return provider.GetRequiredService<PrimitiveValueProvider>().RandomLong();
    }

    public static object RandomSByte(this IServiceProvider provider)
    {
        return provider.GetRequiredService<PrimitiveValueProvider>().RandomSByte();
    }

    public static object RandomShort(this IServiceProvider provider)
    {
        return provider.GetRequiredService<PrimitiveValueProvider>().RandomShort();
    }

    public static object RandomUInt(this IServiceProvider provider)
    {
        return provider.GetRequiredService<PrimitiveValueProvider>().RandomUInt();
    }

    public static object RandomULong(this IServiceProvider provider)
    {
        return provider.GetRequiredService<PrimitiveValueProvider>().RandomULong();
    }

    public static object RandomUShort(this IServiceProvider provider)
    {
        return provider.GetRequiredService<PrimitiveValueProvider>().RandomUShort();
    }
}