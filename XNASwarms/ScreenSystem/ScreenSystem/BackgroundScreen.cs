using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ScreenSystem.ScreenSystem
{
    public class BackgroundScreen : GameScreen
    {
        private const float LogoScreenHeightRatio = 0.25f;
        private const float LogoScreenBorderRatio = 0.0375f;
        private const float LogoWidthHeightRatio = 1.4f;

        private Texture2D _backgroundTexture;
        private Rectangle _logoDestination;
        private Texture2D _logoTexture;
        private Rectangle _viewport;

        public BackgroundScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public override void LoadContent()
        {
            _logoTexture = ScreenManager.Content.Load<Texture2D>("Backgrounds/logo");
            _backgroundTexture = ScreenManager.Content.Load<Texture2D>("Backgrounds/black");

            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Vector2 logoSize = new Vector2();
            logoSize.Y = _logoTexture.Width;
            logoSize.X = _logoTexture.Height;

            float border = viewport.Height * LogoScreenBorderRatio;
            Vector2 logoPosition = new Vector2(logoSize.X,
                                               viewport.Height - border - logoSize.Y*1.25f);
            _logoDestination = new Rectangle((int)logoPosition.X, (int)logoPosition.Y, (int)logoSize.X,
                                             (int)logoSize.Y);
            _viewport = viewport.Bounds;

        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.Draw(_backgroundTexture, _viewport, Color.White);
            ScreenManager.SpriteBatch.Draw(_logoTexture, _logoDestination, Color.White * 0.6f);
            ScreenManager.SpriteBatch.End();
        }
    }
}