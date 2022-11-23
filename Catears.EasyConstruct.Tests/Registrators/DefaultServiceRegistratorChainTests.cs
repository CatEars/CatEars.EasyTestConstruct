using System;
using System.Linq;
using Catears.EasyConstruct.Providers;
using Catears.EasyConstruct.Registrators;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Catears.EasyConstruct.Tests.Registrators;

public class DefaultServiceRegistratorChainTests
{
    [Theory]
    [TestsAllAvailableClassesForRegistration]
    [InlineData(typeof(ClassThatIsStatic), false)]
    [InlineData(typeof(ClassWithSingleConstructor), true)]
    [InlineData(typeof(ClassThatIsAbstract), false)]
    [InlineData(typeof(ClassThatIsSealed), true)]
    [InlineData(typeof(ClassWithSingleMarkedConstructor), true)]
    [InlineData(typeof(ClassWithMultipleConstructors), false)]
    [InlineData(typeof(ClassWithSingleMarkedConstructorAmongMultipleConstructors), true)]
    [InlineData(typeof(ClassWithSingleConstructorWithMultiplePrimitiveParameters), true)]
    [InlineData(typeof(ClassWithSingleConstructorContainingComplexParameter), true)]
    [InlineData(typeof(RecordWithSingleConstructor), true)]
    [InlineData(typeof(RecordThatIsAbstract), false)]
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
        var serviceCollection = new ServiceCollection();
        var sut = DefaultServiceRegistratorChain.FirstLink;

        var registrationContext = ServiceRegistrationContext.FromType(type);
        if (shouldSucceed)
        {
            var didSucceed = sut.TryRegisterService(serviceCollection, registrationContext);

            var expectedRegisteredServices = 1;
            Assert.Equal(shouldSucceed, didSucceed);
            Assert.Equal(expectedRegisteredServices, serviceCollection.Count);
            Assert.True(serviceCollection.All(descriptor => descriptor.ImplementationFactory != null || descriptor.ImplementationInstance != null));
        }
        else
        {
            Assert.Throws<ArgumentException>(() => sut.TryRegisterService(serviceCollection, registrationContext));
        }
    }

    [Theory]
    [TestsAllAvailableClassesForRegistration]
    [InlineData(typeof(ClassThatIsStatic), false)]
    [InlineData(typeof(ClassWithSingleConstructor), true)]
    [InlineData(typeof(ClassThatIsAbstract), false)]
    [InlineData(typeof(ClassThatIsSealed), true)]
    [InlineData(typeof(ClassWithSingleMarkedConstructor), true)]
    [InlineData(typeof(ClassWithMultipleConstructors), false)]
    [InlineData(typeof(ClassWithSingleMarkedConstructorAmongMultipleConstructors), true)]
    [InlineData(typeof(ClassWithSingleConstructorWithMultiplePrimitiveParameters), true)]
    [InlineData(typeof(ClassWithSingleConstructorContainingComplexParameter), true, typeof(RecordWithSingleConstructor))]
    [InlineData(typeof(RecordWithSingleConstructor), true)]
    [InlineData(typeof(RecordThatIsAbstract), false)]
    [InlineData(typeof(RecordThatIsSealed), true)]
    [InlineData(typeof(RecordWithSingleMarkedConstructor), true)]
    [InlineData(typeof(RecordWithMultipleConstructors), false)]
    [InlineData(typeof(RecordWithSingleMarkedConstructorAmongMultipleConstructors), true)]
    [InlineData(typeof(RecordWithSingleConstructorWithMultiplePrimitiveParameters), true)]
    [InlineData(typeof(RecordWithSingleConstructorContainingComplexParameter), true, typeof(RecordWithSingleConstructor))]
    [InlineData(typeof(StructWithNoConstructor), false)]
    [InlineData(typeof(StructWithSingleConstructor), true)]
    [InlineData(typeof(StructWithSingleMarkedConstructor), true)]
    [InlineData(typeof(StructWithMultipleConstructors), false)]
    [InlineData(typeof(StructWithSingleMarkedConstructorAmongMultipleConstructors), true)]
    [InlineData(typeof(StructWithSingleConstructorWithMultiplePrimitiveParameters), true)]
    [InlineData(typeof(StructWithSingleConstructorContainingComplexParameter), true, typeof(RecordWithSingleConstructor))]
    public void
        GetRequiredService_WhenRegisteredUsingDefaultServiceRegistratorChain_CanConstructClassesPossibleToConstruct(
            Type type, bool shouldSucceed, params Type[] extraTypesToRegister)
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.RegisterBasicValueProviders();
        var sut = DefaultServiceRegistratorChain.FirstLink;

        foreach (var extraType in extraTypesToRegister)
        {
            var context = ServiceRegistrationContext.FromType(extraType);
            sut.TryRegisterService(serviceCollection, context);
        }
        
        var registrationContext = ServiceRegistrationContext.FromType(type);
        if (shouldSucceed)
        {
            var didSucceed = sut.TryRegisterService(serviceCollection, registrationContext);
            using var provider = serviceCollection.BuildServiceProvider();
            var resolvedObject = provider.GetService(type);
            
            Assert.True(didSucceed);
            Assert.NotNull(resolvedObject);
            Assert.IsType(type, resolvedObject);
        }
        else
        {
            Assert.Throws<ArgumentException>(() => sut.TryRegisterService(serviceCollection, registrationContext));
        }
    }
}