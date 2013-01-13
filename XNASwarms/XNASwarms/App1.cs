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

namespace XNASwarms
{

    public class App1 : Game
    {
        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private TouchTarget touchTarget;
        private Color backgroundColor = Color.White;
        private bool applicationLoadCompleteSignalled;

        private UserOrientation currentOrientation = UserOrientation.Bottom;
        

        private Matrix screenTransform = Matrix.Identity;
        public int width, height, originalWidth, originalHeight;

        Texture2D swarmIndividual;
        PopulationSimulator populationSimulator;
        public SpriteFont font;
        Rectangle[] touchRects;
        Recipe[] recipes;

        TouchEventArgs touchEvent;
        Dictionary<int, Individual> Supers;
        Random rand;
        int[] randNumbers;
        int lastTime = 0;
        private float TotalElapsed;
        private float TimePerFrame;
        private int FramesPerSec;

        /// <summary>
        /// The target receiving all surface input for the application.
        /// </summary>
        protected TouchTarget TouchTarget
        {
            get { return touchTarget; }
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public App1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            //myPopulation = new PopulationSimulator(new Population(30, 300, 300, "Crumpulation"));
            recipes = new Recipe[1];
            recipes[0] = new Recipe(StockRecipies.Recipe1());
            //recipes[1] = new Recipe(StockRecipies.Recipe1());
            populationSimulator = new PopulationSimulator(400, 400 , recipes);
            Supers = new Dictionary<int, Individual>();
            //rand = new Random();
            //randNumbers = new int[10];
            //foreach (Individual ind in myPopulation.getPopulation())
            //{
            //    ind.getGenome().inducePointMutations(rand.NextDouble(),.5);
            //}
            
            
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
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            graphics.ApplyChanges();
            // Make sure the window is in the right location.
            Program.PositionWindow();
        }

