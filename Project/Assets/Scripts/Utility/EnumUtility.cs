
using System;
using UnityEngine;

namespace Rondo.Generic.Utility {

    public static class EnumUtility {

        public static T GetRandomEnumValue<T>(int startIndex = 0, int endIndex = -1) {
            var v = Enum.GetValues(typeof(T));
            if (endIndex == -1) endIndex = v.Length;
            else endIndex++;
            return (T)v.GetValue(UnityEngine.Random.Range(startIndex, endIndex));
        }

    }

}