using System.Linq.Expressions;
using FluentAssertions;

namespace DotnetExtensions.Test;

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
    public void MergeDictionaries_ShouldCreateAthirdDictionaryWithMergedValues1()
    {
        var first = new Dictionary<string, string>()
        {
            { "a", "10" },
            { "b", "20" },
            { "c", "30" },
        };
        var second = new Dictionary<string, string>()
        {
            { "C", "300" },
            { "d", "400" },
        };
        
        var third = first.Merge(second, StringComparer.OrdinalIgnoreCase);
        
        third
            .Should()
            .ContainKeys(["a", "b", "c", "d"]);
        
        third["c"]
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
        var second = new Dictionary<string, string>()
        {
            { "C", "300" },
            { "d", "400" },
        };
        
        var third = first.Merge(second);
        
        third
            .Should()
            .ContainKeys(["a", "b", "c", "d"]);
        
        third["c"]
            .Should()
            .Be("300");
    }
}