using Microsoft.Extensions.DependencyInjection;

namespace Catears.EasyConstruct.Scopes;

public class DynamicScope : BuildScope
{
    
    
    public DynamicScope(IServiceCollection serviceCollection) : base(serviceCollection)
    {
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
        if (Collection.Any(descriptor => descriptor.ServiceType == type))
        {
            return;
        }

        AddDependencyTreeToCollection(type);
        InvalidateCurrentProvider();
    }

    private void AddDependencyTreeToCollection(Type type)
    {
        throw new NotImplementedException();
    }
}