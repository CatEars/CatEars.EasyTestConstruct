using System;
using Xunit;

namespace CatEars.HappyBuild.Moq.Test;

public class BuildScopeExtensionTests
{
    public interface ITestInterface
    {
        string GetValue();
    }
    
    [Fact]
    public void Resolve_WithInterface_CanRetrieveAndConfigureMockAfterResolveWasCalled()
    {
        var scope = Happy.Build.AutoScope();
        var iface = scope.Build<ITestInterface>();
        var randomString = Guid.NewGuid().ToString();

        var mock = scope.MockFor<ITestInterface>();
        mock.Setup(t => t.GetValue())
            .Returns(randomString);
        
        Assert.Equal(randomString, iface.GetValue());       
    }

    [Fact]
    public void MockFor_WhenCalledBeforeResolve_CreatesNewMockThatCanBeResolved()
    {
        var scope = Happy.Build.AutoScope();
        var randomString = Guid.NewGuid().ToString();
        
        scope
            .MockFor<ITestInterface>()
            .Setup(t => t.GetValue())
            .Returns(randomString);

        var iface = scope.Build<ITestInterface>();
        Assert.Equal(randomString, iface.GetValue());
    }
}