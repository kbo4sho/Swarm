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

namespace XNASwarms
{

    public class App1 : Game
    {
        private readonly GraphicsDeviceManager graphics;

        private TouchTarget touchTarget;
        private Color backgroundColor = Color.Black;
        private bool applicationLoadCompleteSignalled;

        private UserOrientation currentOrientation = UserOrientation.Bottom;
        

        private Matrix screenTransform = Matrix.Identity;
        public int width, height, originalWidth, originalHeight;

        public SpriteFont font;

        private float TotalElapsed;
        private float TimePerFrame;
        private int FramesPerSec;

        ScreenManager screenManager;
        SwarmScreenBase swarmSreen;

        public App1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            ConvertUnits.SetDisplayUnitToSimUnitRatio(16f);
            TargetElapsedTime = TimeSpan.FromTicks(333333);
            
            
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

            // Get the window sized right.
            Program.InitializeWindow(Window, true);
            // Set the graphics device buffers.
            //graphics.PreferredBackBufferWidth = Program.WindowSize.Width;
            //graphics.PreferredBackBufferHeight = Program.WindowSize.Height;
            graphics.PreferredBackBufferWidth = 1680;
            graphics.PreferredBackBufferHeight = 1050;
            graphics.ApplyChanges();
            // Make sure the window is in the right location.
            Program.PositionWindow();
        }

        #endregion

        #region Game Methods

        /// <summary>
        /// Allows the app to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            originalWidth = originalHeight =  graphics.GraphicsDevice.Viewport.Width;
            width = graphics.GraphicsDevice.Viewport.Width / 2  ;
            height = graphics.GraphicsDevice.Viewport.Height / 2;
            IsMouseVisible = true; // easier for debugging not to "lose" mouse
            SetWindowOnSurface();
            screenManager = new ScreenManager(this);
            this.Components.Add(screenManager);
            // Set the application's orientation based on the orientation at launch
            currentOrientation = ApplicationServices.InitialOrientation;

            // Subscribe to surface window availability events
            ApplicationServices.WindowInteractive += OnWindowInteractive;
            ApplicationServices.WindowNoninteractive += OnWindowNoninteractive;

            base.Initialize();

            FramesPerSec = 30;
            TimePerFrame = (float)1 / FramesPerSec;
        }

        protected override void LoadContent()
        {
            var backgroundscreen = new BackgroundScreen();
            screenManager.AddScreen(backgroundscreen);

            swarmSreen = new SwarmScreenBase();
            screenManager.AddScreen(swarmSreen);


            //font = Content.Load<SpriteFont>("SpriteFont1");
            //swarmIndividual = Content.Load<Texture2D>("bee");
        }

        /// <summary>
        /// Allows the app to run logic such as updating the world,
        /// checking for collisions, gathering input and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (ApplicationServices.WindowAvailability != WindowAvailability.Unavailable)
            {
                float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

                #region FrameRate
                TotalElapsed += elapsed;
                if (TotalElapsed > TimePerFrame)
                {
                    //populationSimulator.stepSimulation(SuperList, 20);
                    TotalElapsed -= TimePerFrame;
                }
               
                #endregion   
            }

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
        }

        #endregion

        #region Application Event Handlers

        /// <summary>
        /// This is called when the user can interact with the application's window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowInteractive(object sender, EventArgs e)
        {
            //TODO: Enable audio, animations here

            //TODO: Optionally enable raw image here
        }

        /// <summary>
        /// This is called when the user can see but not interact with the application's window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowNoninteractive(object sender, EventArgs e)
        {
            //TODO: Disable audio here if it is enabled

            //TODO: Optionally enable animations here
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
                if (touchTarget != null)
                {
                    touchTarget.Dispose();
                    touchTarget = null;
                }
            }

            // Release unmanaged Resources.

            // Set large objects to null to facilitate garbage collection.

            base.Dispose(disposing);
        }

        #endregion
    }
}
