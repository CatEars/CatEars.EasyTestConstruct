using System.Reflection;
using CatEars.HappyBuild.Resolvers;

namespace CatEars.HappyBuild.Registration;

internal record ParameterResolverBundle(
    ConstructorInfo Constructor, 
    List<IParameterResolver> ParameterResolvers);