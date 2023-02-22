using CatEars.HappyBuild.DependencyListers;
using CatEars.HappyBuild.Registration;
using Microsoft.Extensions.DependencyInjection;

namespace CatEars.HappyBuild.Scopes;

internal class DynamicBuildScope : ControlledBuildScope
{
    private IDependencyLister DependencyLister { get; }
    
    public DynamicBuildScope(IServiceCollection serviceCollection,
        ParameterResolverBundleCollection resolverCollection,
        BuildContext.Options options) : base(serviceCollection, resolverCollection, options)
    {
        var registeredServices = serviceCollection.Select(descriptor => descriptor.ServiceType);
        var encounterLister = new DependencyTreeWalkingDependencyLister(
            registeredServices.ToHashSet()
        );
        DependencyLister = encounterLister;
    }

    internal override object InternalResolve(Type type)
    {
        EnsureDependencyTreeExists(type);
        return base.InternalResolve(type);
    }

    internal override void InternalMemoize(Type type)
    {
        EnsureDependencyTreeExists(type);
        base.InternalMemoize(type);
    }

    private void EnsureDependencyTreeExists(Type type)
    {
        if (DependencyLister.HasEncounteredType(type))
        {
            return;
        }

        AddDependencyTreeToCollection(type);
        InvalidateCurrentProvider();
    }

    private void AddDependencyTreeToCollection(Type type)
    {
        var registrator = new ServiceRegistrator(ResolverCollection, Options);
        registrator.RegisterServicesOrThrow(Collection, DependencyLister, type);
    }
}