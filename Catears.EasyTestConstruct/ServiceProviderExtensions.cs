using Catears.EasyTestConstruct.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace Catears.EasyTestConstruct;

public static class ServiceProviderExtensions
{
    public static int AnyInt(this IServiceProvider provider) => provider.GetRequiredService<IntProvider>().AnyInt();
}