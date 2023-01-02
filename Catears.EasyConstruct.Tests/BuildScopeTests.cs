using System;
using Catears.EasyConstruct.Registration;
using FakeItEasy.Sdk;
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
        var buildScope = new BuildScope(serviceCollection, ParameterResolverBundleCollection.Empty);
        buildScope.Memoize<object>();

        var firstResult = buildScope.Resolve<object>();
        var secondResult = buildScope.Resolve<object>();

        Assert.NotNull(firstResult);
        Assert.Same(firstResult, secondResult);
    }

    private abstract class SampleAbstractClass
    {
        internal abstract string GetValue();
    }

    [Fact]
    public void Memoize_WhenPoorlyConfigured_ThrowsInvalidOperationException()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton<SampleAbstractClass>();
        var buildScope = new BuildScope(serviceCollection, ParameterResolverBundleCollection.Empty);

        Assert.Throws<InvalidOperationException>(() => buildScope.Memoize<SampleAbstractClass>());
    }

    [Fact]
    public void BindParameterFor_WithStringParameter_ResolveReturnsBoundParameter()
    {
        var scope = CreateSampleBuildScope()
            .BindParameterFor<SampleRecord, string>("42");
        var result = scope.Resolve<SampleRecord>();

        Assert.Equal("42", result.StringValue);
    }

    private class ImplementedAbstractClass : SampleAbstractClass
    {
        private string Value { get; }
        public ImplementedAbstractClass(string ReturnedValue)
        {
            Value = ReturnedValue;
        }
        internal override string GetValue() => Value;
    }

    private record RecordUsingAbstractClass(string A, SampleAbstractClass InnerClass, int B);

    [Fact]
    public void BindParameterFor_WithImplementationOfAbstractClass_ReturnsBoundParameter()
    {
        var context = new BuildContext();
        context.Register<RecordUsingAbstractClass>();
        var scope = context.Scope();
        
        var result = scope.BindParameterFor<RecordUsingAbstractClass, SampleAbstractClass>(
            new ImplementedAbstractClass("42"))
            .Resolve<RecordUsingAbstractClass>();
        
        Assert.Equal("42", result.InnerClass.GetValue());
    }
    
    private static BuildScope CreateSampleBuildScope()
    {
        var buildContext = new BuildContext();
        buildContext.Register<SampleRecord>();
        return buildContext.Scope();
    }
}