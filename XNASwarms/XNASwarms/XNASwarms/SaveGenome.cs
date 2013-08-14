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

        public double x;
        public double y;
        public double dx;
        public double dy;
        public double dx2;
        public double dy2;

        public EmitterActionType type;
        public bool isMobile;


        public SaveGenome()
        {

        }

        public Color getDisplayColor()
        {
            return new Color((float)(c1 / StaticWorldParameters.CohesiveForceMax * 0.8),
                    (float)(c2 / StaticWorldParameters.AligningForceMax * 0.8), (float)(c3 / StaticWorldParameters.SeperatingForceMax * 0.8));
        }
    }
}
