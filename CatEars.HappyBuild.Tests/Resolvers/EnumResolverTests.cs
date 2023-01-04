using System;
using System.Linq;
using CatEars.HappyBuild.Resolvers;
using CatEars.HappyBuild.Tests.Resolvers.Fixtures;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CatEars.HappyBuild.Tests.Resolvers;

public class EnumResolverTests : IClassFixture<BasicProviderFixture>
{
    private BasicProviderFixture ProviderFixture { get; }

    public EnumResolverTests(BasicProviderFixture providerFixture)
    {
        ProviderFixture = providerFixture;
    }

    private enum TestEnum
    {
        A,
        B,
        C
    }

    private enum TestEnumWithNoValue
    {
    }

    [Fact]
    public void Constructor_WithNonEnumType_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new EnumResolver(typeof(EnumResolver)));
    }

    [Fact]
    public void Constructor_WhenEnumHasNoPossibleValues_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new EnumResolver(typeof(TestEnumWithNoValue)));
    }

    [Fact]
    public void ResolveParameter_ReturnsEnumOfTypePassedToConstructor()
    {
        var expectedDomain = Enum.GetValues<TestEnum>()
            .ToHashSet();
        var enumResolver = new EnumResolver(typeof(TestEnum));

        var result = enumResolver.ResolveParameter(ProviderFixture.ServiceCollection.BuildServiceProvider());

        Assert.IsType<TestEnum>(result);
        Assert.Contains((TestEnum)result, expectedDomain);
    }
}