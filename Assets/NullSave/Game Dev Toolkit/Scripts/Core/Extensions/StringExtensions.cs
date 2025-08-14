using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace NullSave.GDTK
{
    public static class StringExtensions
    {

        #region Public Methods

        public static bool IsAllowedId(this string value)
        {
            return Regex.IsMatch(value, "^[A-Za-z0-9_]*$");
        }

        /// <summary>
        /// Check is a string is a numeric only value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNumeric(this string value)
        {
            return float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out _);
        }

        public static void Log(string source, string method, string message)
        {
            Debug.Log(source + "." + method + ": " + message);
        }

        public static void LogError(string source, string method, string message)
        {
            Debug.LogError(source + "." + method + ": " + message);
        }

        public static void LogWarning(string source, string method, string message)
        {
            Debug.LogWarning(source + "." + method + ": " + message);
        }

        /// <summary>
        /// Convert string to boolean
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ToBool(this string value)
        {
            if (string.IsNullOrEmpty(value)) return false;
            if (value.ToLower() == "true") return true;
            if (value.ToLower() == "yes") return true;
            if (value.ToLower() == "on") return true;
            if (value.ToLower() == "checked") return true;
            if (value.ToLower() == "check") return true;

            if (int.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out int iVal))
            {
                return iVal != 0;
            }

            return false;
        }

        public static string ToCamelCase(this string str)
        {
            if (string.IsNullOrEmpty(str)) return str;

            var words = str.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            var leadWord = Regex.Replace(words[0], @"([A-Z])([A-Z]+|[a-z0-9]+)($|[A-Z]\w*)",
                m =>
                {
                    return m.Groups[1].Value.ToLower() + m.Groups[2].Value.ToLower() + m.Groups[3].Value;
                });
            var tailWords = words.Skip(1)
                .Select(word => char.ToUpper(word[0]) + word.Substring(1))
                .ToArray();
            return $"{leadWord}{string.Join(string.Empty, tailWords)}";
        }

        public static string ToAllowedId(this string str)
        {
            if (string.IsNullOrEmpty(str)) return str;

            return Regex.Replace(str, "[^A-Za-z0-9_]", " ", RegexOptions.Compiled).ToCamelCase();
        }

        #endregion

    }
}