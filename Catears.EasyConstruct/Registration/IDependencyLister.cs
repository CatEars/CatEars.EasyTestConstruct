namespace Catears.EasyConstruct.Registration;

internal interface IDependencyLister
{
    IEnumerable<ServiceRegistrationContext> ListDependencies(Type type);
}