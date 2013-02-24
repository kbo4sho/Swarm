using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ScreenSystem.Debug;
using ScreenSystem.ScreenSystem;
using SwarmEngine;
using System;
using XNASwarms;

namespace XNASwarms.W8
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager _graphics;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
#if NETFX_ARM
            this.TargetElapsedTime = TimeSpan.FromSeconds(1.0f / 30.0f);
#endif
        }

        protected override void Initialize()
        {
            ScreenManager screenManager = new ScreenManager(this);
            this.Components.Add(screenManager);

            var backgroundscreen = new BackgroundScreen();
            screenManager.AddScreen(backgroundscreen);

            var debugScreen = new DebugScreen(screenManager, true);
            screenManager.Game.Components.Add(debugScreen);
            this.Services.AddService(typeof(IDebugScreen), debugScreen);

            SwarmScreen1 swarmScreen = new SwarmScreen1(StockRecipies.Stable_A, false);
            screenManager.AddScreen(swarmScreen);
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
