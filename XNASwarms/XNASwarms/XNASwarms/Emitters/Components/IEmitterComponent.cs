#if NETFX_CORE
using XNASwarmsXAML.W8;
#endif
using Microsoft.Xna.Framework;
using SwarmEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNASwarms.Common;

namespace XNASwarms.Emitters
{
    public interface IEmitterComponent
    {
        void BatchEmit(Population population, bool mutate, ControlGroups groups);
        void UpdateInput(Dictionary<int, Individual> supers, ControlGroups groups);
    }
}
