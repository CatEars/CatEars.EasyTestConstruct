using System.Reflection;
using Catears.EasyConstruct.Resolvers;

namespace Catears.EasyConstruct.ResolverFactories;

internal interface IParameterResolverFactory
{
    bool TryCreateParameterResolver(ParameterInfo parameterInfo, out IParameterResolver? resolver);
}