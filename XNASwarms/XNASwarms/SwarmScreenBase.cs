using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using ScreenSystem.ScreenSystem;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Surface.Core;
using Microsoft.Xna.Framework.Input;
using System.Collections;

namespace XNASwarms
{
    class SwarmScreenBase : GameScreen
    {
        Recipe[] recipes;
        PopulationSimulator populationSimulator;
        Random rand;
        Dictionary<int, Individual> Supers;
        public int width, height;
        Texture2D swarmIndividual;
        SwarmsCamera Camera;

        public SwarmScreenBase()
        {
            recipes = new Recipe[1];
            recipes[0] = new Recipe(StockRecipies.Recipe1());
        }

        public override void LoadContent()
        {
            width = ScreenManager.GraphicsDevice.Viewport.Width/2;
            height = ScreenManager.GraphicsDevice.Viewport.Height;
            swarmIndividual = ScreenManager.Content.Load<Texture2D>("bee");
            Camera = new SwarmsCamera(ScreenManager.GraphicsDevice);

            populationSimulator = new PopulationSimulator(600, 200, recipes);
            Supers = new Dictionary<int, Individual>();
            rand = new Random();

            foreach (Individual ind in populationSimulator.getPopulation())
            {
                ind.getGenome().inducePointMutations(rand.NextDouble(), 2);
                //ind.getGenome().inducePointMutations(rand.NextDouble(), 3);
            }
            Supers.Add(0, new Individual());

            base.LoadContent();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            populationSimulator.stepSimulation(Supers.Values.ToList<Individual>(), 20);
            Camera.Update(gameTime);
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            ScreenManager.SpriteBatch.Begin(0, null, null, null, null, null, Camera.View);
            Population population = populationSimulator.getPopulation();
            for (int i = 0; i < populationSimulator.getPopulation().Count; i++)
            {
                Vector2 position = Camera.ConvertScreenToWorldAndDisplayUnits(new Vector2((int)population[i].getX(),(int)population[i].getY()));

                ScreenManager.SpriteBatch.Draw(swarmIndividual, new Rectangle(
                    (int)population.get(i).getX(),
                    (int)population.get(i).getY(), 5, 5),
                    null,
                    population[i].getDisplayColor(),
                    0f,
                    new Vector2(0, 0),
                    SpriteEffects.None, 0);
            }
            ScreenManager.SpriteBatch.End();
            base.Draw(gameTime);
        }

        public override void HandleInput(InputHelper input, Microsoft.Xna.Framework.GameTime gameTime)
        {
            Supers.Clear();
            
            var surfacetouches = input.SurfaceTouches;
            if (surfacetouches.Count > 0)
            {
                //Surface
                UpdateSupers(surfacetouches.ToList(), input);
            }
            else if (input.Touches.Count > 0)
            {
                //Everthing else touch
                UpdateSupers(input.Touches.ToList(), input);
            }
            else
            {
                //Mouse
                Supers.Add(0, new Individual());
                UpdateSupers(Supers.ToList(),input);
            }

            HandleCamera(input, gameTime);
            
        }

        private void UpdateSupers(IList touches, InputHelper input)
        {
            Vector2 position = Camera.ConvertScreenToWorldAndDisplayUnits(input.Cursor);

            for (int i = 0; i < touches.Count; i++)
            {
                Supers[i] = new Individual(((double)(position.X)),
                     ((double)(position.Y)),
                     0.0, 0.0, new Parameters());
            }
        }

        private void HandleCamera(InputHelper input, GameTime gameTime)
        {
            Vector2 camMove = Vector2.Zero;
            float MoveSpeed = 100f;

            if (input.KeyboardState.IsKeyDown(Keys.Up))
            {
                camMove.Y -= MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (input.KeyboardState.IsKeyDown(Keys.Down))
            {
                camMove.Y += MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (input.KeyboardState.IsKeyDown(Keys.Left))
            {
                camMove.X -= MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (input.KeyboardState.IsKeyDown(Keys.Right))
            {
                camMove.X += MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (camMove != Vector2.Zero)
            {
                Camera.MoveCamera(camMove);
            }
            if (input.KeyboardState.IsKeyDown(Keys.OemPlus))
            {
                Camera.Zoom += 5f * (float)gameTime.ElapsedGameTime.TotalSeconds * Camera.Zoom / 20f;
            }
            if (input.KeyboardState.IsKeyDown(Keys.OemMinus))
            {
                Camera.Zoom -= 5f * (float)gameTime.ElapsedGameTime.TotalSeconds * Camera.Zoom / 20f;
            }
            if (input.IsNewKeyPress(Keys.Escape))
            {
                Camera.ResetCamera();
            }
        }
    }
}
