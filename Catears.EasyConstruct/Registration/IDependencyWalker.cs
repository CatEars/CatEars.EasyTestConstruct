namespace Catears.EasyConstruct.Registration;

internal interface IDependencyWalker
{
    IEnumerable<ServiceRegistrationContext> ListDependencies();
}