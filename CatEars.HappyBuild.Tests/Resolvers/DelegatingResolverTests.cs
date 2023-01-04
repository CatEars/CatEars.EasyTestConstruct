using System;
using CatEars.HappyBuild.Resolvers;
using FakeItEasy;
using Xunit;

namespace CatEars.HappyBuild.Tests.Resolvers;

public class DelegatingResolverTests
{
    private record MySpecialType();

    [Fact]
    public void ResolveParameter_ResolvesUsingGivenServiceProvider()
    {
        var provider = A.Fake<IServiceProvider>();
        var myServiceInstance = new MySpecialType();
        A.CallTo(() => provider.GetService(typeof(MySpecialType)))
            .Returns(myServiceInstance);
        var sut = new DelegatingResolver(typeof(MySpecialType));

        var result = sut.ResolveParameter(provider);

        Assert.Same(myServiceInstance, result);
        A.CallTo(() => provider.GetService(typeof(MySpecialType)))
            .MustHaveHappened();
    }
}