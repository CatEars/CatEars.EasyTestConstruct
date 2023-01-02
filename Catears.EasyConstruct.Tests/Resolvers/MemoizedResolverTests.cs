using System;
using Catears.EasyConstruct.Resolvers;
using Catears.EasyConstruct.Tests.Resolvers.Fixtures;
using FakeItEasy;
using Xunit;

namespace Catears.EasyConstruct.Tests.Resolvers;

public class MemoizedResolverTests : IClassFixture<BasicProviderFixture>
{
    private BasicProviderFixture Fixture { get; }

    public MemoizedResolverTests(BasicProviderFixture fixture)
    {
        Fixture = fixture;
    }

    [Fact]
    public void ResolveParameter_WhenCalledOnce_ReturnsObjectConstructedByBuilder()
    {
        var obj = new object();
        object Builder(IServiceProvider _) => obj;
        var resolver = new MemoizedResolver(Builder);

        var result = resolver.ResolveParameter(A.Fake<IServiceProvider>());

        Assert.Same(obj, result);
    }

    [Fact]
    public void ResolveParameter_WhenCalledTwice_CallsBuilderOnlyOnce()
    {
        var count = 0;
        object Builder(IServiceProvider _)
        {
            ++count;
            return new object();
        }

        var resolver = new MemoizedResolver(Builder);

        resolver.ResolveParameter(A.Fake<IServiceProvider>());
        resolver.ResolveParameter(A.Fake<IServiceProvider>());
        resolver.ResolveParameter(A.Fake<IServiceProvider>());

        Assert.Equal(1, count);
    }
}