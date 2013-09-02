using Microsoft.Xna.Framework;
using SwarmEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNASwarms.Common;
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
        private Random rand = new Random();

        public SwarmEmitterComponent(PopulationSimulator simulator)
        {
            populationSimulator = simulator;
            Emitters = new List<EmitterBase>();
            Emitters.Add(new BrushEmitter(new Vector2(0, 0)));
        }

        public void BatchEmit(Population population, bool mutate, ControlGroups groups)
        {
            if (mutate)
            {
                for (int i = 0; i < population.Count; i++)
                {
                    for (int j = 0; j < population[i].Count; j++)
                    {
                        population[i][j].Genome.inducePointMutations(rand.NextDouble(), 1);
                    }
                }
                population.ReassignSpecies();
                population.ReassignAllColors();
            }

            foreach (var spec in population)
            {
                foreach (var indvd in spec)
                {
                    EmitIndividual(indvd, "World", groups);
                }
            }
        }

        /// <summary>
        /// Decides how to handle the input based on the
        /// editing mode that is selected
        /// </summary>
        public void UpdateInput(Dictionary<int, Individual> supers, ControlGroups groups)
        {
            if (StaticEditModeParameters.IsBrushMode())
            {
                if (supers.Count > 0)
                {
                    this.Update(new Vector2((float)supers[0].X, (float)supers[0].Y), groups);
                }
                else
                {
                    this.Update(Vector2.Zero, groups);
                }
            }
            else if (StaticEditModeParameters.IsEraseMode())
            {
                if (supers.Count > 0)
                {
                    EraseIndividual(supers[0].Position, groups);
                }
            }
            else if (StaticEditModeParameters.IsWorldMode())
            {
                this.Update(Vector2.Zero, groups);
            }
        }

        private void Update(Vector2 position, ControlGroups groups)
        {
            foreach (EmitterBase emitter in Emitters)
            {
                if (emitter is IGuideable)
                {
                    ((IGuideable)Emitters[0]).UpdatePosition(position);
                }

                if (emitter is IMeteredAgents)
                {
                    ((IMeteredAgents)emitter).CheckForSafeDistance(position);
                }

                if (emitter.IsActive)
                {
                    if (emitter is BrushEmitter)
                    {
                        if (StaticBrushParameters.IsUndo)
                        {
                            populationSimulator.UndoIndividual();
                        }
                        else
                        {
                            EmitIndividual(emitter.GetIndividual(), "User", groups);
                        }
                    }
                    else
                    {
                        EmitIndividual(emitter.GetIndividual(), "World", groups);
                    }
                }
            }
        }

        private void EmitIndividual(Individual indvd, string group, ControlGroups groups)
        {
            indvd.ID = GetNextIndividualID();
            populationSimulator.EmitIndividual(indvd);
            groups.Add(indvd.ID, group);
        }

        private void EraseIndividual(Vector2 position, ControlGroups groups)
        {
            double diameter = StaticEraseParameters.Diameter;

            List<Individual> matches = populationSimulator.Population.SelectMany(s => s
                                   .Where(i => (int)i.X >= (int)position.X - diameter &&
                                               (int)i.X <= (int)position.X + diameter &&
                                               (int)i.Y >= (int)position.Y - diameter &&
                                               (int)i.Y <= (int)position.Y + diameter)).ToList();

            if (matches.Any())
            {
                populationSimulator.EraseIndividual(matches.First());
                groups.Remove(matches.First().ID);
            }
        }

        private int GetNextIndividualID()
        {
            int tempID = 1;
            return TryIterate(tempID);
        }

        private int TryIterate(int tempID)
        {
            if (!populationSimulator.Population.SelectMany(s => s.Where(i => i.ID == tempID)).Any())
            {
                return tempID;
            }
            else
            {
                tempID++;
                return TryIterate(tempID);
            }
        }
    }
}
