using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace DotnetExtensions
{
    public static class StringExtensions
    {
        public static bool IsNullOrWhiteSpace(this string value)
            => string.IsNullOrWhiteSpace(value);
        
        public static bool IsNotNullOrEmpty(this string value)
            => !string.IsNullOrWhiteSpace(value);
        
        public static bool EndsWithAny(this string value, params string[] values)
            => values.Any(value.EndsWith);
        
        public static bool StartsWithAny(this string value, params string[] values)
            => values.Any(value.StartsWith);
        
        public static bool ContainsAny(this string value, params string[] values)
            => values.Any(value.Contains);

        public static string ReplaceAll(this string value, string oldValue, string newValue, StringComparison comparisonType = StringComparison.CurrentCulture)
        {
            return Regex.Replace(value, oldValue, newValue, RegexOptions.IgnoreCase);
        }

        public static bool WithAproximation(this string value, string compare, double aproximation)
        {
            var distance = value.CalculateDistance(compare);
            double percentage = ((double)distance / (Math.Max(value.Length, compare.Length))) * 100.0;
            return (100 - percentage) >= aproximation;
        } 
        
        /// <summary>
        ///     Calculate the difference between 2 strings using the Levenshtein distance algorithm
        /// </summary>
        /// <param name="source1">First string</param>
        /// <param name="source2">Second string</param>
        /// <returns></returns>
        public static int CalculateDistance(this string source1, string source2)
        {
            var source1Length = source1.Length;
            var source2Length = source2.Length;

            var matrix = new int[source1Length + 1, source2Length + 1];
            
            if (source1Length == 0)
                return source2Length;

            if (source2Length == 0)
                return source1Length;

            for (var i = 0; i <= source1Length; matrix[i, 0] = i++){}
            for (var j = 0; j <= source2Length; matrix[0, j] = j++){}
            
            for (var i = 1; i <= source1Length; i++)
            {
                for (var j = 1; j <= source2Length; j++)
                {
                    var cost = (source2[j - 1] == source1[i - 1]) ? 0 : 1;
                    
                    matrix[i, j] = Math.Min(
                        Math.Min(matrix[i - 1, j] + 1, matrix[i, j - 1] + 1),
                        matrix[i - 1, j - 1] + cost);
                }
            }
            return matrix[source1Length, source2Length];
        }
    }
}