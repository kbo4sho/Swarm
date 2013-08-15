using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNASwarmsXAML.W8
{
    public static class Normalizer
    {
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
