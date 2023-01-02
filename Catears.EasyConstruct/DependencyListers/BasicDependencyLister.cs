using Catears.EasyConstruct.Registration;

namespace Catears.EasyConstruct.DependencyListers;

internal class BasicDependencyLister : IDependencyLister
{
    public IEnumerable<ServiceRegistrationContext> ListDependencies(Type type)
    {
        return new List<ServiceRegistrationContext>()
        {
            ServiceRegistrationContext.FromType(type)
        };
    }
}