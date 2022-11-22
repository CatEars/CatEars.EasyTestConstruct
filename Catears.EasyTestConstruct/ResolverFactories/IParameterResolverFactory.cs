using System.Reflection;
using Catears.EasyTestConstruct.Resolvers;

namespace Catears.EasyTestConstruct.ResolverFactories;

internal interface IParameterResolverFactory
{
    bool TryCreateParameterResolver(ParameterInfo parameterInfo, out IParameterResolver? resolver);
}