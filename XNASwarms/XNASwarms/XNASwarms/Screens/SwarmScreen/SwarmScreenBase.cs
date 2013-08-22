using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using ScreenSystem.ScreenSystem;
using Microsoft.Xna.Framework.Graphics;

#if WINDOWS
using Microsoft.Surface.Core;
#endif
#if NETFX_CORE

#endif
using Microsoft.Xna.Framework.Input;
using SwarmEngine;
using ScreenSystem.Debug;
using XNASwarms.Emitters;
using XNASwarms.Screens.Borders;
using XNASwarms.Analysis.Components;
using XNASwarms.Screens.UI;
using XNASwarms.Saving;


namespace XNASwarms.Screens.SwarmScreen
{
    public class SwarmScreenBase : ControlScreen
    {
        PopulationSimulator populationSimulator;
        IEmitterComponent emitterComponent;
        IAnalysisComponent analysisComponent;
        Dictionary<int, Individual> Supers;
        Texture2D individualTexture, bigIndividualTexture, superAgentTexture;
        protected Border Border;
        private IDebugScreen debugComponent;
        List<Individual> swarmInXOrder;

        public SwarmScreenBase(IEmitterComponent emitterComponent, IAnalysisComponent analysisComponent, PopulationSimulator populationSimulator)
        {
            ButtonSection = new ButtonSection(false, this, "");
            swarmInXOrder = new List<Individual>();
            Supers = new Dictionary<int, Individual>();
            this.emitterComponent = emitterComponent;
            this.populationSimulator = populationSimulator;
            this.analysisComponent = analysisComponent;
        }

        public override void LoadContent()
        {
            individualTexture = ScreenManager.Content.Load<Texture2D>("point");
            bigIndividualTexture = ScreenManager.Content.Load<Texture2D>("beebig");
            superAgentTexture = ScreenManager.Content.Load<Texture2D>("Backgrounds/gray");
            
            debugComponent = ScreenManager.Game.Services.GetService(typeof(IDebugScreen)) as IDebugScreen;
            
            Camera = new SwarmsCamera(ScreenManager.GraphicsDevice);
            Border = new Border(ScreenManager);
            base.LoadContent();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            if (this.IsActive)
            {
                swarmInXOrder = populationSimulator.GetSwarmInXOrder();
                
                foreach(var indvd in swarmInXOrder.OrderBy(i => i.ID))
                {
                    //THIS CHECK FOR DUPLICATE IDS IN THE POPULATION
                    //if (populationSimulator.Population.SelectMany(s => s.GroupBy(i => i).Where(d => d.Count() > 1)).Count() > 0)
                    //{
                    //    debugComponent.AddDebugItem("AGENT ID", indvd.ID.ToString());
                    //}

                    debugComponent.AddDebugItem("AGENT ID", indvd.ID.ToString());
                }

                analysisComponent.Update(swarmInXOrder, gameTime);
                Border.Update(swarmInXOrder);
                emitterComponent.UpdateInput(Supers);

                if (StaticEditModeParameters.IsEraseMode() ||
                    StaticEditModeParameters.IsBrushMode())
                {
                    populationSimulator.stepSimulation(Supers.Values.ToList<Individual>(), 0);
                }
                else
                {
                    populationSimulator.stepSimulation(Supers.Values.ToList<Individual>(), 20);
                }
               
                Camera.Update(gameTime);
            }
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            ScreenManager.SpriteBatch.Begin(0, null, null, null, null, null, Camera.View);

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

            for (int s = 0; s < populationSimulator.Population.Count; s++)
            {
                for (int i = 0; i < populationSimulator.Population[s].Count; i++)
                {
                    DrawIndividual(populationSimulator.Population[s][i], populationSimulator.Population[s][i].getDisplayColor(), individualTexture, populationSimulator.Population[s][i].EmitterType);
                }
            }

            Border.Draw(ScreenManager.SpriteBatch, Camera);

            ScreenManager.SpriteBatch.End();
            base.Draw(gameTime);
        }

        private void DrawIndividual(Individual indvd, Color color, Texture2D texture, EmitterActionType type)
        {
            if (!indvd.IsMobile)
            {
                ScreenManager.SpriteBatch.Draw(bigIndividualTexture, new Rectangle(
                    (int)indvd.X,
                    (int)indvd.Y, bigIndividualTexture.Width, bigIndividualTexture.Height),
                    null,
                    color,
                    (float)Math.Atan2(indvd.Dy, indvd.Dx),
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
                    (float)Math.Atan2(indvd.Dy, indvd.Dx),
                    new Vector2(texture.Width / 2, texture.Height / 2),
                    SpriteEffects.None, 0);
            }

        }

        public override void HandleInput(InputHelper input, Microsoft.Xna.Framework.GameTime gameTime)
        {
            Supers.Clear();
#if WINDOWS
            var surfacetouches = input.SurfaceTouches;
            if (surfacetouches.Count > 0)
            {
                //Surface
                for (int i = 0; i < surfacetouches.Count; i++)
                {
                    Vector2 position = Camera.ConvertScreenToWorldAndDisplayUnits(new Vector2(surfacetouches[i].X, surfacetouches[i].Y));

                    Supers[i] = new Individual(i,((double)position.X),
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

            if (input.IsNewKeyPress(Keys.A))
            {
                analysisComponent.SetVisiblity();
            }

            if (input.IsNewKeyPress(Keys.C))
            {
                debugComponent.SetVisiblity();
            }

            ButtonSection.HandleInput(input, gameTime);
            base.HandleInput(input, gameTime);
        }

        public SaveSpecies GetPopulationAsSaveSpecies()
        {
            return SwarmSaveHelper.GetPopulationAsSaveSpecies(populationSimulator.Population);
        }

        public void UpdatePopulation(string recipiText, bool mutate)
        {
            emitterComponent.BatchEmit(new Population(new Recipe(recipiText).CreatePopulation(0, 0), "CrumpulaAtion"), mutate);
        }

        public void UpdatePopulation(Population population, bool mutate)
        {
            emitterComponent.BatchEmit(population, mutate);

            foreach (var species in population)
            {
                foreach (var spec in species)
                {
                    debugComponent.AddDebugItem("INDVD X", spec.X.ToString(), ScreenSystem.Debug.DebugFlagType.Important);
                }
            }
        }
    }
}
