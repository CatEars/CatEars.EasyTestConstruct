using System.Reflection;
using Catears.EasyConstruct.Resolvers;

namespace Catears.EasyConstruct.ResolverFactories;

internal record AdvancedResolver(Predicate<ParameterInfo> Predicate, Func<ParameterInfo, IParameterResolver> Builder);