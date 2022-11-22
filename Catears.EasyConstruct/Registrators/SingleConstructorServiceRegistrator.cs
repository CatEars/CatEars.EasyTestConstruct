using Microsoft.Extensions.DependencyInjection;

namespace Catears.EasyConstruct.Registrators;

internal class SingleConstructorServiceRegistrator : IServiceRegistrator
{
    public bool TryRegisterService(IServiceCollection collection, ServiceRegistrationContext registrationContext)
    {
        if (registrationContext.Constructors.Length != 1)
        {
            return false;
        }
        
        AdvancedConstructorRegistrationMethod.Register(
            collection, 
            registrationContext.ServiceToRegister, 
            registrationContext.Constructors.First());
        return true;
    }

    public string AlgorithmPrerequisiteAsDescribedToHuman => "Use exactly one constructor";
}