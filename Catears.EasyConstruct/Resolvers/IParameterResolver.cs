namespace Catears.EasyConstruct.Resolvers;

internal interface IParameterResolver
{
    object ResolveParameter(IServiceProvider provider);
}