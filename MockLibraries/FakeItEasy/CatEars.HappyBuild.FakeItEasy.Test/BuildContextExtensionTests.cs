using Xunit;

namespace CatEars.HappyBuild.FakeItEasy.Test;

public class BuildContextExtensionTests
{
    public interface ITestInterface {}
    
    [Fact]
    public void RegisterFake_WithInterface_RegistersAMockedType()
    {
        var context = new BuildContext();
        context.RegisterFake<ITestInterface>();

        var resolved = context.Scope().Resolve<ITestInterface>();
        
        Assert.IsAssignableFrom<ITestInterface>(resolved);
    }

}