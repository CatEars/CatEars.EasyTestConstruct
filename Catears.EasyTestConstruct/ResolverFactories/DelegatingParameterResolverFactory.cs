using System.Reflection;
using Catears.EasyTestConstruct.Resolvers;

namespace Catears.EasyTestConstruct.ResolverFactories;

internal class DelegatingParameterResolverFactory : IParameterResolverFactory
{
    public bool TryCreateParameterResolver(ParameterInfo parameterInfo, out IParameterResolver? resolver)
    {
        resolver = new DelegatingResolver(parameterInfo.ParameterType);
        return true;
    }
}