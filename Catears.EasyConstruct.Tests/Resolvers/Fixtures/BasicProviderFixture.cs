using Catears.EasyConstruct.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace Catears.EasyConstruct.Tests.Resolvers.Fixtures;

public class BasicProviderFixture
{
    public ServiceCollection ServiceCollection { get; } = new();

    public BasicProviderFixture()
    {
        ServiceCollection.RegisterBasicValueProviders();
    }
}