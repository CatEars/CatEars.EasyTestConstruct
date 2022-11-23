using System;
using Catears.EasyConstruct.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace Catears.EasyConstruct.Tests.Providers.Fixtures;

public class BasicProvidersFixture
{
    public IServiceProvider ServiceProvider { get; }
    
    public BasicProvidersFixture()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.RegisterBasicValueProviders();
        ServiceProvider = serviceCollection.BuildServiceProvider();
    }    
    
}