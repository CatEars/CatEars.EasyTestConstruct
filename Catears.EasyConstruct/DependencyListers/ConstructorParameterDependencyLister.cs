using Catears.EasyConstruct.Registration;

namespace Catears.EasyConstruct.DependencyListers;

internal class ConstructorParameterDependencyLister : IDependencyLister
{
    public IEnumerable<ServiceRegistrationContext> ListDependencies(Type rootDependency)
    {
        var rootContext = ServiceRegistrationContext.FromType(rootDependency);
        if (rootContext.IsBasicType)
        {
            return Enumerable.Empty<ServiceRegistrationContext>();
        }
        else if (rootContext.IsMockIntendedType)
        {
            return new List<ServiceRegistrationContext>() { rootContext };
        }

        var foundTypes = new List<ServiceRegistrationContext>() { rootContext };
        var encounteredTypes = new HashSet<Type>() { rootDependency };
        var matchingConstructor = ServiceRegistrator.FindAppropriateConstructorOrThrow(rootContext);
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

            var registrationContext = ServiceRegistrationContext.FromType(param.ParameterType);
            if (registrationContext.IsBasicType)
            {
                continue;
            }

            encounteredTypes.Add(param.ParameterType);
            foundTypes.Add(registrationContext);
        }

        return foundTypes;
    }
}