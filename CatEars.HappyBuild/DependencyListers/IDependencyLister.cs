using CatEars.HappyBuild.Registration;

namespace CatEars.HappyBuild.DependencyListers;

internal interface IDependencyLister
{
    IEnumerable<ServiceRegistrationContext> ListDependencies(Type type);

    bool HasEncounteredType(Type type);
}