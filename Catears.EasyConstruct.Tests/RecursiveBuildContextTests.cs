using System;
using Catears.EasyConstruct.Extensions;
using Catears.EasyConstruct.FakeItEasy;
using FakeItEasy;
using Xunit;

namespace Catears.EasyConstruct.Tests;

public class RecursiveBuildContextTests
{
    internal record InnerRecord(int Value);

    internal record OuterRecord(InnerRecord Inner);

    public interface SampleInterface
    {
        string GetValue();
    }
    
    public class SampleInterfaceImpl : SampleInterface
    {
        internal static readonly string Value = "Sample Value";
        
        public string GetValue() => Value;
    }

    internal class SampleInterfaceWrapper
    {
        internal SampleInterface WrappedInterface { get; } 
            
        public SampleInterfaceWrapper(SampleInterface sampleInterface)
        {
            WrappedInterface = sampleInterface;
        }
        
    }

    internal record ComplexRecord(InnerRecord Inner, SampleInterface SampleInterface, string Value);

    [Fact]
    public void RecursiveRegister_WithComplexRecord_IsAbleToResolveRecordWithHierarchy()
    {
        var buildContext = new BuildContext(new()
        {
            RegistrationMode = RegistrationMode.Recursive
        });
        buildContext.Register<OuterRecord>();
        var scope = buildContext.Scope();

        var outer = scope.Resolve<OuterRecord>();

        Assert.NotNull(outer);
        Assert.NotNull(outer.Inner);
        Assert.NotEqual(0, outer.Inner.Value);
    }

    [Fact]
    public void RecursiveRegister_WithInterfaceRegistrar_CallsUserDefinedMockRegistrationMethod()
    {
        Type? wasCalledWithType = null;
        var context = new BuildContext(new()
        {
            RegistrationMode = RegistrationMode.Recursive,
            MockRegistrationMethod = (_, type) =>
            {
                wasCalledWithType = type;
            }
        });
        context.Register<SampleInterfaceWrapper>();
        
        Assert.Equal(typeof(SampleInterface), wasCalledWithType);
    }
    
 
    [Fact]
    public void RecursiveRegister_WithClassPrimitiveAndInterface_IsAbleToResolveClass()
    {
        var buildContext = new BuildContext(new()
        {
            RegistrationMode = RegistrationMode.Recursive,
            MockRegistrationMethod = BuildContextExtensions.RegisterFake
        });
        buildContext.Register<ComplexRecord>();
        var scope = buildContext.Scope();
        var complex = scope.Resolve<ComplexRecord>();
        
        Assert.NotNull(complex);
        Assert.NotNull(complex.Inner);
        Assert.NotNull(complex.SampleInterface);
        Assert.NotNull(complex.Value);
    }

    [Fact]
    public void RecursiveRegister_WithFakedInterface_CanMockBehavior()
    {
        var buildContext = new BuildContext(new()
        {
            RegistrationMode = RegistrationMode.Recursive,
            MockRegistrationMethod = BuildContextExtensions.RegisterFake
        });
        buildContext.Register<ComplexRecord>();
        var scope = buildContext.Scope();
        var mock = scope.MemoizeAndResolve<SampleInterface>();
        A.CallTo(() => mock.GetValue()).Returns("42");
        var resolved = scope.Resolve<ComplexRecord>();

        var result = resolved.SampleInterface.GetValue();
        
        Assert.Equal("42", result);
    }
}