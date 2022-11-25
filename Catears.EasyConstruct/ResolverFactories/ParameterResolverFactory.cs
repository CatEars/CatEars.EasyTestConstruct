using System.Reflection;
using Catears.EasyConstruct.Resolvers;

namespace Catears.EasyConstruct.ResolverFactories;

internal static class ParameterResolverFactory
{
    private static Dictionary<Type, Func<ParameterInfo, IParameterResolver>> RegisteredResolvers { get; } = new()
    {
        { typeof(int), _ => new IntResolver() },
        { typeof(string), StringResolver.CreateFromParamInfo },
        { typeof(float), _ => new FloatResolver() },
    };

    private static bool IsEnum(ParameterInfo paramInfo)
    {
        return paramInfo.ParameterType.IsEnum;
    }

    private static IEnumerable<AdvancedResolver> AdvancedRegisteredResolvers { get; } = new List<AdvancedResolver>()
    {
        new(IsEnum, info => new EnumResolver(info.ParameterType))
    };

    public static IParameterResolver GetResolverForType(ParameterInfo info)
    {
        var type = info.ParameterType;
        if (RegisteredResolvers.TryGetValue(type, out var resolverBuilder))
        {
            return resolverBuilder(info);
        }

        foreach (var advancedResolver in AdvancedRegisteredResolvers)
        {
            if (advancedResolver.Predicate(info))
            {
                return advancedResolver.Builder(info);
            }
        }
        
        return new DelegatingResolver(type);
    }
}