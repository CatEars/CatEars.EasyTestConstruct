using System;
using System.Collections.Generic;
using System.Linq;
using Catears.EasyConstruct.Providers;
using Xunit;

namespace Catears.EasyConstruct.Tests.Providers;

public class PrimitiveProviderTests
{
    public interface IProviderTester
    {
        bool GeneratesUniqueValues();
    }

    private class ProviderTester<TReturnType> : IProviderTester
    {
        private Func<TReturnType> Generator { get; }

        public ProviderTester(Func<TReturnType> generator)
        {
            Generator = generator;
        }

        private int GetUniqueNumberOfGeneratedValues()
        {
            return GenerateALotOf(Generator).Count;
        }

        public bool GeneratesUniqueValues()
        {
            return GetUniqueNumberOfGeneratedValues() > 50;
        }
    }

    public static IEnumerable<object[]> PrimitiveTesters = new List<object[]>()
    {
        new object[] { new ProviderTester<byte>(() => new PrimitiveValueProvider().RandomByte()) },
        new object[] { new ProviderTester<char>(() => new PrimitiveValueProvider().RandomChar()) },
        new object[] { new ProviderTester<sbyte>(() => new PrimitiveValueProvider().RandomSByte()) },
        new object[] { new ProviderTester<decimal>(() => new PrimitiveValueProvider().RandomDecimal()) },
        new object[] { new ProviderTester<double>(() => new PrimitiveValueProvider().RandomDouble()) },
        new object[] { new ProviderTester<float>(() => new PrimitiveValueProvider().RandomFloat()) },
        new object[] { new ProviderTester<int>(() => new PrimitiveValueProvider().RandomInt()) },
        new object[] { new ProviderTester<long>(() => new PrimitiveValueProvider().RandomLong()) },
        new object[] { new ProviderTester<sbyte>(() => new PrimitiveValueProvider().RandomSByte()) },
        new object[] { new ProviderTester<short>(() => new PrimitiveValueProvider().RandomShort()) },
        new object[] { new ProviderTester<ushort>(() => new PrimitiveValueProvider().RandomUShort()) },
        new object[] { new ProviderTester<uint>(() => new PrimitiveValueProvider().RandomUInt()) },
        new object[] { new ProviderTester<ulong>(() => new PrimitiveValueProvider().RandomULong()) },
    };

    [Theory]
    [MemberData(nameof(PrimitiveTesters))]
    public void PrimitiveTester_CanGenerateValues(IProviderTester tester)
    {
        Assert.True(tester.GeneratesUniqueValues());
    }

    [Fact]
    public void BoolProvider_CanProvideAllBools()
    {
        var provider = new PrimitiveValueProvider();
        var differentBools = GenerateALotOf(() => provider.RandomBool());
        Assert.Equal(2, differentBools.Count);
    }

    private static HashSet<T> GenerateALotOf<T>(Func<T> generator)
    {
        var differentValues = new HashSet<T>();
        foreach (var _ in Enumerable.Range(0, 100))
        {
            differentValues.Add(generator());
        }

        return differentValues;
    }
}