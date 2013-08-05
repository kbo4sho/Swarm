using SwarmEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if WINDOWS_PHONE
using System.Windows.Media;
#else
#if NETFX_CORE
using Windows.UI;
#endif
#endif



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

#if NETFX_CORE || WINDOWS_PHONE
        public static Color Color = Colors.Red;
#endif

        public static bool IsMobile = true;
        public static double StartingDirection = 0;
    }

    public class BrushParameters : Parameters
    {
        public BrushParameters()
        {
            neighborhoodRadius = StaticBrushParameters.neighborhoodRadiusMax;
            normalSpeed = GetSpeed();
            maxSpeed = StaticBrushParameters.maxSpeedMax;
            c1 = StaticBrushParameters.CohesiveForceMax;
            c2 = StaticBrushParameters.AligningForceMax;
            c3 = GetSeperatingForce();
            c4 = StaticBrushParameters.ChanceOfRandomSteeringMax;
            c5 = StaticBrushParameters.TendencyOfPaceKeepingMax;
        }

        private double GetSpeed()
        {
            if (StaticBrushParameters.IsMobile)
            {
                return StaticBrushParameters.normalSpeedMax;
            }
            else
            {
                return 0;
            }
        }

        private double GetSeperatingForce()
        {
            if (StaticBrushParameters.SeperatingForceMax < 25)
            {
                return 25;
            }
            else
            {
                return StaticBrushParameters.SeperatingForceMax;
            }
        }
    }
}
