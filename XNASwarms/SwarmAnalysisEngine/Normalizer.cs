
using System.Collections.Generic;
using System.Linq;
using System;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SwarmAnalysisEngine
{
    public static class Normalizer
    {
        public static float Width;
        public static float Height;

        #region 0 - 1

        public static float NormalizeWidth(float f)
        {
            return KeepValueInBound(((f + Width / 2) / (Width)));
        }

        public static float NormalizeHeight(float f)
        {
            return KeepValueInBound(((f + Height / 2) / (Height)));
        }

        private static float KeepValueInBound(float value)
        {
            if (value < 0)
            {
                value = 0;
            }

            if (value > 1)
            {
                value = 1;
            }

            return value;
        }

        #endregion

        #region -1 to 1

        public static float NormalizeWidthCentered(float f)
        {
            return KeepValueInCenteredBound((float)((((f + Width / 2) * 2) / (Width)) - 1));
        }

        public static float NormalizeHeightCentered(float f)
        {
            return KeepValueInCenteredBound((float)((((f + Height / 2) * 2) / (Height)) - 1));
        }

        private static float KeepValueInCenteredBound(float value)
        {
            if (value < -1)
            {
                value = -1;
            }

            if (value > 1)
            {
                value = 1;
            }

            return value;
        }

        #endregion        

        public static float NormalizeToScreenArea(float f)
        {
            return (f - 0) * (300 - 0) / ((Width * Height) - 0);
        }

        public static float NormalizePointOneToTen(float f)
        {
            return (f - 0) * (10 - (float).1) / (Width - 0);
        }

        public static float Normalize120To800(float f)
        {
            // 5 = min val
            //40 = max val
            var value = (f - 5) * (800 - 120) / (40 - 5);
            if (value > 800)
            {
                value = 800;
            }
            return value;
        }

        public static float Normalize0ToOne(float f)
        {
            // 5 = min val
            //40 = max val
            var value = (f - 5) * (1 - 0) / (40 - 5);
            if (value > 1)
            {
                value = 1;
            }
            return value;
        }


        public static float Normalize(int from, int to, float min, float max, double toNormalize)
        {
            if (toNormalize < min)
            {
                toNormalize = min;
            }

            if (toNormalize > max)
            {
                toNormalize = max;
            }

            var value = (toNormalize - min) * (to - from) / (max - min);
            return (float)value;
        }
    }
}
