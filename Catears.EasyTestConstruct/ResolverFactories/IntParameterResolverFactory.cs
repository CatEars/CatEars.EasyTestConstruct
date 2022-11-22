using System.Reflection;
using Catears.EasyTestConstruct.Resolvers;

namespace Catears.EasyTestConstruct.ResolverFactories;

internal class IntParameterResolverFactory : IParameterResolverFactory
{
    public bool TryCreateParameterResolver(ParameterInfo parameterInfo, out IParameterResolver? resolver)
    {
        var type = parameterInfo.ParameterType;
        if (type == typeof(int) || type == typeof(Int32))
        {
            resolver = new IntResolver();
            return true;
        }

        resolver = null;
        return false;
    }
}