using System.Collections.Generic;
using System.Linq;
using Catears.EasyConstruct.Providers;
using Xunit;

namespace Catears.EasyConstruct.Tests.Providers;

public class StringProviderTests
{

    [Fact]
    public void RandomString_WhenCalled1000Times_Generates1000UniqueStrings()
    {
        const int iterations = 1000;
        var uniqueStrings = new HashSet<string>();
        var sut = new StringProvider();

        foreach (var _ in Enumerable.Range(0, iterations))
        {
            uniqueStrings.Add(sut.RandomString());
        }

        Assert.Equal(iterations, uniqueStrings.Count);
    }

    [Fact]
    public void RandomString_WhenCalledWithVariableNameAndTypeName_IncludesThoseInGeneratedString()
    {
        const string myVariableName = "MyVariableName";
        const string myTypeName = "MyTypeName";
        var sut = new StringProvider();

        var result = sut.RandomString(StringProviderOptions.Default with
        {
            VariableName = myVariableName,
            VariableType = myTypeName
        });

        Assert.Contains(myVariableName, result);
        Assert.Contains(myTypeName, result);
    }

}