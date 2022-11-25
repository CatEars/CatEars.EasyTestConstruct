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
    
    public static object RandomFloat(this IServiceProvider provider) =>
        provider.GetRequiredService<FloatProvider>().RandomFloat();
    
    public static object RandomBool(this IServiceProvider provider) =>
        provider.GetRequiredService<BoolProvider>().RandomBool();
    
    public static object RandomByte(this IServiceProvider provider) =>
        provider.GetRequiredService<ByteProvider>().RandomByte();
    
    public static object RandomChar(this IServiceProvider provider) =>
        provider.GetRequiredService<CharProvider>().RandomChar();
    
    public static object RandomDecimal(this IServiceProvider provider) =>
        provider.GetRequiredService<DecimalProvider>().RandomDecimal();
    
    public static object RandomDouble(this IServiceProvider provider) =>
        provider.GetRequiredService<DoubleProvider>().RandomDouble();

    public static object RandomLong(this IServiceProvider provider) =>
        provider.GetRequiredService<LongProvider>().RandomLong();

    public static object RandomSByte(this IServiceProvider provider) =>
        provider.GetRequiredService<SByteProvider>().RandomSByte();
    
    public static object RandomShort(this IServiceProvider provider) =>
        provider.GetRequiredService<ShortProvider>().RandomShort();
    
    public static object RandomUInt(this IServiceProvider provider) =>
        provider.GetRequiredService<UIntProvider>().RandomUInt();
    
    public static object RandomULong(this IServiceProvider provider) =>
        provider.GetRequiredService<ULongProvider>().RandomULong();
    
    public static object RandomUShort(this IServiceProvider provider) =>
        provider.GetRequiredService<UShortProvider>().RandomUShort();
}