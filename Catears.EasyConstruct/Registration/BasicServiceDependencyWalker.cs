namespace Catears.EasyConstruct.Registration;

internal class BasicServiceDependencyWalker : IServiceDependencyWalker
{
    private Type TypeToEnumerate { get; }

    public BasicServiceDependencyWalker(Type typeToEnumerate)
    {
        TypeToEnumerate = typeToEnumerate;
    }

    public IEnumerable<ServiceRegistrationContext> ListDependencies()
    {
        return new List<ServiceRegistrationContext>()
        {
            ServiceRegistrationContext.FromType(TypeToEnumerate)
        };
    }
}