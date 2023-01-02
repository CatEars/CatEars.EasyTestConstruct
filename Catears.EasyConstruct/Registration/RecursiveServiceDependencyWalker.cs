namespace Catears.EasyConstruct.Registration;

internal class RecursiveServiceDependencyWalker : IServiceDependencyWalker
{
    private Type DependencyTreeRootType { get; }

    private ISet<Type> TypesToDisregard { get; set; }

    public RecursiveServiceDependencyWalker(Type dependencyTreeRootType)
    {
        DependencyTreeRootType = dependencyTreeRootType;
        TypesToDisregard = new HashSet<Type>();
    }

    public IEnumerable<ServiceRegistrationContext> ListDependencies()
    {
        var encounteredTypes = new HashSet<Type>() { DependencyTreeRootType };
        var foundTypes = new List<ServiceRegistrationContext>()
        {
            ServiceRegistrationContext.FromType(DependencyTreeRootType)
        };

        for (var idx = 0; idx < foundTypes.Count; ++idx)
        {
            var typeToInspect = foundTypes[idx];
            if (typeToInspect.IsMockIntendedType)
            {
                continue;
            }

            var matchingConstructor = ServiceRegistrator.FindAppropriateConstructorOrThrow(typeToInspect);
            var constructorParams = matchingConstructor.GetParameters();
            foreach (var param in constructorParams)
            {
                if (encounteredTypes.Contains(param.ParameterType) ||
                    TypesToDisregard.Contains(param.ParameterType))
                {
                    // Skip inline instead of with LINQ expression
                    // in case one class contains same complex type of parameter twice
                    // e.g. public record MyRecord(MyInnerRecord A, MyInnerRecord B)
                    continue;
                }

                var registrationContext = ServiceRegistrationContext.FromType(param.ParameterType);
                if (registrationContext.IsBasicType)
                {
                    continue;
                }

                encounteredTypes.Add(param.ParameterType);
                foundTypes.Add(registrationContext);
            }
        }

        return foundTypes;
    }

    internal void DisregardTypes(ISet<Type> typesToDisregard)
    {
        TypesToDisregard = typesToDisregard;
    }
}