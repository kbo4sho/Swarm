using SwarmEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace SwarmEngine
{
    public static class StaticBrushParameters
    {
        public static int numberOfIndividualsMax = 300;
        public static int neighborhoodRadiusMax = 45;
        public static int normalSpeedMax = 4;
        public static int maxSpeedMax = 8;

        public static double CohesiveForceMax = 1;
        public static double AligningForceMax = 1;
        public static double SeperatingForceMax = 100;
        public static double ChanceOfRandomSteeringMax = 0.5;
        public static double TendencyOfPaceKeepingMax = 1;

        public static Color Color = Colors.Red;

        public static bool IsMobile = true;
    }

    public class BrushParameters : Parameters
    {
        public BrushParameters()
        {
            neighborhoodRadius = StaticBrushParameters.neighborhoodRadiusMax;
            normalSpeed = StaticBrushParameters.normalSpeedMax;
            maxSpeed = GetMaxSpeed();
            c1 = StaticBrushParameters.CohesiveForceMax;
            c2 = StaticBrushParameters.AligningForceMax;
            c3 = StaticBrushParameters.SeperatingForceMax;
            c4 = StaticBrushParameters.ChanceOfRandomSteeringMax;
            c5 = StaticBrushParameters.TendencyOfPaceKeepingMax;
        }

        private double GetMaxSpeed()
        {
            if (StaticBrushParameters.IsMobile)
            {
                return StaticBrushParameters.maxSpeedMax;
            }
            else
            {
                StaticBrushParameters.maxSpeedMax = 0;
                return 0;
            }
        }
    }
}
