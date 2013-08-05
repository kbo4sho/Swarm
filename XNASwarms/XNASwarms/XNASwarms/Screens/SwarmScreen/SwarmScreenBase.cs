using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using ScreenSystem.ScreenSystem;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
#if WINDOWS
using Microsoft.Surface.Core;
#endif
using Microsoft.Xna.Framework.Input;
using System.Collections;
using XNASwarms.Borders;
using XNASwarms.Borders.Walls;
using SwarmEngine;
using ScreenSystem.Debug;
using SwarmAnalysisEngine;
using XNASwarms.Emitters;
using XNASwarms.Screens;
#if NETFX_CORE
using XNASwarmsXAML.W8;
#endif


namespace XNASwarms
{
    public class SwarmScreenBase : ControlScreen
    {
        protected PopulationSimulator populationSimulator;
        EmitterManager emitterManager;
        Dictionary<int, Individual> Supers;
        public int width, height;
        Texture2D superAgentTexture;
        protected Border Border;
        private float TimePerFrame;
        private int FramesPerSec;
        private IDebugScreen debugScreen;
        
        List<Individual> SwarmInXOrder;
        AnalysisEngine analysisEngine;
        

        public SwarmScreenBase()
        {
            analysisEngine = new ClusterAnaylsisEngine();
            FramesPerSec = 30;
            TimePerFrame = (float)1 / FramesPerSec;
            ButtonSection = new ButtonSection(false, this, "");
            SwarmInXOrder = new List<Individual>();
        }

        public override void LoadContent()
        {
#if NETFX_CORE
            emitterManager = new EmitterManager(populationSimulator, ScreenManager.Game.Services.GetService(typeof(IAudio)) as IAudio);
#else
            emitterManager = new EmitterManager(populationSimulator);
#endif
            Camera = new SwarmsCamera(ScreenManager.GraphicsDevice);
            debugScreen = ScreenManager.Game.Services.GetService(typeof(IDebugScreen)) as IDebugScreen;
            debugScreen.ResetDebugItemsToNormal();
            width = ScreenManager.GraphicsDevice.Viewport.Width;
            height = ScreenManager.GraphicsDevice.Viewport.Height;
            
            superAgentTexture = ScreenManager.Content.Load<Texture2D>("Backgrounds/gray");
            
            Supers = new Dictionary<int, Individual>();

            Border = new Border(this, WallFactory.TopBottomPortal(ScreenManager.GraphicsDevice.Viewport.Width / 2, ScreenManager.GraphicsDevice.Viewport.Height / 2, 2), ScreenManager);
            
            Supers.Add(0, new Individual());

            base.LoadContent();
            
            for (int i = 0; i < populationSimulator.Population.Count(); i++)
            {
                debugScreen.AddDebugItem("SPECIES " + i.ToString("00") + " COUNT", populationSimulator.Population[i].Count().ToString(), ScreenSystem.Debug.DebugFlagType.Odd);
                debugScreen.AddDebugItem("SPECIES " + i.ToString("00"), populationSimulator.Population[i].First().Genome.getRecipe(), ScreenSystem.Debug.DebugFlagType.Odd);
            }

            debugScreen.AddDebugItem("SPECIES COUNT ", populationSimulator.Population.Count().ToString(), ScreenSystem.Debug.DebugFlagType.Important);
            debugScreen.AddDebugItem("RESOLUTION", width.ToString() + "x" + height.ToString(), ScreenSystem.Debug.DebugFlagType.Important);
            debugScreen.AddDebugItem("BORDER", Border.GetWallTypeAsText(), ScreenSystem.Debug.DebugFlagType.Important);
            
            debugScreen.AddSpacer();
        }

        

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            if (this.IsActive)
            {
                SwarmInXOrder = populationSimulator.GetSwarmInXOrder();
                //debugScreen.AddAnaysisResult(analysisEngine.Run(SwarmInXOrder, (float)gameTime.ElapsedGameTime.TotalSeconds, true));
                UpdateInput();
                populationSimulator.stepSimulation(Supers.Values.ToList<Individual>(), 10);
                Border.Update(SwarmInXOrder);
                
                Camera.Update(gameTime);
                //debugScreen.AddDebugItem("POPULATION COUNT", SwarmInXOrder.Count().ToString(), ScreenSystem.Debug.DebugFlagType.Important);

                debugScreen.AddDebugItem("Agent1 Dx: ", SwarmInXOrder[0].Dx.ToString(), ScreenSystem.Debug.DebugFlagType.Important);
            }
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            for (int i = 0; i < Supers.Count; i++)
            {
                ScreenManager.SpriteBatch.Draw(superAgentTexture, new Rectangle(
                    (int)Supers[i].X,
                    (int)Supers[i].Y, 6, 6),
                    null,
                    Color.Red,
                    0f,
                    new Vector2(3, 3),
                    SpriteEffects.None, 0);
            }

