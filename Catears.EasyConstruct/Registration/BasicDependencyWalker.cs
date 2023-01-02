namespace Catears.EasyConstruct.Registration;

internal class BasicDependencyWalker : IDependencyWalker
{
    private Type TypeToEnumerate { get; }

    public BasicDependencyWalker(Type typeToEnumerate)
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