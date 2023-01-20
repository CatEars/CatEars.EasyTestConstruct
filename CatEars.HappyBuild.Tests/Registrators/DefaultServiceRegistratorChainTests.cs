﻿using System;
using System.Linq;
using CatEars.HappyBuild.Providers;
using CatEars.HappyBuild.Registration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CatEars.HappyBuild.Tests.Registrators;

public class DefaultServiceRegistratorChainTests
{

    [Theory]
    [TestsAllAvailableClassesForRegistration]
    [InlineData(typeof(ClassThatIsStatic), false)]
    [InlineData(typeof(ClassWithSingleConstructor), true)]
    [InlineData(typeof(ClassThatIsAbstract), true)]
    [InlineData(typeof(ClassThatIsSealed), true)]
    [InlineData(typeof(ClassWithSingleMarkedConstructor), true)]
    [InlineData(typeof(ClassWithMultipleConstructors), false)]
    [InlineData(typeof(ClassWithSingleMarkedConstructorAmongMultipleConstructors), true)]
    [InlineData(typeof(ClassWithSingleConstructorWithMultiplePrimitiveParameters), true)]
    [InlineData(typeof(ClassWithSingleConstructorContainingComplexParameter), true)]
    [InlineData(typeof(RecordWithSingleConstructor), true)]
    [InlineData(typeof(RecordThatIsAbstract), true)]
    [InlineData(typeof(RecordThatIsSealed), true)]
    [InlineData(typeof(RecordWithSingleMarkedConstructor), true)]
    [InlineData(typeof(RecordWithMultipleConstructors), false)]
    [InlineData(typeof(RecordWithSingleMarkedConstructorAmongMultipleConstructors), true)]
    [InlineData(typeof(RecordWithSingleConstructorWithMultiplePrimitiveParameters), true)]
    [InlineData(typeof(RecordWithSingleConstructorContainingComplexParameter), true)]
    [InlineData(typeof(StructWithNoConstructor), false)]
    [InlineData(typeof(StructWithSingleConstructor), true)]
    [InlineData(typeof(StructWithSingleMarkedConstructor), true)]
    [InlineData(typeof(StructWithMultipleConstructors), false)]
    [InlineData(typeof(StructWithSingleMarkedConstructorAmongMultipleConstructors), true)]
    [InlineData(typeof(StructWithSingleConstructorWithMultiplePrimitiveParameters), true)]
    [InlineData(typeof(StructWithSingleConstructorContainingComplexParameter), true)]
    public void TryRegisterService_WithType_RegistersUnlessImpossibleToConstruct(Type type, bool shouldSucceed)
    {
        var registrator = new ServiceRegistrator(ParameterResolverBundleCollection.Empty, BuildContext.Options.Default);
        var serviceCollection = new ServiceCollection();

        var registrationContext = ServiceRegistrationContext.FromType(type);
        if (shouldSucceed)
        {
            registrator.RegisterServiceOrThrow(serviceCollection, registrationContext);

            var expectedRegisteredServices = 1;
            Assert.Equal(expectedRegisteredServices, serviceCollection.Count);
            Assert.True(serviceCollection.All(descriptor => descriptor.ImplementationFactory != null || descriptor.ImplementationInstance != null));
        }
        else
        {
            Assert.Throws<ArgumentException>(() => registrator.RegisterServiceOrThrow(serviceCollection, registrationContext));
        }
    }

    [Theory]
    [TestsAllAvailableClassesForRegistration]
    [InlineData(typeof(ClassThatIsStatic), false)]
    [InlineData(typeof(ClassWithSingleConstructor), true)]
    [InlineData(typeof(ClassThatIsAbstract), true, true)]
    [InlineData(typeof(ClassThatIsSealed), true)]
    [InlineData(typeof(ClassWithSingleMarkedConstructor), true)]
    [InlineData(typeof(ClassWithMultipleConstructors), false)]
    [InlineData(typeof(ClassWithSingleMarkedConstructorAmongMultipleConstructors), true)]
    [InlineData(typeof(ClassWithSingleConstructorWithMultiplePrimitiveParameters), true)]
    [InlineData(typeof(ClassWithSingleConstructorContainingComplexParameter), true, false, typeof(RecordWithSingleConstructor))]
    [InlineData(typeof(RecordWithSingleConstructor), true)]
    [InlineData(typeof(RecordThatIsAbstract), true, true)]
    [InlineData(typeof(RecordThatIsSealed), true)]
    [InlineData(typeof(RecordWithSingleMarkedConstructor), true)]
    [InlineData(typeof(RecordWithMultipleConstructors), false)]
    [InlineData(typeof(RecordWithSingleMarkedConstructorAmongMultipleConstructors), true)]
    [InlineData(typeof(RecordWithSingleConstructorWithMultiplePrimitiveParameters), true)]
    [InlineData(typeof(RecordWithSingleConstructorContainingComplexParameter), true, false, typeof(RecordWithSingleConstructor))]
    [InlineData(typeof(StructWithNoConstructor), false)]
    [InlineData(typeof(StructWithSingleConstructor), true)]
    [InlineData(typeof(StructWithSingleMarkedConstructor), true)]
    [InlineData(typeof(StructWithMultipleConstructors), false)]
    [InlineData(typeof(StructWithSingleMarkedConstructorAmongMultipleConstructors), true)]
    [InlineData(typeof(StructWithSingleConstructorWithMultiplePrimitiveParameters), true)]
    [InlineData(typeof(StructWithSingleConstructorContainingComplexParameter), true, false, typeof(RecordWithSingleConstructor))]
    public void
        GetRequiredService_WhenDefaultRegistered_CanConstructElseNot(
            Type type, bool shouldSucceed, bool throwsOnResolving = false, params Type[] extraTypesToRegister)
    {
        var registrator = new ServiceRegistrator(ParameterResolverBundleCollection.Empty, BuildContext.Options.Default);
        var serviceCollection = new ServiceCollection();
        serviceCollection.RegisterBasicValueProviders();

        foreach (var extraType in extraTypesToRegister)
        {
            var context = ServiceRegistrationContext.FromType(extraType);
            registrator.RegisterServiceOrThrow(serviceCollection, context);
        }

        var registrationContext = ServiceRegistrationContext.FromType(type);
        if (shouldSucceed)
        {
            registrator.RegisterServiceOrThrow(serviceCollection, registrationContext);
            using var provider = serviceCollection.BuildServiceProvider();
            if (throwsOnResolving)
            {
                Assert.Throws<NotImplementedException>(() => provider.GetService(type));
                return;
            }
            var resolvedObject = provider.GetService(type);

            Assert.NotNull(resolvedObject);
            Assert.IsType(type, resolvedObject);
        }
        else
        {
            Assert.Throws<ArgumentException>(() => registrator.RegisterServiceOrThrow(serviceCollection, registrationContext));
        }
    }
}