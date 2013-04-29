
using System.Collections.Generic;
using System.Linq;
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
            //return KeepValueInBound(((f + max / 2) / (max)));
            return f * 300 / (Width * Height);

        }
    }
}
