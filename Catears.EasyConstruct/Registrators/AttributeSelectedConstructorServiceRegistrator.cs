using Microsoft.Extensions.DependencyInjection;

namespace Catears.EasyConstruct.Registrators;

internal class AttributeSelectedConstructorServiceRegistrator : IServiceRegistrator
{
    public bool TryRegisterService(IServiceCollection collection, ServiceRegistrationContext registrationContext)
    {
        var constructor = registrationContext.Constructors.FirstOrDefault(x =>
            Attribute.IsDefined(x, typeof(PreferredConstructorAttribute)));
        if (constructor == null)
        {
            return false;
        }
        
        AdvancedConstructorRegistrationMethod.Register(collection, registrationContext.ServiceToRegister, constructor);
        return true;
    }

    public string AlgorithmPrerequisiteAsDescribedToHuman => 
        $"Mark one constructor with attribute {nameof(PreferredConstructorAttribute)}";
}