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
    public static class WorldParameters
    {
        //TODO Read values from config file
        public static int numberOfIndividualsMax = 100;
        public static int neighborhoodRadiusMax = 60;
        public static int normalSpeedMax = 5;
        public static int maxSpeedMax = 10;
        public static double c1Max = 1;
        public static double c2Max = 1;
        public static double c3Max = 100;
        public static double c4Max = 0.5;
        public static double c5Max = 1;
    }
}
