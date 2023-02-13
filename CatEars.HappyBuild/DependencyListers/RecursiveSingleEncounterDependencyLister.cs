using CatEars.HappyBuild.Registration;

namespace CatEars.HappyBuild.DependencyListers;

internal class RecursiveSingleEncounterDependencyLister : IDependencyLister
{
    private ISet<Type> EncounteredTypes { get; }

    public RecursiveSingleEncounterDependencyLister(ISet<Type>? initiallyEncounteredTypes = null)
    {
        EncounteredTypes = new HashSet<Type>(initiallyEncounteredTypes ?? new HashSet<Type>());
    }
    
    public IEnumerable<ServiceRegistrationContext> ListDependencies(Type rootType)
    {
        if (EncounteredTypes.Contains(rootType))
        {
            yield break;
        }

        var walker = new DependencyTreeWalker(rootType);
        DependencyTreeDecisionPoint decision;
        do
        {
            decision = walker.PopNextDependency();
            var type = decision.GetCurrentType();
            if (type.IsBasicType || EncounteredTypes.Contains(type.ServiceToRegister))
            {
                continue;
            }
            
            EncounteredTypes.Add(type.ServiceToRegister);
            yield return type;
            if (type.IsMockIntendedType)
            {
                continue;
            }
            // Only dive on complex types, not on mocked types, or primitives
            decision.Dive(HasYetToEncounterType);
        } while (!decision.AtEnd());
    }

    private bool HasYetToEncounterType(ServiceRegistrationContext context) => !EncounteredTypes.Contains(context.ServiceToRegister);

    public bool HasEncounteredType(Type type) => EncounteredTypes.Contains(type);
}