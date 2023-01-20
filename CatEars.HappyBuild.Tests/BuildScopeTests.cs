using System;
using CatEars.HappyBuild.Registration;
using CatEars.HappyBuild.Scopes;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CatEars.HappyBuild.Tests;

public class BuildScopeTests
{
    private record SampleRecord(string StringValue);

    private static readonly BuildContext.Options ControlledRegistrationOptions = new()
    {
        RegistrationMode = RegistrationMode.Controlled
    };
    
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
        var buildScope = new ControlledBuildScope(serviceCollection, ParameterResolverBundleCollection.Empty);
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
        var buildScope = new ControlledBuildScope(serviceCollection, ParameterResolverBundleCollection.Empty);

        Assert.Throws<InvalidOperationException>(() => buildScope.Memoize<SampleAbstractClass>());
    }

    [Fact]
    public void BindParameterOfType_WithStringParameter_ResolveReturnsBoundParameter()
    {
        var scope = CreateSampleBuildScope()
            .BindParameter<SampleRecord, string>("42");
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
    public void BindParameter_WithImplementationOfAbstractClass_ReturnsBoundParameter()
    {
        var context = new BuildContext(ControlledRegistrationOptions);
        context.Register<RecordUsingAbstractClass>();
        var scope = context.Scope();
        
        var result = scope.BindParameter<RecordUsingAbstractClass, SampleAbstractClass>(
            new ImplementedAbstractClass("42"))
            .Resolve<RecordUsingAbstractClass>();
        
        Assert.Equal("42", result.InnerClass.GetValue());
    }
    
    private record RecordWithManyStringsAndInts(string A, int X, int Y, int Z, string B, string C);
    
    [Fact]
    public void BindNthParameterOfType_WhenBindingThirdString_BindsThatString()
    {
        var context = new BuildContext(ControlledRegistrationOptions);
        context.Register<RecordWithManyStringsAndInts>();
        var scope = context.Scope();

        var result = scope
            .BindNthParameterOfType<RecordWithManyStringsAndInts, string>("42", 2)
            .Resolve<RecordWithManyStringsAndInts>();
        
        Assert.NotEqual("42", result.A);
        Assert.NotEqual("42", result.B);
        Assert.Equal("42", result.C);
    }

    private record RecordWithDifferentParameters(int A, double B, string C);

    [Fact]
    public void BindNthParameter_WhenBindingSecondParameter_BindsSecondParameter()
    {
        var buildContext = new BuildContext(ControlledRegistrationOptions);
        buildContext.Register<RecordWithDifferentParameters>();
        var scope = buildContext.Scope();

        var result = scope
            .BindNthParameter<RecordWithDifferentParameters, double>(42.42, 1)
            .Resolve<RecordWithDifferentParameters>();
        
        Assert.NotEqual(42.42, result.A);
        Assert.Equal(42.42, result.B);
        Assert.NotEqual("42.42", result.C);
    }
    
    private static BuildScope CreateSampleBuildScope()
    {
        var buildContext = new BuildContext(ControlledRegistrationOptions);
        buildContext.Register<SampleRecord>();
        return buildContext.Scope();
    }
}