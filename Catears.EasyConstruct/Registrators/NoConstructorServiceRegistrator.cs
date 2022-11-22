using Microsoft.Extensions.DependencyInjection;

namespace Catears.EasyConstruct.Registrators;

internal class NoConstructorServiceRegistrator : IServiceRegistrator
{
    public bool TryRegisterService(IServiceCollection collection, ServiceRegistrationContext registrationContext)
    {
        if (registrationContext.Constructors.Length != 0)
        {
            return false;
        }

        collection.AddScoped(registrationContext.ServiceToRegister);
        return true;
    }

    public string AlgorithmPrerequisiteAsDescribedToHuman => "use no constructor";
}