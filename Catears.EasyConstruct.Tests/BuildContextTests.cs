using Xunit;

namespace Catears.EasyConstruct.Tests;

public class BuildContextTests
{

    private record SampleRecord(string StringValue);
    
    [Fact]
    public void Register_WithBuilderFunc_RegistersSampleRecord()
    {
        var buildContext = new BuildContext();
        
        buildContext.Register(() => new SampleRecord(""));

        using var scope = buildContext.Scope();
        Assert.Equal(new SampleRecord(""), scope.Resolve<SampleRecord>());
    }
    
    [Fact]
    public void Register_WithBuilderFuncUsingProvider_RegistersSampleRecord()
    {
        var buildContext = new BuildContext();
        
        buildContext.Register(_ => new SampleRecord(""));

        using var scope = buildContext.Scope();
        Assert.Equal(new SampleRecord(""), scope.Resolve<SampleRecord>());
    }
    
}