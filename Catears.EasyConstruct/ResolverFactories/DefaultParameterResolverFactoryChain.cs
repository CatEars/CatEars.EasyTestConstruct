using Catears.EasyConstruct.ResolverFactories.PrimitiveResolvers;

namespace Catears.EasyConstruct.ResolverFactories;

internal static class DefaultParameterResolverFactoryChain
{
    private static AggregateParameterResolverFactory PrimitiveResolverFactory { get; } = new(
        new List<IParameterResolverFactory>()
        {
            new IntParameterResolverFactory(),
            new FloatParameterResolverFactory()
        });

    public static AggregateParameterResolverFactory FirstLink { get; } = new(
        new List<IParameterResolverFactory>()
        {
            new PredicateSelectorWrappingResolverFactory(info => info.ParameterType.IsPrimitive, PrimitiveResolverFactory),
            new EnumParameterResolverFactory(),
            new StringParameterResolverFactory(),
            new DelegatingParameterResolverFactory()
        });

}