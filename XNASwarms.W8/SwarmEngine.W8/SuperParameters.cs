using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwarmEngine
{
    public class SuperParameters : Parameters
    {
        public SuperParameters()
        {
            neighborhoodRadius = WorldParameters.neighborhoodRadiusMax;
            normalSpeed = WorldParameters.normalSpeedMax;
            maxSpeed = WorldParameters.maxSpeedMax;
            c1 = WorldParameters.c1Max;
            c2 = WorldParameters.c2Max;
            c3 = WorldParameters.c3Max;
            c4 = WorldParameters.c4Max;
            c5 = WorldParameters.c5Max;
        }
    }
}
