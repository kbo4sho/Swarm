using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using SwarmEngine;
using Microsoft.Xna.Framework;
using SwarmAnalysisEngine;

namespace XNASwarms
{
    class SwarmScreenDrawScreen : SwarmScreenBase
    {
        Random rand;
        Texture2D individualTexture, bigIndividualTexture;
        bool Mutate;

        public SwarmScreenDrawScreen(bool mutate)
        {
            Mutate = mutate;
            rand = new Random();
        }

        public override void LoadContent()
        {
            individualTexture = ScreenManager.Content.Load<Texture2D>("bee");
            bigIndividualTexture = ScreenManager.Content.Load<Texture2D>("beebig");
            if (Mutate)
            {
                DoMutation();
            }
            base.LoadContent();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            Population population = populationSimulator.GetPopulation();
            ScreenManager.SpriteBatch.Begin(0, null, null, null, null, null, Camera.View);
            base.Draw(gameTime);
            
            foreach (Species spcs in population)
            {
                foreach (Individual indvd in spcs)
                {
                    DrawIndividual(indvd, indvd.getGenomeColor(), individualTexture);
                }
            }
            ScreenManager.SpriteBatch.End();
        }

        private void DrawIndividual(Individual indvd,Color color, Texture2D texture)
        {
            ScreenManager.SpriteBatch.Draw(texture, new Rectangle(
                        (int)indvd.getX(),
                        (int)indvd.getY(), texture.Width, texture.Height),
                        null,
                        color,
                        0f,
                        new Vector2(texture.Width / 2, texture.Height / 2),
                        SpriteEffects.None, 0);
        }

        private void DoMutation()
        {
            foreach (Species spcs in populationSimulator.GetPopulation())
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
