using CatEars.HappyBuild.DependencyListers;
using CatEars.HappyBuild.Registration;
using Microsoft.Extensions.DependencyInjection;

namespace CatEars.HappyBuild.Scopes;

internal class DynamicScope : BuildScopeImpl
{
    private MockFactory MockFactory { get; }

    private SingleEncounterDependencyListerDecorator EncounterLister { get; }

    private IDependencyLister DependencyLister { get; }
    
    public DynamicScope(IServiceCollection serviceCollection,
        ParameterResolverBundleCollection resolverCollection,
        MockFactory mockFactory) : base(serviceCollection, resolverCollection)
    {
        MockFactory = mockFactory;
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
        var registrator = new ServiceRegistrator(MockFactory, ResolverCollection);
        registrator.RegisterServicesOrThrow(Collection, DependencyLister, type);
    }
}