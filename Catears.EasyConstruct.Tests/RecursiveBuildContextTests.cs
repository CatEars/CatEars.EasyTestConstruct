using System;
using Catears.EasyConstruct.FakeItEasy;
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
        var buildContext = new BuildContext(BuildContext.Options.Default with
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
    public void RegisterDependencyTree_WithInterfaceRegistrar_CallsUserDefinedMockRegistrationMethod()
    {
        Type? wasCalledWithType = null;
        var context = new BuildContext(BuildContext.Options.Default with
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
    public void RegisterDependencyTree_WithClassPrimitiveAndInterface_IsAbleToResolveClass()
    {
        var buildContext = new BuildContext(BuildContext.Options.Default with
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
    public void RegisterDependencyTree_WithFakedInterface_CanMockBehavior()
    {
        throw new NotImplementedException();
    }
}