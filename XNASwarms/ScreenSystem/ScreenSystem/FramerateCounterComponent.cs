using System;
using System.Globalization;
using Microsoft.Xna.Framework;
using ScreenSystem.ScreenSystem.Debuging;

namespace ScreenSystem.ScreenSystem
{
    /// <summary>
    /// Displays the FPS
    /// </summary>
    public class FrameRateCounter : IDebugComponent
    {
        private TimeSpan elapsedTime = TimeSpan.Zero;
        private NumberFormatInfo format;
        private int frameCounter;
        private int frameRate;
        private Vector2 position;
        private bool isVisible;

        public FrameRateCounter(bool isVisible)
        {
            format = new NumberFormatInfo();
            format.NumberDecimalSeparator = ".";
            this.isVisible = isVisible;
            position = new Vector2(30, 10);
        }

        public void Update(GameTime gameTime)
        {
            if (isVisible)
            {
                elapsedTime += gameTime.ElapsedGameTime;

                if (elapsedTime <= TimeSpan.FromSeconds(1)) return;

                elapsedTime -= TimeSpan.FromSeconds(1);
                frameRate = frameCounter;
                frameCounter = 0;
            }
        }

        public void Draw(GameTime gameTime, ScreenManager screenManager)
        {
            if (isVisible)
            {
                frameCounter++;

                string fps = string.Format(format, "{0} fps", frameRate);

                screenManager.SpriteBatch.Begin();
                screenManager.SpriteBatch.DrawString(screenManager.Fonts.FrameRateCounterFont, fps,
                                                      position, Color.LightBlue);
                screenManager.SpriteBatch.End();
            }
        }

        public void SetVisiblity()
        {
            isVisible = !isVisible;
        }
    }
}