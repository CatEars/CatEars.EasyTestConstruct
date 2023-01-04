using CatEars.HappyBuild.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace CatEars.HappyBuild.Tests.Resolvers.Fixtures;

public class BasicProviderFixture
{
    public ServiceCollection ServiceCollection { get; } = new();

    public BasicProviderFixture()
    {
        ServiceCollection.RegisterBasicValueProviders();
    }
}