using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using SwarmEngine;
using Microsoft.Xna.Framework;

namespace XNASwarms
{
    class SwarmScreenDrawScreen : SwarmScreenBase
    {
        Random rand;
        Texture2D swarmIndividual;
        bool Mutate;

        public SwarmScreenDrawScreen(bool mutate)
        {
            Mutate = mutate;
            rand = new Random();
        }

        public override void LoadContent()
        {
            swarmIndividual = ScreenManager.Content.Load<Texture2D>("bee");
            if (Mutate)
            {
                DoMutation();
            }
            base.LoadContent();
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            ScreenManager.SpriteBatch.Begin(0, null, null, null, null, null, Camera.View);
            base.Draw(gameTime);
            Population population = populationSimulator.getPopulation();

            foreach (Species spcs in population)
            {
                foreach (Individual indvd in spcs)
                {
                    ScreenManager.SpriteBatch.Draw(swarmIndividual, new Rectangle(
                        (int)indvd.getX(),
                        (int)indvd.getY(), swarmIndividual.Width, swarmIndividual.Height),
                        null,
                        indvd.getDisplayColor(),
                        0f,
                        new Vector2(swarmIndividual.Width / 2, swarmIndividual.Height/2),
                        SpriteEffects.None, 0);
                }
            }
            ScreenManager.SpriteBatch.End();
        }

        private void DoMutation()
        {
            foreach (Species spcs in populationSimulator.getPopulation())
            {
                foreach (Individual indvdl in spcs)
                {
                    indvdl.getGenome().inducePointMutations(rand.NextDouble(), 2);
                    //ind.getGenome().inducePointMutations(rand.NextDouble(), 3);
                }
            }
            populationSimulator.DetermineSpecies();

        }
    }
}
