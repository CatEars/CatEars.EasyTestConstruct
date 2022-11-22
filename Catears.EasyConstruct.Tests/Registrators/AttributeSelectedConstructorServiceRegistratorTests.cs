using System;
using System.Linq;
using Catears.EasyConstruct.Registrators;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Catears.EasyConstruct.Tests.Registrators;

public class AttributeSelectedConstructorServiceRegistratorTests
{
    [Theory]
    [TestsAllAvailableClassesForRegistration]
    [InlineData(typeof(ClassThatIsStatic), false)]
    [InlineData(typeof(ClassWithSingleConstructor), false)]
    [InlineData(typeof(ClassThatIsAbstract), false)]
    [InlineData(typeof(ClassThatIsSealed), false)]
    [InlineData(typeof(ClassWithSingleMarkedConstructor), true)]
    [InlineData(typeof(ClassWithMultipleConstructors), false)]
    [InlineData(typeof(ClassWithSingleMarkedConstructorAmongMultipleConstructors), true)]
    [InlineData(typeof(RecordWithSingleConstructor), false)]
    [InlineData(typeof(RecordThatIsAbstract), false)]
    [InlineData(typeof(RecordThatIsSealed), false)]
    [InlineData(typeof(RecordWithSingleMarkedConstructor), true)]
    [InlineData(typeof(RecordWithMultipleConstructors), false)]
    [InlineData(typeof(RecordWithSingleMarkedConstructorAmongMultipleConstructors), true)]
    [InlineData(typeof(StructWithNoConstructor), false)]
    [InlineData(typeof(StructWithSingleConstructor), false)]
    [InlineData(typeof(StructWithSingleMarkedConstructor), true)]
    [InlineData(typeof(StructWithMultipleConstructors), false)]
    [InlineData(typeof(StructWithSingleMarkedConstructorAmongMultipleConstructors), true)]
    public void TryRegisterService_WithType_RegistersWhenAttributeMarkedConstructor(Type type, bool shouldSucceed)
    {
        var serviceCollection = new ServiceCollection();
        var sut = new AttributeSelectedConstructorServiceRegistrator();

        var didSucceed = sut.TryRegisterService(serviceCollection, ServiceRegistrationContext.FromType(type));

        var expectedRegisteredServices = shouldSucceed ? 1 : 0;
        Assert.Equal(shouldSucceed, didSucceed);
        Assert.Equal(expectedRegisteredServices, serviceCollection.Count);
        Assert.True(serviceCollection.All(descriptor => descriptor.ImplementationFactory != null || descriptor.ImplementationInstance != null));
    }
}