using System.Reflection;
using Catears.EasyTestConstruct.Resolvers;

namespace Catears.EasyTestConstruct.ResolverFactories;

internal class AggregateParameterResolverFactory : IParameterResolverFactory
{
    private List<IParameterResolverFactory> Factories { get; }

    public AggregateParameterResolverFactory(List<IParameterResolverFactory>? factories = null)
    {
        factories ??= new List<IParameterResolverFactory>();
        Factories = factories;
    }

    public IParameterResolver CreateParameterResolverOrThrow(ParameterInfo info)
    {
        if (TryCreateParameterResolver(info, out var resolver))
        {
            return resolver!;
        }

        throw new InvalidOperationException(
            "Tried to create a resolver for parameter but there was no matching factory");
    }

    public bool TryCreateParameterResolver(ParameterInfo parameterInfo, out IParameterResolver? resolver)
    {
        foreach (var factory in Factories)
        {
            if (factory.TryCreateParameterResolver(parameterInfo, out var innerResolver))
            {
                resolver = innerResolver;
                return true;
            }
        }

        resolver = null;
        return false;
    }
}