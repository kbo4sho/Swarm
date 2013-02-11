using System;
using System.Globalization;
using Microsoft.Xna.Framework;

namespace ScreenSystem.ScreenSystem
{
    /// <summary>
    /// Displays the FPS
    /// </summary>
    public class FrameRateCounter : DrawableGameComponent
    {
        private TimeSpan _elapsedTime = TimeSpan.Zero;
        private NumberFormatInfo _format;
        private int _frameCounter;
        private int _frameRate;
        private Vector2 _position;
        private ScreenManager _screenManager;
        private bool IsVisible;

        public FrameRateCounter(ScreenManager screenManager, bool visible)
            : base(screenManager.Game)
        {
            _screenManager = screenManager;
            _format = new NumberFormatInfo();
            _format.NumberDecimalSeparator = ".";
            IsVisible = visible;
#if XBOX
            _position = new Vector2(55, 35);
#else
            _position = new Vector2(30, 10);
#endif
        }

        public override void Update(GameTime gameTime)
        {
            if (IsVisible)
            {
                _elapsedTime += gameTime.ElapsedGameTime;

                if (_elapsedTime <= TimeSpan.FromSeconds(1)) return;

                _elapsedTime -= TimeSpan.FromSeconds(1);
                _frameRate = _frameCounter;
                _frameCounter = 0;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if (IsVisible)
            {
                _frameCounter++;

                string fps = string.Format(_format, "{0} fps", _frameRate);

                _screenManager.SpriteBatch.Begin();
                _screenManager.SpriteBatch.DrawString(_screenManager.Fonts.FrameRateCounterFont, fps,
                                                      _position, Color.LightBlue);
                _screenManager.SpriteBatch.End();
            }
            base.Draw(gameTime);
        }

        public void SetVisiblity()
        {
            IsVisible = !IsVisible;
        }
    }
}