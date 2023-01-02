namespace Catears.EasyConstruct.Registration;

internal class BasicDependencyWalker : IDependencyWalker
{
    public IEnumerable<ServiceRegistrationContext> ListDependencies(Type type)
    {
        return new List<ServiceRegistrationContext>()
        {
            ServiceRegistrationContext.FromType(type)
        };
    }
}