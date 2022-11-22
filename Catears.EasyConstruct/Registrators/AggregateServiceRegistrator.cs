﻿using Microsoft.Extensions.DependencyInjection;

namespace Catears.EasyConstruct.Registrators;

internal class AggregateServiceRegistrator : IServiceRegistrator
{
    private List<IServiceRegistrator> Registrators { get; }

    public AggregateServiceRegistrator(List<IServiceRegistrator>? registrators = null)
    {
        registrators ??= new List<IServiceRegistrator>();
        Registrators = registrators;
    }

    public bool TryRegisterService(IServiceCollection collection, ServiceRegistrationContext serviceToRegister)
    {
        foreach (var registrator in Registrators)
        {
            if (registrator.TryRegisterService(collection, serviceToRegister))
            {
                return true;
            }
        }
        
        return ThrowNoMatchingServiceRegistratorFound(serviceToRegister);
    }

    private bool ThrowNoMatchingServiceRegistratorFound(ServiceRegistrationContext serviceToRegister)
    {
        var message = $"Could not find any suitable constructor for type {serviceToRegister.ServiceToRegister.Name}. " +
                      AlgorithmPrerequisiteAsDescribedToHuman;
        throw new ArgumentException(message);
    }

    public string AlgorithmPrerequisiteAsDescribedToHuman => GeneratePrerequisiteDescription();

    private string GeneratePrerequisiteDescription()
    {
        var descriptions = Registrators.Select(x => x.AlgorithmPrerequisiteAsDescribedToHuman);
        var fullDescription = string.Join(", ", descriptions);
        var message = $"Try one of: {fullDescription}";
        return message;
    }
}