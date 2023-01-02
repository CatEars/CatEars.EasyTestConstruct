using Catears.EasyConstruct.Registration;
using Microsoft.Extensions.DependencyInjection;

namespace Catears.EasyConstruct.Scopes;

public class DynamicScope : BuildScope
{
    private Func<Type, object>? MockFactoryMethod { get; }

    public DynamicScope(IServiceCollection serviceCollection, 
        Func<Type, object>? mockFactoryMethod) : base(serviceCollection)
    {
        MockFactoryMethod = mockFactoryMethod;
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
        return Collection.Any(descriptor => descriptor.ServiceType == type);
    }

    private void AddDependencyTreeToCollection(Type type)
    {
        var walker = new RecursiveServiceDependencyWalker(type);
        var registrator = new ServiceRegistrator(MockFactoryMethod);
        registrator.RegisterServicesOrThrow(Collection, walker);
    }
}