using System.Collections;

namespace Catears.EasyConstruct.Registration;

internal class BasicServiceDependencyWalker : IServiceDependencyWalker
{
    private Type TypeToEnumerate { get; }
    
    public BasicServiceDependencyWalker(Type typeToEnumerate)
    {
        TypeToEnumerate = typeToEnumerate;
    }
    
    public IEnumerator<ServiceRegistrationContext> GetEnumerator()
    {
        return new List<ServiceRegistrationContext>()
        {
            ServiceRegistrationContext.FromType(TypeToEnumerate)
        }.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}