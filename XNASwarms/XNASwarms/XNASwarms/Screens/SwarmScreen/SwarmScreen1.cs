using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SwarmEngine;
using XNASwarms.Emitters;
using XNASwarms.Analysis.Components;

namespace XNASwarms.Screens.SwarmScreen
{
    class SwarmScreen1 : SwarmScreenBase
    {
        public SwarmScreen1(IEmitterComponent emitterComponent, IAnalysisComponent analysisComponent, PopulationSimulator populationSimulator)
            : base(emitterComponent, analysisComponent, populationSimulator)
        {

            
        }
    }
}
