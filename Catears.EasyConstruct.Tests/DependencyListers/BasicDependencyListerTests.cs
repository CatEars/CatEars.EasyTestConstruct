using System;
using System.Linq;
using Catears.EasyConstruct.DependencyListers;
using Xunit;

namespace Catears.EasyConstruct.Tests.DependencyListers;

public class BasicDependencyListerTests
{
    [Theory]
    [InlineData(typeof(BasicRecord), typeof(BasicRecord))]
    [InlineData(typeof(RecordWithInnerRecord), typeof(RecordWithInnerRecord))]
    [InlineData(typeof(SelfReferentialClass), typeof(SelfReferentialClass))]
    public void ListDependencies_WithType_ReturnsExpectedListOfTypes(Type rootType, params Type[] expectedTypes)
    {
        var walker = new BasicDependencyLister();

        var result = walker.ListDependencies(rootType);

        Assert.Equal(expectedTypes, result.Select(x => x.ServiceToRegister).ToArray());
    }
}