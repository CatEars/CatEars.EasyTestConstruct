using Catears.EasyConstruct.Registration;

namespace Catears.EasyConstruct.DependencyListers;

internal interface IDependencyLister
{
    IEnumerable<ServiceRegistrationContext> ListDependencies(Type type);
}