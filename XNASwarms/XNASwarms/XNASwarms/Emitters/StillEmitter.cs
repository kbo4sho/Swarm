using Microsoft.Xna.Framework;
using SwarmEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNASwarms.Emitters;

namespace XNASwarms.Emitters
{
    public class StillEmitter : EmitterBase, IAudioInfluenced
    {
        public StillEmitter(Vector2 position)
            : base(EmitterType.Still, position)
        {
            
        }

        public override Individual Update()
        {
            return new Individual(0, this.Position.X, this.Position.Y, this.Position.X, this.Position.Y, this.Parameters);
        }

        public void UpdateByAudio(double[] fftData)
        {
            this.Parameters = new Parameters(WorldParameters.neighborhoodRadiusMax,
                                             GetTime(fftData[1]),
                                             WorldParameters.maxSpeedMax,
                                             WorldParameters.CohesiveForceMax,
                                             WorldParameters.AligningForceMax,
                                             WorldParameters.SeperatingForceMax,
                                             WorldParameters.ChanceOfRandomSteeringMax,
                                             WorldParameters.TendencyOfPaceKeepingMax);
        }

        private int GetTime(double value)
        {
            if (value > 15)
            {
                return 15;
            }

            if (value < 0)
            {
                return 1;
            }

            return (int)value;
        }
    }
}
