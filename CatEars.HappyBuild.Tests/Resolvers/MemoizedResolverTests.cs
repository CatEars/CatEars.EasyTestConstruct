using System;
using CatEars.HappyBuild.Resolvers;
using CatEars.HappyBuild.Tests.Resolvers.Fixtures;
using FakeItEasy;
using Xunit;

namespace CatEars.HappyBuild.Tests.Resolvers;

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
        var resolver = new MemoizedResolver(Builder, typeof(object));

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

        var resolver = new MemoizedResolver(Builder, typeof(object));

        resolver.ResolveParameter(A.Fake<IServiceProvider>());
        resolver.ResolveParameter(A.Fake<IServiceProvider>());
        resolver.ResolveParameter(A.Fake<IServiceProvider>());

        Assert.Equal(1, count);
    }
}