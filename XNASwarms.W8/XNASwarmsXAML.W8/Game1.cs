using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ScreenSystem.Debug;
using ScreenSystem.ScreenSystem;
using SwarmEngine;
using System;
using VSS;
using XNASwarms;
using XNASwarms.Emitters;

namespace XNASwarmsXAML.W8
{

    public class Game1 : Game
    {
        GraphicsDeviceManager _graphics;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            ScreenManager screenManager = new ScreenManager(this);
            this.Components.Add(screenManager);

            var backgroundscreen = new BackgroundScreen();
            screenManager.AddScreen(backgroundscreen);

            var debugScreen = new DebugScreen(screenManager, false);
            screenManager.Game.Components.Add(debugScreen);
            this.Services.AddService(typeof(IDebugScreen), debugScreen);

            var audioScreen = new Audio();
            screenManager.AddScreen(audioScreen);
            this.Services.AddService(typeof(IAudio), audioScreen);

            Recipe[] recipes = new Recipe[1];
            recipes[0] = new Recipe(StockRecipies.Stable_A);
            PopulationSimulator populationSimulator = new PopulationSimulator(0, 0, recipes);

            SwarmScreen1 swarmScreen = new SwarmScreen1(new SwarmEmitterComponent(populationSimulator), populationSimulator);
            screenManager.AddScreen(swarmScreen);

#if NETFX_CORE
            ControlClient controlClient = new ControlClient(swarmScreen, this.Services.GetService(typeof(IAudio)) as IAudio);
#else
            ControlClient controlClient = new ControlClient(swarmScreen));
#endif     
            this.Services.AddService(typeof(IControlClient), controlClient);

            
            base.Initialize();
        }

        protected override void LoadContent()
        {
           
        }

        protected override void UnloadContent()
        {
           
        }
   
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
