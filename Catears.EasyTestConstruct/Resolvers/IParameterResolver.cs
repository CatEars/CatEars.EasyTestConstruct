namespace Catears.EasyTestConstruct.Resolvers;

internal interface IParameterResolver
{
    object ResolveParameter(IServiceProvider provider);
}