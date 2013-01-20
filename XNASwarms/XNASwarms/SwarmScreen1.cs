using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace XNASwarms
{
    class SwarmScreen1 : SwarmScreenBase
    {
        Texture2D swarmIndividual;

        public SwarmScreen1()
        {
            recipes = new Recipe[2];
            recipes[0] = new Recipe(StockRecipies.Recipe1());
            recipes[1] = new Recipe(StockRecipies.Recipe2());
        }

        public override void LoadContent()
        {
            swarmIndividual = ScreenManager.Content.Load<Texture2D>("bee");
            base.LoadContent();
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            ScreenManager.SpriteBatch.Begin(0, null, null, null, null, null, Camera.View);
            Population population = populationSimulator.getPopulation();
            for (int i = 0; i < populationSimulator.getPopulation().Count; i++)
            {
                ScreenManager.SpriteBatch.Draw(swarmIndividual, new Rectangle(
                    (int)population.get(i).getX(),
                    (int)population.get(i).getY(), 5, 5),
                    null,
                    population[i].getDisplayColor(),
                    0f,
                    new Vector2(0, 0),
                    SpriteEffects.None, 0);
            }

            base.Draw(gameTime);
            ScreenManager.SpriteBatch.End();
            
        }
    }
}
