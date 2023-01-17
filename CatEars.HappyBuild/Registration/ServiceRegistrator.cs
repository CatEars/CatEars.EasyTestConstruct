﻿using System.Reflection;
using CatEars.HappyBuild.Annotations;
using CatEars.HappyBuild.DependencyListers;
using CatEars.HappyBuild.Resolvers;
using Microsoft.Extensions.DependencyInjection;

namespace CatEars.HappyBuild.Registration;

internal class ServiceRegistrator
{
    private ParameterResolverBundleCollection ResolverCollection { get; }
    
    private Func<Type, object>? MockFactoryMethod { get; }

    public ServiceRegistrator(Func<Type, object>? mockFactoryMethod, 
        ParameterResolverBundleCollection resolverCollection)
    {
        MockFactoryMethod = mockFactoryMethod;
        ResolverCollection = resolverCollection;
    }

    internal void RegisterServicesOrThrow(
        IServiceCollection collection,
        IDependencyLister lister,
        Type type)
    {
        foreach (var dependency in lister.ListDependencies(type))
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
            if (MockFactoryMethod == null)
            {
                var msg = $"Trying to register abstract type or interface '{context.ServiceToRegister.Name}' without any " +
                          $"defined function that handles such types. Add a `{nameof(BuildContext.Options.MockFactoryMethod)}` " +
                          "when creating your build context to register mocks for these kinds of types when they " +
                          "are encountered.";
                throw new ArgumentException(msg);
            }
            collection.AddTransient(context.ServiceToRegister,
                _ => MockFactoryMethod(context.ServiceToRegister));
            return;
        }

        var constructorToRegister = FindAppropriateConstructorOrThrow(context);
        Register(collection, context, constructorToRegister);
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