using System;
using System.Linq;
using Catears.EasyConstruct.Providers;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Catears.EasyConstruct.Tests.Providers;

public class ProvidersConsistencyChecks
{

    [Fact]
    public void AllClassesMarkedBasicValueProviderAreRegistered()
    {
        var sampleProvider = typeof(IntProvider);
        var allProvidersInProviderNamespace = sampleProvider
            .Assembly
            .GetTypes()
            .Where(type => Attribute.IsDefined(type, typeof(BasicValueProviderAttribute)))
            .ToHashSet();
        var serviceCollection = new ServiceCollection();
        
        serviceCollection.RegisterBasicValueProviders();
        
        Assert.Equal(allProvidersInProviderNamespace.Count, serviceCollection.Count);
        var actualProviders = serviceCollection
            .Select(x => x.ServiceType)
            .ToHashSet();
        Assert.Equal(allProvidersInProviderNamespace, actualProviders);
        Assert.True(actualProviders.Count > 0);
    }

    [Fact]
    public void AllClassesInProvidersNamespaceEndingWithProviderAreMarkedWithBasicValueProviderAttribute()
    {
        var sampleProvider = typeof(IntProvider);
        var allProvidersInProviderNamespace = sampleProvider
            .Assembly
            .GetTypes()
            .Where(type => type.Namespace == sampleProvider.Namespace && type.Name.EndsWith("Provider"))
            .ToHashSet();
        
        var allMarkedProviders = sampleProvider
            .Assembly
            .GetTypes()
            .Where(type => Attribute.IsDefined(type, typeof(BasicValueProviderAttribute)))
            .ToHashSet();
        
        Assert.Equal(allMarkedProviders, allProvidersInProviderNamespace);
    }
    
}