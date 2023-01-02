using System.Collections;

namespace Catears.EasyConstruct.Registration;

internal class RecursiveServiceDependencyWalker : IServiceDependencyWalker
{
    private Type DependencyTreeRootType { get; }
 
    public RecursiveServiceDependencyWalker(Type dependencyTreeRootType)
    {
        DependencyTreeRootType = dependencyTreeRootType;
    }
    
    public IEnumerator<ServiceRegistrationContext> GetEnumerator()
    {
        var encounteredTypes = new HashSet<Type>() { DependencyTreeRootType };
        var foundTypes = new List<ServiceRegistrationContext>()
        {
            ServiceRegistrationContext.FromType(DependencyTreeRootType)
        };
        
        for (var idx = 0; idx < foundTypes.Count; ++idx)
        {
            var typeToInspect = foundTypes[idx];
            if (typeToInspect.IsPrimitiveType || typeToInspect.IsMockIntendedType)
            {
                continue;
            }
            var matchingConstructor = ServiceRegistrator.FindAppropriateConstructorOrThrow(typeToInspect);
            var constructorParams = matchingConstructor.GetParameters();
            foreach (var param in constructorParams)
            {
                if (encounteredTypes.Contains(param.ParameterType))
                {
                    // Skip inline instead of with LINQ expression
                    // in case one class contains same complex type of parameter twice
                    // e.g. public record MyRecord(MyInnerRecord A, MyInnerRecord B)
                    continue;
                }

                encounteredTypes.Add(param.ParameterType);
                foundTypes.Add(ServiceRegistrationContext.FromType(param.ParameterType));
            }
        }

        return foundTypes.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}