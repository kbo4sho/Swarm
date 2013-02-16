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

namespace XNASwarms
{
    public class SwarmScreenBase : ControlScreen
    {
        protected PopulationSimulator populationSimulator;
        
        Dictionary<int, Individual> Supers;
        public int width, height;
        Texture2D superAgentTexture;
        protected Border Border;
        private float TimePerFrame;
        private int FramesPerSec;
        private IDebugScreen debugScreen;
        AnalysisEngine analysisEngine;
        

        public SwarmScreenBase()
        {
            analysisEngine = new ClusterAnaylsisEngine();
            FramesPerSec = 14;
            TimePerFrame = (float)1 / FramesPerSec;
            ButtonSection = new ButtonSection(false, Vector2.Zero, this, "");
        }

        public override void LoadContent()
        {
            debugScreen = ScreenManager.Game.Services.GetService(typeof(IDebugScreen)) as IDebugScreen;

            width = ScreenManager.GraphicsDevice.Viewport.Width;
            height = ScreenManager.GraphicsDevice.Viewport.Height;

            Camera = new SwarmsCamera(ScreenManager.GraphicsDevice);
            superAgentTexture = ScreenManager.Content.Load<Texture2D>("Backgrounds/gray");
            
            Supers = new Dictionary<int, Individual>();

            Border = new Border(this, WallFactory.FourPortal(ScreenManager.GraphicsDevice.Viewport.Width / 2, ScreenManager.GraphicsDevice.Viewport.Height / 2, 2), ScreenManager);
            
            Supers.Add(0, new Individual());

            base.LoadContent();
            
            for (int i = 0; i < populationSimulator.getPopulation().Count(); i++)
            {
                debugScreen.AddDebugItem("SPECIES " + i.ToString("00") + " COUNT", populationSimulator.getPopulation()[i].Count().ToString(), ScreenSystem.Debug.DebugFlagType.Odd);
                debugScreen.AddDebugItem("SPECIES " + i.ToString("00"), populationSimulator.getPopulation()[i].First().getGenome().getRecipe(), ScreenSystem.Debug.DebugFlagType.Odd);
                
            }
            debugScreen.AddDebugItem("SPECIES COUNT ", populationSimulator.getPopulation().Count().ToString(), ScreenSystem.Debug.DebugFlagType.Important);
            debugScreen.AddDebugItem("RESOLUTION", width.ToString() + "x" + height.ToString(), ScreenSystem.Debug.DebugFlagType.Important);
            debugScreen.AddDebugItem("BORDER", Border.GetWallTypeAsText(), ScreenSystem.Debug.DebugFlagType.Important);
            
            debugScreen.AddSpacer();
        }

        

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            analysisEngine.Run();
            Border.Update(populationSimulator.getSwarmInBirthOrder().ToList<Individual>());
            populationSimulator.stepSimulation(Supers.Values.ToList<Individual>(), 20);   
            Camera.Update(gameTime);
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {

            for (int i = 0; i < Supers.Count; i++)
            {
                Vector2 position = Camera.ConvertScreenToWorldAndDisplayUnits(new Vector2((int)Supers[i].getX(), (int)Supers[i].getY()));

                ScreenManager.SpriteBatch.Draw(superAgentTexture, new Rectangle(
                    (int)Supers[i].getX(),
                    (int)Supers[i].getY(), 6, 6),
                    null,
                    Color.Red,
                    0f,
                    new Vector2(3, 3),
                    SpriteEffects.None, 0);
            }

            Border.Draw(ScreenManager.SpriteBatch, Camera);

            base.Draw(gameTime);
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

                    Supers[i] = new Individual(((double)position.X),
                         ((double)position.Y),
                         0.0, 0.0, new Parameters());
                }
            }
            else if (Supers.Count <= 0 && input.Cursor != Vector2.Zero)
            {
                //Mouse
                Supers.Add(0, new Individual());
                for (int i = 0; i < Supers.Count; i++)
                {
                    Vector2 position = Camera.ConvertScreenToWorldAndDisplayUnits(input.Cursor);

                    Supers[i] = new Individual(((double)position.X),
                         ((double)position.Y),
                         0,0, new Parameters());
                }
            }
            //TouchPanel.EnabledGestures = GestureType.Pinch;
            //while (TouchPanel.IsGestureAvailable)
            //{
            //    GestureSample gesture = TouchPanel.ReadGesture();

            //    switch (gesture.GestureType)
            //    {
            //        case GestureType.Pinch:
            //            float scaleFactor = PinchZoom.GetScaleFactor(gesture.Position, gesture.Position2,
            //                gesture.Delta, gesture.Delta2);
            //            //Vector2 panDelta = PinchZoom.GetTranslationDelta(gesture.Position, gesture.Position2,
            //            //    gesture.Delta, gesture.Delta2, spritePos, scaleFactor);
            //            Camera.Zoom *= scaleFactor;
            //            //spritePos += panDelta;
            //            break;
            //    }
            //}

            ButtonSection.HandleInput(input, gameTime);

            base.HandleInput(input, gameTime);
            
        }

        public Population GetPopulation()
        {
            return populationSimulator.getPopulation();
        }

        public SaveSpecies GetPopulationAsSaveSpecies()
        {
            SaveSpecies saveSpecies = new SaveSpecies();
            saveSpecies.CreadtedDt = DateTime.Now;
            foreach (Species species in populationSimulator.getPopulation())
            {
                saveSpecies.SavedSpecies.Add(GetSavedGenomes(species));
            }
            return saveSpecies;
        }

        private List<SaveGenome> GetSavedGenomes(Species species)
        {
            List<SaveGenome> savedGenomes = new List<SaveGenome>();
            foreach (Individual individual in species)
            {
                savedGenomes.Add(GetSavedGenomeFromIndividual(individual));
            }
            return savedGenomes;
        }

        private SaveGenome GetSavedGenomeFromIndividual(Individual individual)
        {
            SaveGenome savedGenome = new SaveGenome();
            savedGenome.neighborhoodRadius = individual.genome.getNeighborhoodRadius();
            savedGenome.normalSpeed = individual.genome.getNormalSpeed();
            savedGenome.maxSpeed = individual.genome.getMaxSpeed();
            savedGenome.c1 = individual.genome.getC1();
            savedGenome.c2 = individual.genome.getC2();
            savedGenome.c3 = individual.genome.getC3();
            savedGenome.c4 = individual.genome.getC4();
            savedGenome.c5 = individual.genome.getC5();
            return savedGenome;
        }
        //private void UpdateSupers(ReadOnlyTouchPointCollection touches, InputHelper input)
        //{
         

        //    for (int i = 0; i < touches.Count; i++)
        //    {
        //        Vector2 position = Camera.ConvertScreenToWorldAndDisplayUnits(new Vector2(touches[i].X,touches[i].Y));

        //        Supers[i] = new Individual(((double)position.X),
        //             ((double)position.Y),
        //             0.0, 0.0, new Parameters());
        //    }
        //}



        
    }
}
