using CatEars.HappyBuild.Registration;

namespace CatEars.HappyBuild.DependencyListers;

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