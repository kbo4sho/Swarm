using Microsoft.Xna.Framework;
using SwarmEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNASwarms.Emitters
{
    public class BrushEmitter : EmitterBase, IGuideable, IAudioInfluenced
    {
        public BrushEmitter(Vector2 position)
            : base(EmitterType.Brush, position)
        {
            
        }

        public override Individual Update()
        {
            return new Individual(0, this.Position.X, this.Position.Y, this.Position.X, this.Position.Y, this.Parameters);
        }


        public void UpdatePosition(Vector2 position)
        {
            this.Position = position;
        }

        public void UpdateByAudio(double[] fftData)
        {
            this.Parameters = new Parameters(WorldParameters.neighborhoodRadiusMax,
                                             fftData[0],
                                             WorldParameters.maxSpeedMax,
                                             fftData[12],
                                             WorldParameters.AligningForceMax,
                                             WorldParameters.SeperatingForceMax,
                                             WorldParameters.ChanceOfRandomSteeringMax,
                                             WorldParameters.TendencyOfPaceKeepingMax);
        }
    }
}
