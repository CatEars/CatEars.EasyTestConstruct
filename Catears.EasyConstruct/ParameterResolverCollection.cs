using System.Reflection;
using Catears.EasyConstruct.Resolvers;

namespace Catears.EasyConstruct;

internal static class ParameterResolverCollection
{
    private record PredicateResolver(Predicate<ParameterInfo> Predicate,
        Func<ParameterInfo, IParameterResolver> Builder);

    private static Dictionary<Type, Func<ParameterInfo, IParameterResolver>> RegisteredResolvers { get; } = new()
    {
        { typeof(int), _ => new FuncResolver(provider => provider.RandomInt()) },
        { typeof(string), StringResolver.CreateFromParamInfo },
        { typeof(float), _ => new FuncResolver(provider => provider.RandomFloat()) },
        { typeof(bool), _ => new FuncResolver(provider => provider.RandomBool()) },
        { typeof(byte), _ => new FuncResolver(provider => provider.RandomByte()) },
        { typeof(char), _ => new FuncResolver(provider => provider.RandomChar()) },
        { typeof(decimal), _ => new FuncResolver(provider => provider.RandomDecimal()) },
        { typeof(double), _ => new FuncResolver(provider => provider.RandomDouble()) },
        { typeof(long), _ => new FuncResolver(provider => provider.RandomLong()) },
        { typeof(sbyte), _ => new FuncResolver(provider => provider.RandomSByte()) },
        { typeof(short), _ => new FuncResolver(provider => provider.RandomShort()) },
        { typeof(uint), _ => new FuncResolver(provider => provider.RandomUInt()) },
        { typeof(ulong), _ => new FuncResolver(provider => provider.RandomULong()) },
        { typeof(ushort), _ => new FuncResolver(provider => provider.RandomUShort()) }
    };

    private static IEnumerable<PredicateResolver> RegisteredPredicateResolvers { get; } = new List<PredicateResolver>()
    {
        new(IsEnum, info => new EnumResolver(info.ParameterType))
    };
    
    private static bool IsEnum(ParameterInfo paramInfo)
    {
        return paramInfo.ParameterType.IsEnum;
    }
    
    public static IParameterResolver GetResolverForType(ParameterInfo info)
    {
        var type = info.ParameterType;
        if (RegisteredResolvers.TryGetValue(type, out var resolverBuilder))
        {
            return resolverBuilder(info);
        }

        foreach (var advancedResolver in RegisteredPredicateResolvers)
        {
            if (advancedResolver.Predicate(info))
            {
                return advancedResolver.Builder(info);
            }
        }

        return new DelegatingResolver(type);
    }
}