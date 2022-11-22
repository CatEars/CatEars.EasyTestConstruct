using System.Reflection;
using Catears.EasyTestConstruct.Resolvers;

namespace Catears.EasyTestConstruct.ResolverFactories;

internal class EnumParameterResolverFactory : IParameterResolverFactory
{
    public bool TryCreateParameterResolver(ParameterInfo parameterInfo, out IParameterResolver? resolver)
    {
        var type = parameterInfo.ParameterType;
        if (type.IsEnum)
        {
            resolver = new EnumResolver(type);
            return true;
        }

        resolver = null;
        return false;
    }
}