using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Surface;
using Microsoft.Surface.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using ScreenSystem.ScreenSystem;
using ScreenSystem.Debug;
using SwarmEngine;
using SwarmAudio;
using SwarmAnalysisEngine;

namespace XNASwarms
{

    public class App1 : Game
    {
        private readonly GraphicsDeviceManager graphics;

        public App1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            ConvertUnits.SetDisplayUnitToSimUnitRatio(1f);
            //TargetElapsedTime = TimeSpan.FromTicks(167777);
            //this.TargetElapsedTime = TimeSpan.FromSeconds(1.0f / 30.0f);   
        }

        #region Initialization

        /// <summary>
        /// Moves and sizes the window to cover the input surface.
        /// </summary>
        private void SetWindowOnSurface()
        {
            System.Diagnostics.Debug.Assert(Window != null && Window.Handle != IntPtr.Zero,
                "Window initialization must be complete before SetWindowOnSurface is called");
            if (Window == null || Window.Handle == IntPtr.Zero)
                return;

            //graphics.PreferredBackBufferWidth = 1920;
            //graphics.PreferredBackBufferHeight = 1080;
            //graphics.PreferredBackBufferWidth = 1680;
            //graphics.PreferredBackBufferHeight = 1050;
            //graphics.PreferredBackBufferWidth = 1600;
            //graphics.PreferredBackBufferHeight = 900;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 768;
            //graphics.IsFullScreen = true;
            graphics.ApplyChanges();
            //graphics.ToggleFullScreen();

            Normalizer.Width = GraphicsDevice.Viewport.Width;
            Normalizer.Height = GraphicsDevice.Viewport.Height;
        }

        #endregion

        #region Game Methods

        protected override void Initialize()
        {
            SetWindowOnSurface();

            ScreenManager screenManager = new ScreenManager(this);
            this.Components.Add(screenManager);
            
            var backgroundscreen = new BackgroundScreen();
            screenManager.AddScreen(backgroundscreen);

            var debugScreen = new DebugScreen(screenManager,false);
            screenManager.Game.Components.Add(debugScreen);
            this.Services.AddService(typeof(IDebugScreen), debugScreen);

            SwarmScreen1 swarmScreen = new SwarmScreen1(StockRecipies.Stable_A, false);
            screenManager.AddScreen(swarmScreen);

            base.Initialize();
            SoundEngine.Init();

        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the app should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        /// <summary>
        /// UnloadContent will be called once per app and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            base.UnloadContent();
        }

        #endregion

        #region IDisposable

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Release managed resources.
                IDisposable graphicsDispose = graphics as IDisposable;
                if (graphicsDispose != null)
                {
                    graphicsDispose.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
