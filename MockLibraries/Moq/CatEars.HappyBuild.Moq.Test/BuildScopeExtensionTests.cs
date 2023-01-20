using System;
using Moq;
using Xunit;

namespace CatEars.HappyBuild.Moq.Test;

public class BuildScopeExtensionTests
{
    public interface ITestInterface
    {
        string GetValue();
    }
    
    [Fact]
    public void Resolve_WithInterface_CanRetrieveAndConfigureMock()
    {
        var scope = Happy.Build.AutoScope();
        var iface = scope.Resolve<ITestInterface>();
        var randomString = Guid.NewGuid().ToString();

        var mock = scope.MockFor<ITestInterface>();
        mock.Setup(t => t.GetValue())
            .Returns(randomString);
        
        Assert.Equal(randomString, iface.GetValue());       
    }
}