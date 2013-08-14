using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SwarmEngine;

namespace XNASwarms
{
    class SwarmScreen1 : SwarmScreenDrawScreen
    {
        public SwarmScreen1(string recipe, bool mutate)
        {
            Recipe[] recipes = new Recipe[1];
            recipes[0] = new Recipe(recipe);
            populationSimulator = new PopulationSimulator(0, 0, recipes);
        }
        
    }
}
