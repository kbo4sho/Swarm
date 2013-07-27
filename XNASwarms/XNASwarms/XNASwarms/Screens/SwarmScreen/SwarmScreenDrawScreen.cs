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
            individualTexture = ScreenManager.Content.Load<Texture2D>("point");
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

            for (int s = 0; s < population.Count; s++ )
            {
                for (int i = 0; i < population[s].Count; i++)
                {
                    DrawIndividual(population[s][i], population[s][i].getDisplayColor(), individualTexture, population[s][i].EmitterType);
                }
            }

            base.Draw(gameTime);
            ScreenManager.SpriteBatch.End();
        }

        private void DrawIndividual(Individual indvd,Color color, Texture2D texture, EmitterType type)
        {
            if (type == EmitterType.Brush)
            {
                if (!indvd.IsMobile)
                {
                    ScreenManager.SpriteBatch.Draw(bigIndividualTexture, new Rectangle(
                        (int)indvd.X,
                        (int)indvd.Y, bigIndividualTexture.Width, bigIndividualTexture.Height),
                        null,
                        color,
                        (float)Math.Atan2(indvd.getDy(), indvd.getDx()),
                        new Vector2(bigIndividualTexture.Width / 2, bigIndividualTexture.Height / 2),
                        SpriteEffects.None, 0);
                }
                else
                {
                    ScreenManager.SpriteBatch.Draw(texture, new Rectangle(
                        (int)indvd.X,
                        (int)indvd.Y, texture.Width, texture.Height),
                        null,
                        color,
                        (float)Math.Atan2(indvd.getDy(), indvd.getDx()),
                        new Vector2(texture.Width / 2, texture.Height / 2),
                        SpriteEffects.None, 0);
                }
            }
            else
            {
                ScreenManager.SpriteBatch.Draw(texture, new Rectangle(
                        (int)indvd.X,
                        (int)indvd.Y, texture.Width, texture.Height),
                        null,
                        color,
                        (float)Math.Atan2(indvd.getDy(), indvd.getDx()),
                        new Vector2(texture.Width / 2, texture.Height / 2),
                        SpriteEffects.None, 0);
            }
        }

        private void DoMutation()
        {
            foreach (Species spcs in populationSimulator.GetPopulation())
            {
                foreach (Individual indvdl in spcs)
                {
                    indvdl.Genome.inducePointMutations(rand.NextDouble(), 2);
                    //ind.Genome.inducePointMutations(rand.NextDouble(), 3);
                }
            }
            populationSimulator.DetermineSpecies();

        }
    }
}
