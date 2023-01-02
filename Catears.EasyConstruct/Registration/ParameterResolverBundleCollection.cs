namespace Catears.EasyConstruct.Registration;

internal record ParameterResolverBundleCollection(Dictionary<Type, ParameterResolverBundle> BundleMap)
{
    internal ParameterResolverBundleCollection Copy() =>
        new(new Dictionary<Type, ParameterResolverBundle>(BundleMap));

    internal static ParameterResolverBundleCollection Empty => 
        new(new Dictionary<Type, ParameterResolverBundle>());
}