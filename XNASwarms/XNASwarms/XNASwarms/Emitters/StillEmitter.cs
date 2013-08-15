using Microsoft.Xna.Framework;
using SwarmEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNASwarms.Emitters;
using XNASwarmsXAML.W8;

namespace XNASwarms.Emitters
{
    public class AudioEmitter : EmitterBase, IAudioInfluenced
    {
        public AudioEmitter(Vector2 position)
            : base(EmitterActionType.Still, position)
        {
            
        }

        public override Individual GetIndividual()
        {
            return new Individual(0, this.Position.X, this.Position.Y, this.Position.X, this.Position.Y, this.Parameters);
        }

        public void UpdateByAudio(double[] fftData)
        {
            //25-100
            var b = fftData.Take(66).Average();
            b = Normalizer.Normalize(25, 100, .01f, 45f, b);
            //0-1
            var g = fftData.Skip(66).Take(66).Average();
            g = Normalizer.Normalize(0, 1, .01f, 6f, g);
            
            //0-1
            var r = fftData.Skip(132).Take(66).Average();
            r = Normalizer.Normalize(0, 1, .001f, 2.5f, r);


            this.Parameters = new Parameters(StaticWorldParameters.neighborhoodRadiusMax,
                                             StaticWorldParameters.normalSpeedMax,
                                             StaticWorldParameters.maxSpeedMax,
                                             r,
                                             g,
                                             b,
                                             StaticWorldParameters.ChanceOfRandomSteeringMax,
                                             StaticWorldParameters.TendencyOfPaceKeepingMax);
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
