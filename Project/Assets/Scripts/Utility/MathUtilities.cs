using UnityEngine;

namespace Rondo.Generic.Utility {

    public static class MathUtilities {

        public static Vector2 GetRandomVector2(float minX, float minY, float maxX, float maxY) {
			return new Vector2(
				UnityEngine.Random.Range(minX, maxX),
				UnityEngine.Random.Range(minY, maxY));
		}

		public static float RemapFloat(float value, float min, float max, float targetMin, float targetMax) {
			value = Mathf.Clamp(value, min, max);
			float result = targetMin + (value - min) * (targetMax - targetMin) / (max - min);
			if (Mathf.Approximately(result, 0))
				return 0;

			return result;
		}

	}

}