namespace CatEars.HappyBuild.Resolvers;

internal interface IParameterResolver
{
    object ResolveParameter(IServiceProvider provider);

    bool Provides(Type type);
}