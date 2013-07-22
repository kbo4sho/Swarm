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
            neighborhoodRadius = StaticWorldParameters.neighborhoodRadiusMax;
            normalSpeed = StaticWorldParameters.normalSpeedMax;
            maxSpeed = StaticWorldParameters.maxSpeedMax;
            c1 = StaticWorldParameters.CohesiveForceMax;
            c2 = StaticWorldParameters.AligningForceMax;
            c3 = StaticWorldParameters.SeperatingForceMax;
            c4 = StaticWorldParameters.ChanceOfRandomSteeringMax;
            c5 = StaticWorldParameters.TendencyOfPaceKeepingMax;
        }
    }
}
