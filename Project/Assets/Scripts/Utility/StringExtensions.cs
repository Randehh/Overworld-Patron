
using System;
using System.Globalization;
using UnityEngine;

namespace Rondo.Generic.Utility {

    public static class StringUtility {

        public static string ToCamelCase(this string text) {
            text = text.Replace('_', ' ');
            return char.ToUpperInvariant(text[0]) + text.Substring(1, text.Length - 1).ToLowerInvariant();
        }

        public static string ToUpperCaseFirstCharacter(this string text) {
            return char.ToUpper(text[0]) + text.Substring(1);
        }
    }

}