            Border.Draw(ScreenManager.SpriteBatch, Camera);

            base.Draw(gameTime);
        }

        public override void UnloadContent()
        {
            ButtonSection.Dispose();
            base.UnloadContent();
        }

        /// <summary>
        /// Decides how to handle the input based on the
        /// editing mode that is selected
        /// </summary>
        private void UpdateInput()
        {
            if (StaticEditModeParameters.IsBrushMode())
            {
                //Brush
                //TODO: need a static class for determining what the editing state is
                if (Supers.Count > 0)
                {
                    emitterManager.Update(new Vector2((float)Supers[0].X, (float)Supers[0].Y));
                }
                else
                {
                    emitterManager.Update(Vector2.Zero);
                }
            }
            else if (StaticEditModeParameters.IsHandMode() || StaticEditModeParameters.IsWorldMode())
            {
                emitterManager.Update(Vector2.Zero);
            }
           
        }

        public override void HandleInput(InputHelper input, Microsoft.Xna.Framework.GameTime gameTime)
        {
            //debugScreen.AddDebugItem("Dx & Dx2", populationSimulator.GetPopulation().First().First().getDx().ToString() + " & " + populationSimulator.GetPopulation().First().First().getDx2().ToString());
            Supers.Clear();
#if WINDOWS
            var surfacetouches = input.SurfaceTouches;
            if (surfacetouches.Count > 0)
            {
                //Surface
                for (int i = 0; i < surfacetouches.Count; i++)
                {
                    Vector2 position = Camera.ConvertScreenToWorldAndDisplayUnits(new Vector2(surfacetouches[i].X, surfacetouches[i].Y));

                    Supers[i] = new Individual(((double)position.X),
                         ((double)position.Y),
                         0.0, 0.0, new Parameters());
                }
            }
            else
#endif
            if (input.Touches.Count > 0)
            {
                //Everthing else touch
                for (int i = 0; i < input.Touches.Count; i++)
                {
                    Vector2 position = Camera.ConvertScreenToWorldAndDisplayUnits(new Vector2(input.Touches[i].Position.X, input.Touches[i].Position.Y));

                    Supers[i] = new Individual(i, ((double)position.X),
                         ((double)position.Y),
                         0.0, 0.0, new SuperParameters());
                }
            }
            else if (Supers.Count <= 0 &&
                     (input.Cursor != Vector2.Zero) &&
                     input.MouseState.LeftButton == ButtonState.Pressed)
            {
                //Here we should activate the emitters 
                //Mouse
                Supers.Add(0, new Individual());
                for (int i = 0; i < Supers.Count; i++)
                {
                    Vector2 position = Camera.ConvertScreenToWorldAndDisplayUnits(input.Cursor);

                    Supers[i] = new Individual(i, ((double)position.X),
                         ((double)position.Y),
                         0, 0, new SuperParameters());
                }
            }

            //if ((input.Cursor != Vector2.Zero) &&
            //             input.MouseState.LeftButton == ButtonState.Pressed)
            //{
            //    Supers.Add(0, new Individual());
            //    for (int i = 0; i < Supers.Count; i++)
            //    {
            //        Vector2 position = Camera.ConvertScreenToWorldAndDisplayUnits(input.Cursor);

            //        Supers[i] = new Individual(i, ((double)position.X),
            //             ((double)position.Y),
            //             0, 0, new SuperParameters());
            //    }
            //}

            ButtonSection.HandleInput(input, gameTime);

            base.HandleInput(input, gameTime);
            
        }

        public SaveSpecies GetPopulationAsSaveSpecies()
        {
            return SwarmSaveHelper.GetPopulationAsSaveSpecies(populationSimulator.GetSavablePopulation());
        }

        public void UpdatePopulation(string recipiText, bool mutate)
        {
            populationSimulator.UpdatePopulation(recipiText, mutate);
        }

        public void UpdatePopulation(Population population, bool mutate)
        {
            populationSimulator.UpdatePopulation(population, mutate);

            foreach (var species in population)
            {
                foreach(var spec in species)
                {
                    debugScreen.AddDebugItem("INDVD X", spec.X.ToString(), ScreenSystem.Debug.DebugFlagType.Important);
                }
            }
            
        }
    }
}
