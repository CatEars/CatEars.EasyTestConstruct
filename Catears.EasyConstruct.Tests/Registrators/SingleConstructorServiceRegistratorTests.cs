using System;
using System.Linq;
using Catears.EasyConstruct.Registrators;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Catears.EasyConstruct.Tests.Registrators;

public class SingleConstructorServiceRegistratorTests
{

    [Theory]
    [TestsAllAvailableClassesForRegistration]
    [InlineData(typeof(ClassThatIsStatic), false)]
    [InlineData(typeof(ClassWithSingleConstructor), true)]
    [InlineData(typeof(ClassThatIsAbstract), false)]
    [InlineData(typeof(ClassThatIsSealed), true)]
    [InlineData(typeof(ClassWithSingleMarkedConstructor), true)]
    [InlineData(typeof(ClassWithMultipleConstructors), false)]
    [InlineData(typeof(ClassWithSingleMarkedConstructorAmongMultipleConstructors), false)]
    [InlineData(typeof(ClassWithSingleConstructorWithMultiplePrimitiveParameters), true)]
    [InlineData(typeof(ClassWithSingleConstructorContainingComplexParameter), true)]
    [InlineData(typeof(RecordWithSingleConstructor), true)]
    [InlineData(typeof(RecordThatIsAbstract), false)]
    [InlineData(typeof(RecordThatIsSealed), true)]
    [InlineData(typeof(RecordWithSingleMarkedConstructor), true)]
    [InlineData(typeof(RecordWithMultipleConstructors), false)]
    [InlineData(typeof(RecordWithSingleMarkedConstructorAmongMultipleConstructors), false)]
    [InlineData(typeof(RecordWithSingleConstructorWithMultiplePrimitiveParameters), true)]
    [InlineData(typeof(RecordWithSingleConstructorContainingComplexParameter), true)]
    [InlineData(typeof(StructWithNoConstructor), false)]
    [InlineData(typeof(StructWithSingleConstructor), true)]
    [InlineData(typeof(StructWithSingleMarkedConstructor), true)]
    [InlineData(typeof(StructWithMultipleConstructors), false)]
    [InlineData(typeof(StructWithSingleMarkedConstructorAmongMultipleConstructors), false)]
    [InlineData(typeof(StructWithSingleConstructorWithMultiplePrimitiveParameters), true)]
    [InlineData(typeof(StructWithSingleConstructorContainingComplexParameter), true)]
    public void TryRegisterService_WithType_RegistersWhenSingleConstructor(Type type, bool shouldSucceed)
    {
        var serviceCollection = new ServiceCollection();
        var sut = new SingleConstructorServiceRegistrator();

        var didSucceed = sut.TryRegisterService(serviceCollection, ServiceRegistrationContext.FromType(type));

        var expectedRegisteredServices = shouldSucceed ? 1 : 0;
        Assert.Equal(shouldSucceed, didSucceed);
        Assert.Equal(expectedRegisteredServices, serviceCollection.Count);
        Assert.True(serviceCollection.All(descriptor => descriptor.ImplementationFactory != null || descriptor.ImplementationInstance != null));
    }
    
}