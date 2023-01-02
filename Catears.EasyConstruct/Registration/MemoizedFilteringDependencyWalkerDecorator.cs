namespace Catears.EasyConstruct.Registration;

internal class MemoizedFilteringDependencyWalkerDecorator : IDependencyWalker
{
    private IDependencyWalker DecoratedWalker { get; }
    private ISet<Type> RegisteredTypes { get; }

    public MemoizedFilteringDependencyWalkerDecorator(
        IDependencyWalker decoratedWalker,
        ISet<Type> initiallyRegisteredTypes)
    {
        DecoratedWalker = decoratedWalker;
        RegisteredTypes = initiallyRegisteredTypes;
    }

    public IEnumerable<ServiceRegistrationContext> ListDependencies(Type type)
    {
        if (RegisteredTypes.Contains(type))
        {
            return Enumerable.Empty<ServiceRegistrationContext>();
        }
        var newDependencies = ListNewDependenciesFromInnerWalker(type);
        RegisterNewDependencies(newDependencies);
        return newDependencies;
    }

    private List<ServiceRegistrationContext> ListNewDependenciesFromInnerWalker(Type type)
    {
        var dependencies = DecoratedWalker.ListDependencies(type);
        var newDependencies = dependencies.Where(
                x => !RegisteredTypes.Contains(x.ServiceToRegister))
            .ToList();
        return newDependencies;
    }

    private void RegisterNewDependencies(List<ServiceRegistrationContext> newDependencies)
    {
        var typesToEncounter = newDependencies.Select(x => x.ServiceToRegister);
        RegisteredTypes.UnionWith(typesToEncounter);
    }
}