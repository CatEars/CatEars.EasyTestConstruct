using System.Reflection;
using CatEars.HappyBuild.Registration;

namespace CatEars.HappyBuild.DependencyListers;

internal class DependencyTreeWalkingDependencyLister : IDependencyLister
{
    private ISet<Type> EncounteredTypes { get; }

    public DependencyTreeWalkingDependencyLister(ISet<Type>? initiallyEncounteredTypes = null)
    {
        EncounteredTypes = initiallyEncounteredTypes == null
            ? new HashSet<Type>()
            : new HashSet<Type>(initiallyEncounteredTypes);
    }

    public IEnumerable<ServiceRegistrationContext> ListDependencies(Type rootType)
    {
        if (EncounteredTypes.Contains(rootType))
        {
            yield break;
        }

        var typeQueue = new List<ServiceRegistrationContext>()
        {
            ServiceRegistrationContext.FromType(rootType)
        };

        for (var idx = 0; idx < typeQueue.Count; ++idx)
        {
            var currentType = typeQueue[idx];
            if (currentType.IsBasicType || EncounteredTypes.Contains(currentType.ServiceToRegister))
            {
                continue;
            }

            EncounteredTypes.Add(currentType.ServiceToRegister);
            yield return currentType;
            if (currentType.IsMockIntendedType)
            {
                continue;
            }

            var constructorInfo = ServiceRegistrator.FindAppropriateConstructorOrThrow(currentType);
            var typesToVisit = ListConstructorParameterTypes(constructorInfo)
                .Select(ServiceRegistrationContext.FromType)
                .Where(service => !service.IsBasicType && !EncounteredTypes.Contains(service.ServiceToRegister));
            typeQueue.AddRange(typesToVisit);
        }
    }

    public bool HasEncounteredType(Type type) => EncounteredTypes.Contains(type);

    private static IEnumerable<Type> ListConstructorParameterTypes(ConstructorInfo constructorInfo)
        => constructorInfo.GetParameters().Select(x => x.ParameterType);
}