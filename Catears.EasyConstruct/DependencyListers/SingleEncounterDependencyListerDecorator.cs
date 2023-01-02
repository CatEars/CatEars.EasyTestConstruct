using Catears.EasyConstruct.Registration;

namespace Catears.EasyConstruct.DependencyListers;

internal class SingleEncounterDependencyListerDecorator : IDependencyLister
{
    private IDependencyLister DecoratedLister { get; }
    private ISet<Type> AlreadyEncounteredTypes { get; }
    private ISet<Type> PriorDependenciesListed { get; }
    
    public SingleEncounterDependencyListerDecorator(
        IDependencyLister decoratedLister,
        ISet<Type>? initiallyEncounteredTypes = null)
    {
        DecoratedLister = decoratedLister;
        initiallyEncounteredTypes ??= new HashSet<Type>();
        AlreadyEncounteredTypes = new HashSet<Type>(initiallyEncounteredTypes);
        PriorDependenciesListed = new HashSet<Type>(initiallyEncounteredTypes);
    }

    public IEnumerable<ServiceRegistrationContext> ListDependencies(Type type)
    {
        if (PriorDependenciesListed.Contains(type))
        {
            return Enumerable.Empty<ServiceRegistrationContext>();
        }

        PriorDependenciesListed.Add(type);
        var newDependencies = ListNewDependenciesFromInnerWalker(type);
        RegisterNewDependencies(newDependencies);
        return newDependencies;
    }

    private List<ServiceRegistrationContext> ListNewDependenciesFromInnerWalker(Type type)
    {
        var dependencies = DecoratedLister.ListDependencies(type);
        var newDependencies = dependencies.Where(
                x => !AlreadyEncounteredTypes.Contains(x.ServiceToRegister))
            .ToList();
        return newDependencies;
    }

    private void RegisterNewDependencies(IEnumerable<ServiceRegistrationContext> newDependencies)
    {
        var typesToEncounter = newDependencies.Select(x => x.ServiceToRegister);
        AlreadyEncounteredTypes.UnionWith(typesToEncounter);
    }

    internal bool HasEncounteredType(Type type) => AlreadyEncounteredTypes.Contains(type);
}