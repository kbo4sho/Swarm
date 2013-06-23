using Microsoft.Xna.Framework;
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
            private set;
        }

        public EmitterBase(EmitterType emitterType)
        {
            IsActive = true;
            EmitterType = emitterType;
            Position = new Vector2(0, 0);
        }
    }
}
