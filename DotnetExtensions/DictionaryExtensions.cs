using System;
using System.Collections.Generic;
using System.Linq;

namespace DotnetExtensions
{
    public static class DictionaryExtension
    {
        public static IDictionary<T, U> Merge<T, U>(this IDictionary<T, U> sourceLeft, IDictionary<T, U> sourceRight)
        {
            IDictionary<T, U> result = new Dictionary<T,U>();

            sourceLeft
                .Concat(sourceRight)
                .ToList()
                .ForEach(kvp => 
                    result[kvp.Key] = kvp.Value
                );

            return result;
        }

        public static IDictionary<T, U> Merge<T, U>(this IDictionary<T, U> sourceLeft, IDictionary<T, U> sourceRight, Func<U, U, U> mergeExpression)
        {
            IDictionary<T, U> result = new Dictionary<T,U>();

            //Merge expression example
            //(leftValue, rightValue) => leftValue + " " + rightValue;

            sourceLeft
                .Concat(sourceRight)
                .ToList()
                .ForEach(kvp => 
                    result[kvp.Key] =
                        (!result.ContainsKey(kvp.Key))
                            ? kvp.Value
                            : mergeExpression(result[kvp.Key], kvp.Value)
                );

            return result;
        }


        public static IDictionary<TKey, TValue> Merge<TKey, TValue>(this IDictionary<TKey, TValue> sourceLeft, IEnumerable<IDictionary<TKey, TValue>> sourcesRight)
        {
            IDictionary<TKey, TValue> result = new Dictionary<TKey, TValue>();
      
            new[] { sourceLeft }
                .Concat(sourcesRight)
                .ToList()
                .ForEach(dic =>
                    result = result.Merge(dic)
                );

            return result;
        }

        public static IDictionary<TKey, TValue> Merge<TKey, TValue>(this IDictionary<TKey, TValue> sourceLeft, IEnumerable<IDictionary<TKey, TValue>> sourcesRight, Func<TValue, TValue, TValue> mergeExpression)
        {
            IDictionary<TKey, TValue> result = new Dictionary<TKey, TValue>();

            new[] { sourceLeft }
                .Concat(sourcesRight)
                .ToList()
                .ForEach(dic =>
                    result = result.Merge(dic, mergeExpression)
                );

            return result;
        }
    }
}