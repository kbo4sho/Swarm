﻿using Microsoft.Xna.Framework;
using SwarmEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNASwarms.Screens.Emitters;

namespace XNASwarms.Screens.Emitters
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

        public EmitterBase(EmitterType emitterType, Vector2 position)
        {
            IsActive = true;
            EmitterType = emitterType;
            Position = position;
        }

        public virtual Individual Update()
        {
            return new Individual(0, this.Position.X, this.Position.Y, this.Position.X, this.Position.Y, new SuperParameters());
        }
    }
}
