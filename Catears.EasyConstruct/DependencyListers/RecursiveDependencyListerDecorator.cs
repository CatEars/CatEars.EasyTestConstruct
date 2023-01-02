using Catears.EasyConstruct.Registration;

namespace Catears.EasyConstruct.DependencyListers;

internal class RecursiveDependencyListerDecorator : IDependencyLister
{
    private IDependencyLister DecoratedLister { get; }

    public RecursiveDependencyListerDecorator(IDependencyLister decoratedLister)
    {
        DecoratedLister = decoratedLister;
    }

    public IEnumerable<ServiceRegistrationContext> ListDependencies(Type type)
    {
        var visitedTypes = new HashSet<Type>() { type };
        var foundTypes = DecoratedLister.ListDependencies(type).ToList();
        var addedTypes = foundTypes.Select(x => x.ServiceToRegister).ToHashSet();
        for (var idx = 0; idx < foundTypes.Count; ++idx)
        {
            var currentType = foundTypes[idx].ServiceToRegister;
            if (visitedTypes.Contains(currentType))
            {
                continue;
            }
            var addedDependencies = DecoratedLister.ListDependencies(currentType);
            var newDependencies = addedDependencies.Where(x => !addedTypes.Contains(x.ServiceToRegister))
                .ToList();
            visitedTypes.Add(currentType);
            foreach (var dependency in newDependencies)
            {
                foundTypes.Add(dependency);
                addedTypes.Add(dependency.ServiceToRegister);
            }
        }
        
        return foundTypes;
    }
}