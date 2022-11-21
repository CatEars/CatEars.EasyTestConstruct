using JungleAnimals.Test.Fixtures;
using Xunit;

namespace JungleAnimals.Test;

public record MyTestRecord(string Value, int Stuff);

public class RecordTests : IClassFixture<BuildContextFixture>
{
    private BuildContextFixture Fixture { get; }

    public RecordTests(BuildContextFixture fixture)
    {
        Fixture = fixture;
        Fixture.Context.Register<MyTestRecord>();
    }

    [Fact]
    public void CSharpRecord_WhenResolved_ReturnsInstanceOfRecord()
    {
        using var scope = Fixture.Context.Scope();
        var record = scope.Resolve<MyTestRecord>();

        Assert.IsType<MyTestRecord>(record);
    }
}