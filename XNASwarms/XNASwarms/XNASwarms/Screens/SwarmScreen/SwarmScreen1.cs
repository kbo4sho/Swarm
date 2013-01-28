using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SwarmEngine;

namespace XNASwarms
{
    class SwarmScreen1 : SwarmScreenBase
    {
        Texture2D swarmIndividual;

        public SwarmScreen1(string recipe, bool mutate)
            : base(recipe, mutate)
        {
            
        }

        public override void LoadContent()
        {            
            swarmIndividual = ScreenManager.Content.Load<Texture2D>("bee");
            base.LoadContent();
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            ScreenManager.SpriteBatch.Begin(0, null, null, null, null, null, Camera.View);
            base.Draw(gameTime);
            Population population = populationSimulator.getPopulation();

            foreach(Species spcs in population)
            {
                foreach(Individual indvd in spcs)
                {
                    ScreenManager.SpriteBatch.Draw(swarmIndividual, new Rectangle(
                        (int)indvd.getX(),
                        (int)indvd.getY(), 5, 5),
                        null,
                        indvd.getDisplayColor(),
                        0f,
                        new Vector2(0, 0),
                        SpriteEffects.None, 0);
                }
            }

            
            ScreenManager.SpriteBatch.End();
            
        }
    }
}
