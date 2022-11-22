using System.Collections.Generic;
using System.Linq;
using Catears.EasyConstruct.Providers;
using Xunit;

namespace Catears.EasyConstruct.Tests.Providers;

public class IntProviderTests
{
    [Fact]
    public void RandomInt_WhenIterating1000Times_GeneratesAtLeast100UniqueIntegers()
    {
        const int iterations = 1000;
        const int minimumExpectedUniqueInts = 100;
        var uniqueIds = new HashSet<int>();
        var sut = new IntProvider();

        foreach (var _ in Enumerable.Range(0, iterations))
        {
            uniqueIds.Add(sut.RandomInt());
        }
        
        Assert.True(uniqueIds.Count > minimumExpectedUniqueInts);
    }

    [Fact]
    public void RandomInt_WhenSuppliedWithLowAndHighParameters_GeneratesNumbersInThatRange()
    {
        const int iterations = 1000;
        const int low = 1;
        const int high = 5;
        var expectedResults = Enumerable.Range(low, high - low).ToHashSet();
        var actualResults = new HashSet<int>();
        var sut = new IntProvider();

        foreach (var _ in Enumerable.Range(0, iterations))
        {
            actualResults.Add(sut.RandomInt(low, high));
        }
        
        Assert.Equal(expectedResults, actualResults);
    }
}