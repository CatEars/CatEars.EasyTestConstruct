using CatEars.HappyBuild.Providers;
using CatEars.HappyBuild.Resolvers;
using CatEars.HappyBuild.Tests.Resolvers.Fixtures;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CatEars.HappyBuild.Tests.Resolvers;

public class StringResolverTests : IClassFixture<BasicProviderFixture>
{
    private BasicProviderFixture Fixture { get; }

    public StringResolverTests(BasicProviderFixture fixture)
    {
        Fixture = fixture;
    }

    [Fact]
    public void ResolveParameter_WithNoConfiguration_GeneratesString()
    {
        var sut = new StringResolver(StringProviderOptions.Default);
        using var scope = Fixture.ServiceCollection.BuildServiceProvider();

        var result = sut.ResolveParameter(scope);

        Assert.IsType<string>(result);
        Assert.False(string.IsNullOrWhiteSpace((string)result));
    }

    [Fact]
    public void ResolveParameter_WithVariableAndTypeName_GeneratesStringContainingVariableAndTypeName()
    {
        var sut = new StringResolver(StringProviderOptions.Default with
        {
            VariableName = "MyVariableName",
            VariableType = "MyVariableType"
        });
        using var scope = Fixture.ServiceCollection.BuildServiceProvider();

        var result = sut.ResolveParameter(scope);

        Assert.IsType<string>(result);
        var str = (string)result;
        Assert.Contains("MyVariableName", str);
        Assert.Contains("MyVariableType", str);
    }

}