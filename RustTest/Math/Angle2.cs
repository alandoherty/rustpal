using System;

namespace RustTest
{
    public class Angle2
    {
        private static readonly float[] eights360;
        public float X { get; set; }
        public float Y { get; set; }

        static Angle2() {
            eights360 = new float[0x2000];
            for (long i = 0L; i < 0x2000L; i += 1L) {
                eights360[(int)((IntPtr)i)] = (float)((((double)i) / 65536.0) * 360.0);
            }
        }


        public Angle2(float x, float y) {
            // TODO: Complete member initialization
            this.X = x;
            this.Y = y;
        }

        public Angle2() {
            // TODO: Complete member initialization
        }

        public int encoded {
            get {
                return ((Encode360(this.Y) << 0x10) | Encode360(this.X));
            }
            set {
                this.X = Decode360(value & 0xffff);
                this.Y = Decode360((value >> 0x10) & 0xffff);
            }
        }

        public Angle2 decoded {
            get {
                Angle2 angle = this;
                angle.encoded = this.encoded;
                return angle;
            }
        }

        public static float Decode360(int x) {
            int num = x / 0x2000;
            float num2 = (num * 45f) + eights360[x - (num * 0x2000)];
            return ((num2 >= 180f) ? (num2 - 360f) : num2);
        }


        public static int Encode360(float x) {
            x = Mathf.DeltaAngle(0f, x);
            if (x < 0f) {
                x += 360f;
            }
            switch ((Mathf.FloorToInt(x) / 0x2d)) {
                case 0:
                    return Mathf.RoundToInt((float)(x * 182.04444444444445));

                case 1:
                    return (Mathf.RoundToInt((float)((x - 45f) * 182.04444444444445)) + 0x2000);

                case 2:
                    return (Mathf.RoundToInt((float)((x - 90f) * 182.04444444444445)) + 0x4000);

                case 3:
                    return (Mathf.RoundToInt((float)((x - 135f) * 182.04444444444445)) + 0x6000);

                case 4:
                    return (Mathf.RoundToInt((float)((x - 180f) * 182.04444444444445)) + 0x8000);

                case 5:
                    return (Mathf.RoundToInt((float)((x - 225f) * 182.04444444444445)) + 0xa000);

                case 6:
                    return (Mathf.RoundToInt((float)((x - 270f) * 182.04444444444445)) + 0xc000);

                case 7:
                    return (Mathf.RoundToInt((float)((x - 315f) * 182.04444444444445)) + 0xe000);

                case 8:
                    return 0;
            }
            return -1;
        }
    }
}
