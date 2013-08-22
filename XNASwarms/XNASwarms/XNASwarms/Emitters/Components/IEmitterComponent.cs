using Microsoft.Xna.Framework;
using SwarmEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if NETFX_CORE
using XNASwarmsXAML.W8;
#endif

namespace XNASwarms.Emitters
{
    public interface IEmitterComponent
    {
        void BatchEmit(Population population, bool mutate);
        void UpdateInput(Dictionary<int, Individual> supers);
    }
}
