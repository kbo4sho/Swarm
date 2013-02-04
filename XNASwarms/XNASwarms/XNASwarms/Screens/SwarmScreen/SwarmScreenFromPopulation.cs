using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SwarmEngine;

namespace XNASwarms
{
    class SwarmScreenFromPopulation : SwarmScreenDrawScreen
    {
        public SwarmScreenFromPopulation(Population population)
            : base(false)
        {
            populationSimulator = new PopulationSimulator(0, 0, StockSpecies.Species_A);
        }
    }
}
