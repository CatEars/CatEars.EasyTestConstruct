using Microsoft.Extensions.DependencyInjection;

namespace Catears.EasyTestConstruct.Registrators;

internal interface IServiceRegistrator
{
    bool TryRegisterService(IServiceCollection collection, ServiceRegistrationContext registrationContext);
    
    string AlgorithmPrerequisiteAsDescribedToHuman { get; }
}