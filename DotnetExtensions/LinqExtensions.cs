using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DotnetExtensions
{
    public static class LinqExtensions
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
            => source.GroupBy(keySelector).Select(x => x.First());
        
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            using (var enumerator = source.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    action(enumerator.Current);
                }
            }
        }

        public static IEnumerable<TResult> WhereIf<TResult>
        (this IEnumerable<TResult> values, bool condition, Func<TResult, bool> predicateIf,
            Func<TResult, bool> predicateElse = null)
            => condition ? values?.Where(predicateIf)
                : predicateElse != null ? values?.Where(predicateElse)
                : values;

        public static IEnumerable<TResult> WhereIf<TResult>
        (this IEnumerable<TResult> values, bool condition, Func<TResult, int, bool> predicateIf,
            Func<TResult, int, bool> predicateElse = null)
            => condition ? values?.Where(predicateIf)
                : predicateElse != null ? values?.Where(predicateElse)
                : values;

        public static IEnumerable<TResult> WhereIf<TResult>
        (this IEnumerable<TResult> values, bool condition, Func<TResult, int, bool> predicateIf,
            Func<TResult, bool> predicateElse = null)
            => condition ? values?.Where(predicateIf)
                : predicateElse != null ? values?.Where(predicateElse)
                : values;

        public static IEnumerable<TResult> WhereIf<TResult>
        (this IEnumerable<TResult> values, bool condition, Func<TResult, bool> predicateIf,
            Func<TResult, int, bool> predicateElse)
            => condition ? values?.Where(predicateIf)
                : predicateElse != null ? values?.Where(predicateElse)
                : values;

        public static IEnumerable<TResult> WhereIf<TResult>
        (this IEnumerable<TResult> values, bool? condition, Func<TResult, bool> predicateIf,
            Func<TResult, bool> predicateElse = null)
            => condition.HasValue && condition.Value ? values?.Where(predicateIf)
                : predicateElse != null ? values?.Where(predicateElse)
                : values;

        public static IQueryable<TResult> WhereIf<TResult>
        (this IQueryable<TResult> values, bool condition, Expression<Func<TResult, bool>> predicateIf,
            Expression<Func<TResult, bool>> predicateElse = null)
            => condition ? values?.Where(predicateIf)
                : predicateElse != null ? values?.Where(predicateElse)
                : values;

        public static IQueryable<TResult> WhereIf<TResult>
        (this IQueryable<TResult> values, bool? condition, Expression<Func<TResult, bool>> predicateIf,
            Expression<Func<TResult, bool>> predicateElse = null)
            => condition.HasValue && condition.Value ? values?.Where(predicateIf)
                : predicateElse != null ? values?.Where(predicateElse)
                : values;

        public static IDictionary<TKey, TValue> Merge<TKey, TValue>(this IDictionary<TKey, TValue> first,
            IDictionary<TKey, TValue> second, IEqualityComparer<TKey> comparer = null)
        {
            if (first is Dictionary<TKey, TValue> dictionary
                && comparer is null)
                comparer = dictionary.Comparer;
            
            var merged = new Dictionary<TKey, TValue>(comparer);
            if (first != null)
            {
                foreach (var kvp in first)
                {
                    merged[kvp.Key] = kvp.Value;
                }
            }
            if (second == null) return merged;
            
            foreach (var value in second)
            {
                merged[value.Key] = value.Value;
            }
            return merged;
        }
    }
}