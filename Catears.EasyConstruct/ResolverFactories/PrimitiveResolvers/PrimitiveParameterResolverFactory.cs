using System.Reflection;
using Catears.EasyConstruct.Resolvers;

namespace Catears.EasyConstruct.ResolverFactories.PrimitiveResolvers;

internal abstract class PrimitiveParameterResolverFactory<TExpectedType, TExpectedResolver> : IParameterResolverFactory where TExpectedResolver : IParameterResolver, new()
{
    public bool TryCreateParameterResolver(ParameterInfo parameterInfo, out IParameterResolver? resolver)
    {
        if (parameterInfo.ParameterType == typeof(TExpectedType))
        {
            resolver = new TExpectedResolver();
            return true;
        }

        resolver = null;
        return false;
    }
}