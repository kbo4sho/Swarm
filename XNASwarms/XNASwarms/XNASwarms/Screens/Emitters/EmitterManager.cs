using Microsoft.Xna.Framework;
using SwarmEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNASwarms.Screens.Emitters;

namespace XNASwarms.Screens.Emitters
{
    public class EmitterManager
    {
        public List<EmitterBase> Emitters
        {
            get;
            private set;
        }

        private PopulationSimulator populationSimulator;

        //TODO Should have an overload for loading emitters from saves
        public EmitterManager(PopulationSimulator simulator)
        {
            populationSimulator = simulator;
            Emitters = new List<EmitterBase>();
            Emitters.Add(new BrushEmitter(new Vector2(-200,0)));
        }

        //TODO this will be a collection of positons 
        public void Update(Vector2 position)
        {
            foreach(EmitterBase emitter in Emitters)
            {
                if (emitter is IGuideable)
                {
                    ((IGuideable)emitter).UpdatePosition(position);
                }

                if (emitter.IsActive)
                {
                    populationSimulator.EmitIndividual(emitter.Update());
                }
            }
        }
    }
}
