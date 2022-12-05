using System;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Catears.EasyConstruct.Tests;

public class BuildScopeTests
{
    private record SampleRecord(string StringValue);

    [Fact]
    public void Memoize_WithTypeParameter_MemoizesThatClass()
    {
        var scope = CreateSampleBuildScope();
        scope.Memoize<SampleRecord>();

        var firstResult = scope.Resolve<SampleRecord>();
        var secondResult = scope.Resolve<SampleRecord>();
        
        Assert.Same(firstResult, secondResult);
    }

    [Fact]
    public void Memoize_CalledMultipleTimes_WillStillMemoize()
    {
        var scope = CreateSampleBuildScope();
        scope.Memoize<SampleRecord>();
        scope.Memoize<SampleRecord>();
        scope.Memoize<SampleRecord>();
        scope.Memoize<SampleRecord>();
        scope.Memoize<SampleRecord>();

        var firstResult = scope.Resolve<SampleRecord>();
        var secondResult = scope.Resolve<SampleRecord>();
        
        Assert.Same(firstResult, secondResult);
    }

    [Fact]
    public void Use_CalledWithBasicBuilderFunction_UsesBasicBuilderFunction()
    {
        var scope = CreateSampleBuildScope();
        var count = 0;
        scope.Use(() =>
        {
            ++count;
            return new object();
        });

        scope.Resolve<object>();
        
        Assert.Equal(1, count);
    }

    [Fact]
    public void Memoize_WhenUsingSingleton_ResolvesSingletonObject()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton(new object());
        var buildScope = new BuildScope(serviceCollection);
        buildScope.Memoize<object>();
        
        var firstResult = buildScope.Resolve<object>();
        var secondResult = buildScope.Resolve<object>();
        
        Assert.NotNull(firstResult);
        Assert.Same(firstResult, secondResult);
    }

    private abstract class SampleAbstractClass
    {
    }

    [Fact]
    public void Memoize_WhenPoorlyConfigured_ThrowsInvalidOperationException()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton<SampleAbstractClass>();
        var buildScope = new BuildScope(serviceCollection);

        Assert.Throws<InvalidOperationException>(() => buildScope.Memoize<SampleAbstractClass>());
    }
    
    private static BuildScope CreateSampleBuildScope()
    {
        var buildContext = new BuildContext();
        buildContext.Register<SampleRecord>();
        return buildContext.Scope();
    }
}