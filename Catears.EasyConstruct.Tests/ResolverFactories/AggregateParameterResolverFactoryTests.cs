using System;
using System.Linq;
using Catears.EasyConstruct.ResolverFactories;
using Xunit;

namespace Catears.EasyConstruct.Tests.ResolverFactories;

public class AggregateParameterResolverFactoryTests
{
    private record SampleRecord(string StringValue);

    [Fact]
    public void CreateParameterResolverOrThrow_WithNoMatchingFactories_ThrowsInvalidOperationException()
    {
        var resolverFactory = new AggregateParameterResolverFactory();
        var sampleParameter = typeof(SampleRecord).GetConstructors().First().GetParameters().First();

        Assert.Throws<InvalidOperationException>(() => resolverFactory.CreateParameterResolverOrThrow(sampleParameter));
    }
}