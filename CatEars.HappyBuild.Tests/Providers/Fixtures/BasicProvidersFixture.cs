using System;
using CatEars.HappyBuild.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace CatEars.HappyBuild.Tests.Providers.Fixtures;

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