using Microsoft.Xna.Framework;
using SwarmEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNASwarms.Emitters
{
    public abstract class EmitterBase : IEmitter 
    {
        public bool IsActive
        {
            get;
            private set;
        }

        public EmitterType EmitterType
        {
            get;
            private set;
        }

        public Vector2 Position
        {
            get;
            protected set;
        }

        public Parameters Parameters
        {
            get;
            protected set;
        }

        public EmitterBase(EmitterType emitterType, Vector2 position)
            :this(emitterType, position, new SuperParameters()){}

        public EmitterBase(EmitterType emitterType, Vector2 position, Parameters parameters)
        {
            IsActive = true;
            EmitterType = emitterType;
            Position = position;
            Parameters = parameters;
        }

        public virtual Individual Update()
        {
            return new Individual(0, this.Position.X, this.Position.Y, this.Position.X, this.Position.Y, new SuperParameters());
        }

        protected void SetActive(bool value)
        {
            this.IsActive = value;
        }
    }
}
