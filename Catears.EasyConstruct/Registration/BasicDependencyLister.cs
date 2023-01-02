namespace Catears.EasyConstruct.Registration;

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