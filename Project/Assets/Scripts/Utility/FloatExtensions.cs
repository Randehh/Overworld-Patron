﻿
using System;
using System.Globalization;
using UnityEngine;

namespace Rondo.Generic.Utility {

    public static class FloatExtensions {

        public static float Map(this float value, float from1, float to1, float from2, float to2) {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

    }

}