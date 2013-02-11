using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SwarmEngine;

namespace XNASwarms
{
#if WINDOWS
    [Serializable]
#endif
    public class SaveGenome
    {
        public double neighborhoodRadius;
        public double normalSpeed;
        public double maxSpeed;
        public double c1;
        public double c2;
        public double c3;
        public double c4;
        public double c5;

        public SaveGenome()
        {

        }

        public Color getDisplayColor()
        {
            return new Color((float)(c1 / Parameters.c1Max * 0.8),
                    (float)(c2 / Parameters.c2Max * 0.8), (float)(c3 / Parameters.c3Max * 0.8));
        }
    }
}
