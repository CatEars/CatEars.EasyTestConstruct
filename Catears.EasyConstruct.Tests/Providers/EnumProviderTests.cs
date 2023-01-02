using System;
using System.Collections.Generic;
using System.Linq;
using Catears.EasyConstruct.Providers;
using Xunit;

namespace Catears.EasyConstruct.Tests.Providers;

internal enum TestableEnum
{
    A,
    B,
    C,
    D,
    E
}

public class EnumProviderTests
{

    [Fact]
    public void RandomEnum_WhenCalled1000Times_ProducesEachEnumAtLeastOnce()
    {
        const int iterations = 1000;
        var expectedEnums = Enum.GetValues<TestableEnum>().ToHashSet();
        var generatedEnums = new HashSet<TestableEnum>();
        var sut = new EnumProvider();

        foreach (var _ in Enumerable.Range(0, iterations))
        {
            generatedEnums.Add((TestableEnum)sut.RandomEnum<TestableEnum>());
        }

        Assert.Equal(expectedEnums, generatedEnums);
        Assert.True(generatedEnums.Count > 0);
    }

    [Fact]
    public void RandomEnum_WhenCalledWithNonEnumType_ThrowsAssertionError()
    {
        var sut = new EnumProvider();

        Assert.Throws<ArgumentException>(() => sut.RandomEnum(typeof(EnumProvider)));
    }

}