using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwarmEngine
{
    /// <summary>
    /// Static class to hold parameters for game
    /// </summary>
    public static class StaticWorldParameters
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
    }
}
