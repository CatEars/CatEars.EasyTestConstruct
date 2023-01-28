using System;
using Xunit;

namespace CatEars.HappyBuild.Tests;

public class BuildContextTests
{
    private record SampleRecord(string StringValue);

    public interface ITestInterface
    {
    }

    public record SampleInterfaceRecord(ITestInterface TestInterface);

    public class SampleTestInterfaceImplementation : ITestInterface {
    }

    [Fact]
    public void Register_WithBuilderFunc_RegistersSampleRecord()
    {
        var buildContext = new BuildContext();

        buildContext.Register(() => new SampleRecord(""));

        var scope = buildContext.Scope();
        Assert.Equal(new SampleRecord(""), scope.Build<SampleRecord>());
    }

    [Fact]
    public void Register_WithBuilderFuncUsingProvider_RegistersSampleRecord()
    {
        var buildContext = new BuildContext();

        buildContext.Register(_ => new SampleRecord(""));

        var scope = buildContext.Scope();
        Assert.Equal(new SampleRecord(""), scope.Build<SampleRecord>());
    }

    [Fact]
    public void Register_WithInterfaceAndImplementation_CanFetchImplementationAsInterface()
    {
        var buildContext = new BuildContext();
        
        buildContext.Register<ITestInterface, SampleTestInterfaceImplementation>();

        var scope = buildContext.Scope();
        var result = scope.Build<ITestInterface>();
        Assert.IsType<SampleTestInterfaceImplementation>(result);
    }
    
    [Fact]
    public void Register_WithInterfaceAndDefaultRegistrationOptions_ThrowsNotImplementedExceptionOnResolve()
    {
        var context = new BuildContext();
        context.Register<ITestInterface>();
        var scope = context.Scope();

        Assert.Throws<NotImplementedException>(() => scope.Build<ITestInterface>());
    }

}