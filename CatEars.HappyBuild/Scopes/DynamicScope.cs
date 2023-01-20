using CatEars.HappyBuild.DependencyListers;
using CatEars.HappyBuild.Registration;
using Microsoft.Extensions.DependencyInjection;

namespace CatEars.HappyBuild.Scopes;

internal class DynamicScope : ControlledBuildScope
{
    private SingleEncounterDependencyListerDecorator EncounterLister { get; }

    private IDependencyLister DependencyLister { get; }
    
    public DynamicScope(IServiceCollection serviceCollection,
        ParameterResolverBundleCollection resolverCollection,
        BuildContext.Options options) : base(serviceCollection, resolverCollection, options)
    {
        var registeredServices = serviceCollection.Select(descriptor => descriptor.ServiceType);
        var encounterLister = new SingleEncounterDependencyListerDecorator(
            new ConstructorParameterDependencyLister(),
            registeredServices.ToHashSet()
        );
        EncounterLister = encounterLister;
        DependencyLister = new RecursiveDependencyListerDecorator(EncounterLister);
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
        if (EncounterLister.HasEncounteredType(type))
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