
using System.Reflection;
using CatEars.HappyBuild.Registration;

namespace CatEars.HappyBuild.DependencyListers;

internal interface DependencyTreeDecisionPoint
{
    ServiceRegistrationContext GetCurrentType();
    
    void Dive(Predicate<ServiceRegistrationContext> shouldIncludeType);

    bool AtEnd();
}

internal class DependencyTreeWalker
{
    private Queue<ServiceRegistrationContext> DiscoveredTypeQueue { get; }
    
    public DependencyTreeWalker(Type rootType)
    {
        DiscoveredTypeQueue = new Queue<ServiceRegistrationContext>();
        DiscoveredTypeQueue.Enqueue(ServiceRegistrationContext.FromType(rootType));
    }

    private class DecisionPoint : DependencyTreeDecisionPoint
    {
        private ServiceRegistrationContext DiscoveredType { get; }
        private Queue<ServiceRegistrationContext> CurrentTypeQueue { get; }
        
        internal DecisionPoint(ServiceRegistrationContext discoveredType, Queue<ServiceRegistrationContext> currentTypeQueue)
        {
            DiscoveredType = discoveredType;
            CurrentTypeQueue = currentTypeQueue;
        }
        
        public ServiceRegistrationContext GetCurrentType()
        {
            return DiscoveredType;
        }

        public void Dive(Predicate<ServiceRegistrationContext> shouldIncludeType)
        {
            var constructorInfo = ServiceRegistrator.FindAppropriateConstructorOrThrow(DiscoveredType);
            var typesToVisit = ListConstructorParameterTypes(constructorInfo)
                .Select(ServiceRegistrationContext.FromType)
                .Where(service => !service.IsBasicType && shouldIncludeType(service));
            foreach (var typeToVisit in typesToVisit)
            {
                CurrentTypeQueue.Enqueue(typeToVisit);
            }
        }

        private static IEnumerable<Type> ListConstructorParameterTypes(ConstructorInfo constructorInfo)
        {
            var constructorParams = constructorInfo.GetParameters();
            foreach (var param in constructorParams)
            {
                yield return param.ParameterType;
            }
        }

        public bool AtEnd()
        {
            return CurrentTypeQueue.Count == 0;
        }
    }
    
    internal DependencyTreeDecisionPoint PopNextDependency()
    {
        if (DiscoveredTypeQueue.Count == 0)
        {
            throw new InvalidOperationException(
                "Cannot pop a new dependency when there are no more dependencies to be discovered");
        }

        var nextTypeToDiscover = DiscoveredTypeQueue.Dequeue();
        return new DecisionPoint(nextTypeToDiscover, DiscoveredTypeQueue);
    }
    
}