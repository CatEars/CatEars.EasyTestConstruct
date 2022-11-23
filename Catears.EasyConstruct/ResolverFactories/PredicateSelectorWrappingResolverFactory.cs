using System.Reflection;
using Catears.EasyConstruct.Resolvers;

namespace Catears.EasyConstruct.ResolverFactories;

internal class PredicateSelectorWrappingResolverFactory : IParameterResolverFactory
{
    private IParameterResolverFactory WrappedParameterResolverFactory { get; }
    
    private Predicate<ParameterInfo> SelectionCriteria { get; }

    public PredicateSelectorWrappingResolverFactory(Predicate<ParameterInfo> selectionCriteria, IParameterResolverFactory wrappedParameterResolverFactory)
    {
        WrappedParameterResolverFactory = wrappedParameterResolverFactory;
        SelectionCriteria = selectionCriteria;
    }
    
    public bool TryCreateParameterResolver(ParameterInfo parameterInfo, out IParameterResolver? resolver)
    {
        if (SelectionCriteria(parameterInfo))
        {
            return WrappedParameterResolverFactory.TryCreateParameterResolver(parameterInfo, out resolver);
        }

        resolver = null;
        return false;
    }
}