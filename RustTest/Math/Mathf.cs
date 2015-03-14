using System;

namespace RustTest
{
    public static class Mathf
    {
        public static float DeltaAngle(float current, float target) {
            float single = Mathf.Repeat(target - current, 360f);
            if (single > 180f) {
                single = single - 360f;
            }
            return single;
        }

        public static float Floor(float f) {
            return (float)Math.Floor((double)f);
        }

        public static float Repeat(float t, float length) {
            return t - Mathf.Floor(t / length) * length;
        }

        public static int FloorToInt(float f) {
            return (int)Math.Floor((double)f);
        }

        public static int RoundToInt(float f) {
            return (int)Math.Round((double)f);
        }
    }
}
