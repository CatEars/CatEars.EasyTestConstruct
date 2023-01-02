using System;
using System.Collections.Generic;
using System.Linq;
using Catears.EasyConstruct.Registration;
using Xunit;

namespace Catears.EasyConstruct.Tests.Registrators;

public class SingleEncounterDependencyListerTests
{

    [Fact]
    public void ListDependencies_WithDisregardedTypes_ReturnsNoneOfDisregardedTypes()
    {
        var disregardedTypes = new HashSet<Type> { typeof(BasicRecord) };
        var walker = new SingleEncounterDependencyListerDecorator(
            new ConstructorParameterDependencyLister(),
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
        var walker = new SingleEncounterDependencyListerDecorator(
            new ConstructorParameterDependencyLister());

        var firstResult = walker.ListDependencies(typeof(RecordWithChainedRecord))
            .Select(x => x.ServiceToRegister);
        var secondResult = walker.ListDependencies(typeof(RecordWithInnerRecord))
            .Select(x => x.ServiceToRegister);
        var thirdResult = walker.ListDependencies(typeof(RecordWithChainedRecord))
            .Select(x => x.ServiceToRegister);

        var expectedFirst = new List<Type>() { typeof(RecordWithChainedRecord), typeof(RecordWithInnerRecord) };
        var expectedSecond = new List<Type>() { typeof(BasicRecord) };
        var expectedThird = new List<Type>();
        
        Assert.Equal(expectedFirst, firstResult.ToList());
        Assert.Equal(expectedSecond, secondResult.ToList());
        Assert.Equal(expectedThird, thirdResult.ToList());
    }
}