using CatEars.HappyBuild.Extensions;
using CatEars.HappyBuild.Providers;
using CatEars.HappyBuild.Tests.Providers.Fixtures;
using Xunit;

namespace CatEars.HappyBuild.Tests.Providers;

public class ServiceProviderExtensionsTests : IClassFixture<BasicProvidersFixture>
{
    private BasicProvidersFixture Fixture { get; }

    private enum TestEnum
    {
        A, B, C
    }

    public ServiceProviderExtensionsTests(BasicProvidersFixture fixture)
    {
        Fixture = fixture;
    }

    [Fact]
    public void RandomInt_ReturnsAnInt()
    {
        var randomInt = Fixture.ServiceProvider.RandomInt();

        Assert.IsType<int>(randomInt);
    }

    [Fact]
    public void RandomEnum_ReturnsAnEnum()
    {
        var randomEnum = Fixture.ServiceProvider.RandomEnum(typeof(TestEnum));
        Assert.IsType<TestEnum>(randomEnum);
    }

    [Fact]
    public void RandomEnum_WithTypeParameter_ReturnsAnEnum()
    {
        var randomEnum = Fixture.ServiceProvider.RandomEnum<TestEnum>();
        Assert.IsType<TestEnum>(randomEnum);
    }

    [Fact]
    public void RandomString_ReturnsAString()
    {
        var randomString = Fixture.ServiceProvider.RandomString();
        Assert.IsType<string>(randomString);
    }

    [Fact]
    public void RandomString_WithParameters_ReturnsAStringMatchingOptions()
    {
        var randomString = Fixture.ServiceProvider.RandomString(new StringProviderOptions("AAAA", "BBBB"));
        Assert.Contains("AAAA", randomString);
        Assert.Contains("BBBB", randomString);
    }
}
