using Microsoft.Xna.Framework;
using SwarmEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNASwarms.Emitters
{
    public class BrushEmitter : EmitterBase, IGuideable, IAudioInfluenced, IMeteredAgents
    {
        private const float space = 7f;

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
                                             WorldParameters.CohesiveForceMax,
                                             WorldParameters.AligningForceMax,
                                             WorldParameters.SeperatingForceMax,
                                             WorldParameters.ChanceOfRandomSteeringMax,
                                             WorldParameters.TendencyOfPaceKeepingMax);
        }

        public Vector2 lastPosition { get; set; }

        public bool canDraw { get; set; }

        public void CheckForSafeDistance(Vector2 position)
        {
            canDraw = false;
            if (lastPosition == null)
            {
                lastPosition = position;
                canDraw = true;
            }
            //TODO: Create helper method to determine this number
            if ((position.Y > (lastPosition.Y + space)) ||
                (position.Y < (lastPosition.Y - space)) ||
                (position.X > (lastPosition.X + space)) ||
                (position.X < (lastPosition.X - space)))
            {
                canDraw = true;
                lastPosition = position;
            }

            if (canDraw)
            {
                this.SetActive(true);
            }
            else
            {
                this.SetActive(false);
            }
        }
    }
}
