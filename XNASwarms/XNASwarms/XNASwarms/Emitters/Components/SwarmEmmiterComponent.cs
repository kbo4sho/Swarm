using Microsoft.Xna.Framework;
using SwarmEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNASwarms.Emitters;

namespace XNASwarms.Emitters
{
    public class SwarmEmitterComponent : IEmitterComponent
    {
        public List<EmitterBase> Emitters
        {
            get;
            private set;
        }
        private PopulationSimulator populationSimulator;

        public SwarmEmitterComponent(PopulationSimulator simulator)
        {
            populationSimulator = simulator;
            Emitters = new List<EmitterBase>();
            Emitters.Add(new BrushEmitter(new Vector2(0, 0)));
        }

        /// <summary>
        /// Decides how to handle the input based on the
        /// editing mode that is selected
        /// </summary>
        public void UpdateInput(Dictionary<int, Individual> supers)
        {
            if (StaticEditModeParameters.IsBrushMode())
            {
                if (supers.Count > 0)
                {
                    this.Update(new Vector2((float)supers[0].X, (float)supers[0].Y));
                }
                else
                {
                    this.Update(Vector2.Zero);
                }
            }
            else if (StaticEditModeParameters.IsEraseMode())
            {
                if (supers.Count > 0)
                {
                    populationSimulator.EraseIndividual(supers[0].Position);
                }
            }
            else if (StaticEditModeParameters.IsWorldMode())
            {
                this.Update(Vector2.Zero);
            }
        }

        private void Update(Vector2 position)
        {
            if (Emitters[0] is IGuideable)
            {
                ((IGuideable)Emitters[0]).UpdatePosition(position);
            }

            if (Emitters[0] is IMeteredAgents)
            {
                ((IMeteredAgents)Emitters[0]).CheckForSafeDistance(position);
            }

            if (Emitters[0].IsActive)
            {
                if (Emitters[0] is BrushEmitter)
                {
                    if (!StaticBrushParameters.IsUndo)
                    {
                        populationSimulator.EmitIndividual(Emitters[0].GetIndividual());
                    }
                    else
                    {
                        populationSimulator.UndoIndividual();
                    }
                }
                else
                {
                    populationSimulator.EmitIndividual(Emitters[0].GetIndividual());
                }
            }
        }
    }
}
