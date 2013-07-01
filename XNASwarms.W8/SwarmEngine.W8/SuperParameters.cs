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
            c1 = WorldParameters.CohesiveForceMax;
            c2 = WorldParameters.AligningForceMax;
            c3 = WorldParameters.SeperatingForceMax;
            c4 = WorldParameters.ChanceOfRandomSteeringMax;
            c5 = WorldParameters.TendencyOfPaceKeepingMax;
        }
    }
}
