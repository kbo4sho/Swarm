using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using SwarmEngine;
using Microsoft.Xna.Framework;
using SwarmAnalysisEngine;
using XNASwarms.Emitters;

namespace XNASwarms
{
    class SwarmScreenDrawScreen : SwarmScreenBase
    {
        public SwarmScreenDrawScreen(IEmitterComponent emitterComponent, PopulationSimulator populationSimulator)
            : base(emitterComponent, populationSimulator)
        {

        }
    }
}
