using Microsoft.Xna.Framework;
using SwarmEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNASwarms.Screens.Emitters
{
    public class BrushEmitter : EmitterBase, IGuideable
    {
        public BrushEmitter(Vector2 position)
            : base(EmitterType.Brush, position)
        {
            
        }

        public override Individual Update()
        {
            return new Individual(0, this.Position.X, this.Position.Y, this.Position.X, this.Position.Y, new SuperParameters());
        }


        public void UpdatePosition(Vector2 position)
        {
            this.Position = position;
        }
    }
}
