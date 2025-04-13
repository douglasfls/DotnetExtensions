using System.Linq.Expressions;
using System.Runtime.InteropServices;
using FluentAssertions;

namespace DotnetExtensions.Test;

public class StringTests
{
    [Theory]
    [InlineData(null, true)]
    [InlineData("", true)]
    [InlineData(" ", true)]
    [InlineData("abc", false)]
    public void TestIsNullOrWhiteSpace(string value, bool expectedResult)
        => value.IsNullOrWhiteSpace().Should().Be(expectedResult);

    [Theory]
    [InlineData(null, false)]
    [InlineData("", false)]
    [InlineData(" ", false)]
    [InlineData("abc", true)]
    public void TestIsNotNullOrWhiteSpace(string value, bool expectedResult)
        => value.IsNotNullOrEmpty().Should().Be(expectedResult);

    [Theory]
    [InlineData("Some", "String", "Foo", true)]
    [InlineData("Var", "Sume", "S", true)]
    [InlineData("String", "Baz", "Foo", false)]
    public void TestStartsWithAny(string v1, string v2, string v3, bool expectedResult)
        => "SomeString".StartsWithAny(v1, v2, v3).Should().Be(expectedResult);

    [Theory]
    [InlineData("Some", "String", "Foo", true)]
    [InlineData("Var", "Sume", "S", false)]
    [InlineData("String", "Baz", "Foo", true)]
    public void TestEndsWithAny(string v1, string v2, string v3, bool expectedResult)
        => "SomeString".EndsWithAny(v1, v2, v3).Should().Be(expectedResult);

    [Theory]
    [InlineData("Some", "String", "Foo", true)]
    [InlineData("Var", "Sume", "S", true)]
    [InlineData("String", "Baz", "Foo", true)]
    [InlineData("Wait", "What", "Ehin?", false)]
    public void TestContainsAny(string v1, string v2, string v3, bool expectedResult)
        => "SomeString".ContainsAny(v1, v2, v3).Should().Be(expectedResult);

    [Theory]
    [InlineData("Some", "Some", 0)]
    [InlineData("Some", "Somo", 0.95)]
    [InlineData("Some", "Saco", 3)]
    public void TestLevenshteinDistance(string v1, string v2, int expectedResult)
        => v1.CalculateDistance(v2).Should().Be(expectedResult);

    [Theory]
    [InlineData("Some", "Some", 100)]
    [InlineData("Some", "Somo", 75)]
    [InlineData("Some", "Saco", 25)]
    [InlineData("Douglas", "Douuglas", 75)]
    public void TestWithAproximation(string v1, string v2, double expectedResult)
        => v1.WithAproximation(v2, expectedResult).Should().BeTrue();

    [Theory]
    [InlineData("aaaaaa", "a", "b", "bbbbbb")]
    [InlineData("aaaaaa", "aa", "baa", "baabaabaa")]
    public void TestReplaceAll(string v1, string v2, string v3, string expectedResult)
        => v1.ReplaceAll(v2, v3).Should().Be(expectedResult);
}

public class WhereIfTests
{
    private static Func<int, bool> IsEven => i => i % 2 == 0;
    private static Expression<Func<int, bool>> IsEvenExpr => i => i % 2 == 0;

    [Theory]
    [InlineData(1, 10, true, 5)]
    [InlineData(1, 1, true, 0)]
    [InlineData(1, 10, false, 10)]
    [InlineData(1, 1, false, 1)]
    public void GetEven_WhereIfShould_QueryIfConditionIsSet(int value, int rangeSize, bool condition, int expected)
    {
        Enumerable.Range(1, rangeSize)
            .WhereIf(condition, IsEven)
            .Should()
            .HaveCount(expected);
    }

    [Theory]
    [InlineData(1, 10, true, 5)]
    [InlineData(1, 1, true, 0)]
    [InlineData(1, 10, false, 10)]
    [InlineData(1, 1, false, 1)]
    [InlineData(1, 10, null, 10)]
    [InlineData(1, 1, null, 1)]
    public void GetEven_WhereIfShould_QueryIfConditionIsNull(int value, int rangeSize, bool? condition, int expected)
    {
        Enumerable.Range(1, rangeSize)
            .WhereIf(condition, IsEven)
            .Should()
            .HaveCount(expected);
    }

    [Theory]
    [InlineData(1, 10, true, 5)]
    [InlineData(1, 1, true, 0)]
    [InlineData(1, 10, false, 10)]
    [InlineData(1, 1, false, 1)]
    public void GetEven_QueryableWhereIfShould_QueryIfConditionIsSet(int value, int rangeSize, bool condition,
        int expected)
    {
        Enumerable.Range(1, rangeSize)
            .AsQueryable()
            .WhereIf(condition, IsEvenExpr)
            .Should()
            .HaveCount(expected);
    }

    [Theory]
    [InlineData(1, 10, true, 5)]
    [InlineData(1, 1, true, 0)]
    [InlineData(1, 10, false, 10)]
    [InlineData(1, 1, false, 1)]
    [InlineData(1, 10, null, 10)]
    [InlineData(1, 1, null, 1)]
    public void GetEven_QueryableWhereIfShould_QueryIfConditionIsNull(int value, int rangeSize, bool? condition,
        int expected)
    {
        Enumerable.Range(1, rangeSize)
            .AsQueryable()
            .WhereIf(condition, IsEvenExpr)
            .Should()
            .HaveCount(expected);
    }

    [Fact]
    public void MergeDictionaries_ShouldCreateAthirdDictionaryWithMergedValues()
    {
        var first = new Dictionary<string, string>()
        {
            { "1", "10" },
            { "2", "20" },
            { "3", "30" },
        };
        var second = new Dictionary<string, string>()
        {
            { "3", "300" },
            { "4", "400" },
        };

        var third = first.Merge(second);

        third
            .Should()
            .ContainKeys(["1", "2", "3", "4"]);

        third["3"]
            .Should()
            .Be("300");
    }

    [Fact]
    public void MergeDictionaries_ShouldCreateAthirdDictionaryWithMergedValues2()
    {
        var first = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "a", "10" },
            { "b", "20" },
            { "c", "30" },
        };
        var second = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "C", "300" },
            { "d", "400" },
        };

        var third = first.Merge(second);

        third
            .Should()
            .ContainKeys(["a", "b", "c", "C", "d"]);

        third["c"]
            .Should()
            .Be("30");
    }

    [Fact]
    public void ForeachEnumerable_ShouldNavigateObject()
    {
        var array = Enumerable.Range(1, 10).Select(p => p.ToString()).ToArray();
        var newList = new List<int>();
        array.ForEach(p => newList.Add(int.Parse(p)));

        newList.Should().HaveCount(array.Length);
        newList.Should().BeEquivalentTo([
            1, 2, 3, 4, 5, 6, 7, 8, 9, 10
        ]);
    }
}