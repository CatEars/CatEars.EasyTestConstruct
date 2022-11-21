using System.Linq;
using JungleAnimals.Food;
using JungleAnimals.Test.Fixtures;
using Xunit;

namespace JungleAnimals.Test;

public class MemoizeTests : IClassFixture<BuildContextFixture>
{
    private BuildContextFixture Fixture { get; }

    public MemoizeTests(BuildContextFixture fixture)
    {
        Fixture = fixture;
    }

    [Fact]
    public void Memoize_WhenRunAHundredTimes_AlwaysReturnsSameObject()
    {
        using var scope = Fixture.Context.Scope();
        scope.Memoize<Banana>();

        var firstBanana = scope.Resolve<Banana>();
        foreach (var _ in Enumerable.Range(0, 100))
        {
            var nextBanana = scope.Resolve<Banana>();
            Assert.Same(firstBanana, nextBanana);
        }
    }
}