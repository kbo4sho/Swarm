using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNASwarmsXAML.W8
{
    public static class Normalizer
    {
        public static float Normalize(int from, int to, int min, int max, int toNormalize)
        {
            var value = (toNormalize - min) * (to - from) / (max - min);
            return value;
        }
    }
}
