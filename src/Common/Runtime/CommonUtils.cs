using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;

namespace Racer.EzUtilities.Common.Runtime
{
    /// <summary>
    /// Provides utility methods for common operations in Unity projects.
    /// </summary>
    public static class CommonUtils
    {
#if UNITY_EDITOR
        /// <summary>
        /// Deletes multiple assets from the Unity project.
        /// </summary>
        /// <param name="paths">Asset paths to delete.</param>
        public static void DeleteAssets(string[] paths)
        {
            AssetDatabase.DeleteAssets(paths, new List<string>());
        }
#endif

        /// <summary>
        /// Converts an input string into a sanitized identifier-like value.
        /// </summary>
        /// <param name="input">Source text to sanitize.</param>
        /// <returns>
        /// A concatenated alphanumeric string with each segment capitalized, prefixed with `_` when needed.
        /// </returns>
        public static string SanitizeString(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            var words = input.Split(new[] { ' ', '_', '-', '.' }, StringSplitOptions.RemoveEmptyEntries);
            var sb = new StringBuilder();

            foreach (var word in words)
            {
                var cleaned = new string(word.Where(char.IsLetterOrDigit).ToArray());
                if (cleaned.Length == 0)
                    continue;

                sb.Append(char.ToUpperInvariant(cleaned[0]));
                if (cleaned.Length > 1)
                    sb.Append(cleaned[1..]);
            }

            var result = sb.ToString();
            if (string.IsNullOrEmpty(result))
                return input;

            if (!char.IsLetter(result[0]) && result[0] != '_')
                result = "_" + result;

            return result;
        }
    }
}