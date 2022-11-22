using Microsoft.Extensions.DependencyInjection;

namespace Catears.EasyTestConstruct.Registrators;

internal class NoConstructorServiceRegistrator : IServiceRegistrator
{
    public bool TryRegisterService(IServiceCollection collection, ServiceRegistrationContext registrationContext)
    {
        if (!registrationContext.Constructors.Any())
        {
            return false;
        }

        collection.AddScoped(registrationContext.ServiceToRegister);
        return true;
    }

    public string AlgorithmPrerequisiteAsDescribedToHuman => "use no constructor";
}