using System.Reflection;
using CatEars.HappyBuild.Annotations;
using CatEars.HappyBuild.DependencyListers;
using CatEars.HappyBuild.Resolvers;
using Microsoft.Extensions.DependencyInjection;

namespace CatEars.HappyBuild.Registration;

internal class ServiceRegistrator
{
    private ParameterResolverBundleCollection ResolverCollection { get; }
    
    private BuildContext.Options Options { get; }
    
    public ServiceRegistrator(
        ParameterResolverBundleCollection resolverCollection, 
        BuildContext.Options options)
    {
        ResolverCollection = resolverCollection;
        Options = options;
    }

    internal void RegisterServicesOrThrow(
        IServiceCollection collection,
        IDependencyLister lister,
        Type type)
    {
        var dependencies = lister.ListDependencies(type).ToList();
        foreach (var dependency in dependencies)
        {
            RegisterServiceOrThrow(collection, dependency);
        }
    }

    internal void RegisterServiceOrThrow(IServiceCollection collection, ServiceRegistrationContext context)
    {
        if (context.IsOpenGenericType)
        {
            // Services that are open and generic (like `typeof(MyTypeWithUnspecifiedParameters<>)`
            // will only be constructed if they have a specific implementation type. The object will not be built
            // with a factory method.
            collection.AddTransient(context.ServiceToRegister, context.ServiceToRegister);
            return;
        }

        if (context.IsBasicType)
        {
            return;
        }

        if (context.IsMockIntendedType)
        {
            collection.AddTransient(context.ServiceToRegister,
                _ => CreateMock(context.ServiceToRegister));
            return;
        }

        var constructorToRegister = FindAppropriateConstructorOrThrow(context);
        Register(collection, context, constructorToRegister);
    }

    private object CreateMock(Type contextServiceToRegister)
    {
        var mockFactory = Options.MockFactory;
        var methodInfo = mockFactory.GetType().GetMethod(nameof(mockFactory.CreateMock));
        if (methodInfo == null)
        {
            var message = $"Mock factory of type '{mockFactory.GetType().Name}' did not implement the MockFactory " +
                          $"interface correctly. Cannot create mocks from it";
            throw new InvalidOperationException(message);
        }
        var genericMethod = methodInfo.MakeGenericMethod(contextServiceToRegister);
        try
        {
            var resultingObject = genericMethod.Invoke(mockFactory, new object[] { Options });
            if (resultingObject == null)
            {
                var message =
                    $"Mock factory of type '{mockFactory.GetType().Name}' did not return a valid object for a mock. " +
                    $"For HappyBuild to work correctly it needs to be able to create mocks with the mock factory.";
                throw new InvalidOperationException(message);
            }

            return resultingObject;
        }
        catch (TargetInvocationException ex) when (ex.InnerException != null)
        {
            throw ex.InnerException;
        }
    }

    internal static ConstructorInfo FindAppropriateConstructorOrThrow(ServiceRegistrationContext context)
    {
        // Most user-defined objects in C# will have a constructor. For most classes and records this is automatically generated.
        // However, for static classes and structs they are not. In those cases you should not be able to register
        // without a builder function so we do not allow automatic service registration for types without a constructor.
        // Only 1+ constructors.
        if (context.Constructors.Length == 1)
        {
            return context.Constructors.First();
        }

        bool IsPreferredConstructor(ConstructorInfo info) =>
            Attribute.IsDefined(info, typeof(HappyBuildConstructorAttribute));

        var markedConstructor = context.Constructors.FirstOrDefault(IsPreferredConstructor);
        if (markedConstructor != null)
        {
            return markedConstructor;
        }

        var msg = $"Constructor for type {context.ServiceToRegister.Name} did not contain exactly 1 constructor, " +
                  $"and it did not contain a constructor marked as {nameof(HappyBuildConstructorAttribute)} and can " +
                  $"therefore not be registered for automatically constructing";
        throw new ArgumentException(msg);
    }

    private void Register(IServiceCollection serviceCollection, ServiceRegistrationContext context,
        ConstructorInfo constructor)
    {
        var parameterDescriptors = constructor.GetParameters();
        var sortedByPosition = parameterDescriptors.OrderBy(paramInfo => paramInfo.Position);
        var parameterResolvers = sortedByPosition
            .Select(ParameterResolverCollection.GetResolverForType)
            .ToList();

        serviceCollection.AddTransient(context.ServiceToRegister, services =>
        {
            var parameters = parameterResolvers.Select(resolver => resolver.ResolveParameter(services));
            return constructor.Invoke(parameters.ToArray());
        });
        ResolverCollection.BundleMap.Add(context.ServiceToRegister, new ParameterResolverBundle(
            constructor, parameterResolvers));
    }
}