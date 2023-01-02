using System;
using Catears.EasyConstruct.FakeItEasy;
using Xunit;

namespace Catears.EasyConstruct.Tests;

public class BuildContextTests
{
    private record SampleRecord(string StringValue);

    public interface ITestInterface
    {
    }

    public record SampleInterfaceRecord(ITestInterface TestInterface);

    [Fact]
    public void Register_WithBuilderFunc_RegistersSampleRecord()
    {
        var buildContext = new BuildContext();

        buildContext.Register(() => new SampleRecord(""));

        var scope = buildContext.Scope();
        Assert.Equal(new SampleRecord(""), scope.Resolve<SampleRecord>());
    }

    [Fact]
    public void Register_WithBuilderFuncUsingProvider_RegistersSampleRecord()
    {
        var buildContext = new BuildContext();

        buildContext.Register(_ => new SampleRecord(""));

        var scope = buildContext.Scope();
        Assert.Equal(new SampleRecord(""), scope.Resolve<SampleRecord>());
    }

    [Fact]
    public void Register_WithInterfaceAndDefaultRegistrationOptions_ThrowsArgumentException()
    {
        var context = new BuildContext();

        Assert.Throws<ArgumentException>(() => context.Register<ITestInterface>());
    }

    [Fact]
    public void RegisterFake_WithInterface_RegistersAMockedType()
    {
        var context = new BuildContext();
        context.RegisterFake(typeof(ITestInterface));

        var resolved = context.Scope().Resolve<ITestInterface>();
        
        Assert.IsAssignableFrom<ITestInterface>(resolved);
    }
}