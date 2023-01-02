using System.Reflection;
using Catears.EasyConstruct.Resolvers;

namespace Catears.EasyConstruct.Registration;

internal record ParameterResolverBundle(
    ConstructorInfo Constructor, 
    List<IParameterResolver> ParameterResolvers);