using Catears.EasyConstruct.Registration;
using Microsoft.Extensions.DependencyInjection;

namespace Catears.EasyConstruct.Scopes;

internal class DynamicScope : BuildScope
{
    private Func<Type, object>? MockFactoryMethod { get; }

    private ISet<Type> RegisteredTypes { get; }

    public DynamicScope(IServiceCollection serviceCollection,
        Func<Type, object>? mockFactoryMethod) : base(serviceCollection)
    {
        MockFactoryMethod = mockFactoryMethod;
        var registeredServices = serviceCollection.Select(descriptor => descriptor.ServiceType);
        RegisteredTypes = new HashSet<Type>(registeredServices);
    }

    protected override object InternalResolve(Type type)
    {
        EnsureDependencyTreeExists(type);
        return base.InternalResolve(type);
    }

    protected override void InternalMemoize(Type type)
    {
        EnsureDependencyTreeExists(type);
        base.InternalMemoize(type);
    }

    private void EnsureDependencyTreeExists(Type type)
    {
        if (TypeIsAlreadyRegistered(type))
        {
            return;
        }

        AddDependencyTreeToCollection(type);
        InvalidateCurrentProvider();
    }

    private bool TypeIsAlreadyRegistered(Type type)
    {
        return RegisteredTypes.Contains(type);
    }

    private void AddDependencyTreeToCollection(Type type)
    {
        var walker = new RecursiveDependencyWalker();
        walker.DisregardTypes(RegisteredTypes);
        var registrator = new ServiceRegistrator(MockFactoryMethod);
        registrator.RegisterServicesOrThrow(Collection, walker, type);
        RegisteredTypes.UnionWith(registrator.SuccessfullyRegisteredServices);
    }
}