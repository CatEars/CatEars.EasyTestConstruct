using Catears.EasyConstruct.Resolvers;
using Catears.EasyConstruct.Tests.Resolvers.Fixtures;
using FakeItEasy;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Catears.EasyConstruct.Tests.Resolvers;

public class IntResolverTests : IClassFixture<BasicProviderFixture>
{
    private BasicProviderFixture Fixture { get; }
    
    public IntResolverTests(BasicProviderFixture fixture)
    {
        Fixture = fixture;
    }

    [Fact]
    public void ResolveParameter_ReturnsInt()
    {
        var sut = new IntResolver();
        using var scope = Fixture.ServiceCollection.BuildServiceProvider();

        var result = sut.ResolveParameter(scope);

        Assert.IsType<int>(result);
    }

}