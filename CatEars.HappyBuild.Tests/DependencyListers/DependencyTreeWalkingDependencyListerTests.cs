using System;
using System.Collections.Generic;
using System.Linq;
using CatEars.HappyBuild.DependencyListers;
using Xunit;

namespace CatEars.HappyBuild.Tests.DependencyListers;

public class DependencyTreeWalkingDependencyListerTests
{

    [Fact]
    public void ListDependencies_WithDisregardedTypes_ReturnsNoneOfDisregardedTypes()
    {
        var disregardedTypes = new HashSet<Type> { typeof(BasicRecord) };
        var walker = new DependencyTreeWalkingDependencyLister(
            disregardedTypes);

        var result = walker.ListDependencies(typeof(RecordWithInnerRecord));

        var expected = new List<Type>() { typeof(RecordWithInnerRecord) };
        var registrations = result.Select(x => x.ServiceToRegister).ToList();
        Assert.Single(registrations);
        Assert.Equal(expected, registrations);
        Assert.DoesNotContain(disregardedTypes, type => registrations.Contains(type));
    }

    [Fact]
    public void ListDependencies_WhenCalledWithMultiplePartsOfComplexTypeChain_ReturnsTypesOnlyOnce()
    {
        var walker = new DependencyTreeWalkingDependencyLister();

        var firstResult = walker.ListDependencies(typeof(RecordWithChainedRecord))
            .Select(x => x.ServiceToRegister);
        var secondResult = walker.ListDependencies(typeof(RecordWithInnerRecord))
            .Select(x => x.ServiceToRegister);

        var expectedFirst = new List<Type>() { typeof(RecordWithChainedRecord), typeof(RecordWithInnerRecord), typeof(BasicRecord) };
        var expectedSecond = new List<Type>();
        
        Assert.Equal(expectedFirst, firstResult.ToList());
        Assert.Equal(expectedSecond, secondResult.ToList());
    }
    
    [Theory]
    [InlineData(typeof(BasicRecord), typeof(BasicRecord))]
    [InlineData(typeof(RecordWithInnerRecord), typeof(RecordWithInnerRecord), typeof(BasicRecord))]
    [InlineData(typeof(SelfReferentialClass), typeof(SelfReferentialClass))]
    [InlineData(typeof(int))]
    [InlineData(typeof(string))]
    public void ListDependencies_WithType_ReturnsExpectedListOfTypes(Type rootType, params Type[] expectedTypes)
    {
        var walker = new DependencyTreeWalkingDependencyLister();

        var result = walker.ListDependencies(rootType);

        Assert.Equal(expectedTypes, result.Select(x => x.ServiceToRegister).ToArray());
    }
}