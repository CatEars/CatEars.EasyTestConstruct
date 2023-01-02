using System;
using System.Linq;
using Catears.EasyConstruct.DependencyListers;
using Xunit;

namespace Catears.EasyConstruct.Tests.DependencyListers;

public class ConstructorParameterDependencyListerTests
{
    [Theory]
    [InlineData(typeof(BasicRecord), typeof(BasicRecord))]
    [InlineData(typeof(RecordWithInnerRecord), typeof(RecordWithInnerRecord), typeof(BasicRecord))]
    [InlineData(typeof(SelfReferentialClass), typeof(SelfReferentialClass))]
    [InlineData(typeof(int))]
    [InlineData(typeof(string))]
    public void ListDependencies_WithType_ReturnsExpectedListOfTypes(Type rootType, params Type[] expectedTypes)
    {
        var walker = new ConstructorParameterDependencyLister();

        var result = walker.ListDependencies(rootType);

        Assert.Equal(expectedTypes, result.Select(x => x.ServiceToRegister).ToArray());
    }
}