        /// <summary>
        /// Initializes the surface input system. This should be called after any window
        /// initialization is done, and should only be called once.
        /// </summary>
        private void InitializeSurfaceInput()
        {
            System.Diagnostics.Debug.Assert(Window != null && Window.Handle != IntPtr.Zero,
                "Window initialization must be complete before InitializeSurfaceInput is called");
            if (Window == null || Window.Handle == IntPtr.Zero)
                return;
            System.Diagnostics.Debug.Assert(touchTarget == null,
                "Surface input already initialized");
            if (touchTarget != null)
                return;

            // Create a target for surface input.
            touchTarget = new TouchTarget(Window.Handle, EventThreadChoice.OnBackgroundThread);
            touchTarget.EnableInput();
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
            width = graphics.GraphicsDevice.Viewport.Width / 2;
            height = graphics.GraphicsDevice.Viewport.Height / 2;
            IsMouseVisible = true; // easier for debugging not to "lose" mouse
            SetWindowOnSurface();
            InitializeSurfaceInput();

            // Set the application's orientation based on the orientation at launch
            currentOrientation = ApplicationServices.InitialOrientation;

            // Subscribe to surface window availability events
            ApplicationServices.WindowInteractive += OnWindowInteractive;
            ApplicationServices.WindowNoninteractive += OnWindowNoninteractive;
            ApplicationServices.WindowUnavailable += OnWindowUnavailable;

            // Setup the UI to transform if the UI is rotated.
            // Create a rotation matrix to orient the screen so it is viewed correctly
            // when the user orientation is 180 degress different.
            //Matrix inverted = Matrix.CreateRotationZ(MathHelper.ToRadians(180)) *
            //           Matrix.CreateTranslation(graphics.GraphicsDevice.Viewport.Width,
            //                                     graphics.GraphicsDevice.Viewport.Height,
            //                                     0);

            //if (currentOrientation == UserOrientation.Top)
            //{
            //    screenTransform = inverted;
            //}

            base.Initialize();
            //touchTarget.TouchDown += new EventHandler<TouchEventArgs>(this.SpawnSuper);
            //touchTarget.TouchUp += new EventHandler<TouchEventArgs>(this.DestroySuper);

            FramesPerSec = 30;
            TimePerFrame = (float)1 / FramesPerSec;

        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("SpriteFont1");
            swarmIndividual = Content.Load<Texture2D>("bee");
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
                //if (ApplicationServices.WindowAvailability == WindowAvailability.Interactive)
                //{
                //    // TODO: Process touches, 
                //    // use the following code to get the state of all current touch points.
                //    ReadOnlyTouchPointCollection touchPoints = touchTarget.GetState();
                //    for (int i = 0; i < touchPoints.Count; i++)
                //    {
                //        //if(Supers[i] != null)
                //        //Supers[i].SetPosition(touchPoints.GetTouchPointFromId(Supers[i].TouchID));
                //        //Supers.ElementAt(i).Value.SetPosition(touchPoints[i]);
                //        if (Supers.ElementAt(i).Value != null)
                //        {
                //            //error here
                //            Supers.ElementAt(i).Value.SetPosition(touchPoints.GetTouchPointFromId(Supers.ElementAt(i).Key));
                //        }
                //    }
                //}
                List<Individual> SuperList = new List<Individual>();
                //for (int i = 0; i < Supers.Count; i++)
                //{
                //    SuperList.Add(Supers.ElementAt(i).Value);
                //}
                float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

                #region FrameRate
                TotalElapsed += elapsed;
                if (TotalElapsed > TimePerFrame)
                {
                    populationSimulator.stepSimulation(SuperList, 20);
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
            int max, x, y;

            if (!applicationLoadCompleteSignalled)
            {
                // Dismiss the loading screen now that we are starting to draw
                ApplicationServices.SignalApplicationLoadComplete();
                applicationLoadCompleteSignalled = true;
            }

            //TODO: Rotate the UI based on the value of screenTransform here if desired
           

            GraphicsDevice.Clear(backgroundColor);
            spriteBatch.Begin();

            //Draw User Touch Areas
            //if (touchRects != null && touchRects.Length > 0)
            //{
            //    foreach (Rectangle r in touchRects)
            //    {
            //        spriteBatch.Draw(swarmIndividual, r, Color.White);
            //    }
            //}

            Population population = populationSimulator.getPopulation();

            if ((max = population.size()) == 0)
            {
                //FIX IF WE GET HERE
                //redraw();
                return;
            }            

            for(int i = 0; i < populationSimulator.getPopulation().Count; i++)
            {
                spriteBatch.Draw(swarmIndividual, new Rectangle(
                    (int)population[i].getX() + width,
                    (int)population[i].getY() + height, 4, 4),
                    null,
                    population[i].getDisplayColor(),
                    0f,
                    new Vector2(2, 2),
                    SpriteEffects.None, 0);
            }

            spriteBatch.End();


            //TODO: Add your drawing code here
            //TODO: Avoid any expensive logic if application is neither active nor previewed

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

        /// <summary>
        /// This is called when the application's window is not visible or interactive.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowUnavailable(object sender, EventArgs e)
        {
            //TODO: Disable audio, animations here

            //TODO: Disable raw image if it's enabled
        }

        //public void SpawnSuper(object sender, TouchEventArgs e)
        //{
        //    Individual super = new Individual(10, 10, 10, 10, new Parameters());
        //    myPopulation.getPopulation().Add(super);
        //    super.TouchID = e.TouchPoint.Id;
        //    super.super = true;
        //    Supers.Add(e.TouchPoint.Id, super);
        //}

        public void DestroySuper(object sender, TouchEventArgs e)
        {
            try
            {
                if (Supers[e.TouchPoint.Id] != null) //So it's not a null problem. The dictionary entry does not exist... :/ Create it if it doesn't?
                {
                    populationSimulator.removeIndividual(Supers[e.TouchPoint.Id]);
                    for (int i = 0; i < Supers.Count; i++)
                    {
                        if (Supers.ElementAt(i).Key == e.TouchPoint.Id)
                        {
                            Supers.Remove(Supers.ElementAt(i).Key);
                            break;
                        }
                    }
                }
            }
            catch { }
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
