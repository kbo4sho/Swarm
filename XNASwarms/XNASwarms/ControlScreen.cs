using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ScreenSystem.ScreenSystem;


namespace XNASwarms
{
    public class ControlScreen : GameScreen
    {

        public SwarmsCamera Camera;
        protected ButtonSection ButtonSection;
        
        public ControlScreen()
            : base()
        {
            ButtonSection = new ButtonSection(false, Vector2.Zero, this, "");
        }

        public override void LoadContent()
        {
            ButtonSection.Load();
            base.LoadContent();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            ButtonSection.Update();
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void Draw(GameTime gameTime)
        {
            ButtonSection.Draw(gameTime);
            base.Draw(gameTime);
        }

        public override void HandleInput(InputHelper input, GameTime gameTime)
        {
            HandleCamera(input, gameTime);
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