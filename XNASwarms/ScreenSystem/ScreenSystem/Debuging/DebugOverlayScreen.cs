using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SwarmAnalysisEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenSystem.ScreenSystem.Debuging
{
    public interface IDebugOverLayScreen
    {
        void AddAnaysisResult(List<Analysis> analysisresult);
        void SetVisiblity();
    }

    public class DebugOverlayScreen : DrawableGameComponent, IDebugOverLayScreen
    {
        private List<FilterResult> FilterResults;
        private bool ConsoleVisible;
        private ScreenManager screenManager;
        private Texture2D LineTexture;

        Color centerXColor;

        public DebugOverlayScreen(ScreenManager screenmanager, bool IsVisible)
            : base(screenmanager.Game)
        {
        }

        protected override void LoadContent()
        {
            LineTexture = screenManager.Content.Load<Texture2D>("centermarker");
            base.LoadContent();
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (ConsoleVisible)
            {
                DrawAnalysisFilters();
            }
        }

        private void DrawAnalysisFilters()
        {
            if (FilterResults != null)
            {
                for (int i = FilterResults.Count - 1; i >= 0; i -= 1)
                {
                    for (int c = 0; c < FilterResults[i].ClusterPoints.Count(); c++)
                    {
                        centerXColor = Color.Blue;
                        screenManager.SpriteBatch.Draw(LineTexture, FilterResults[i].ClusterPoints[c], null, centerXColor, 0, new Vector2(-(screenManager.GraphicsDevice.Viewport.Width / 2), -(screenManager.GraphicsDevice.Viewport.Height / 2)) + new Vector2(5, 5), Vector2.One, SpriteEffects.None, 0);
                    }
                }
                if (FilterResults.Count > 1)
                {
                    FilterResults.Clear();
                }
            }
        }

        public void AddAnaysisResult(List<Analysis> analysisresult)
        {
            throw new NotImplementedException();
        }

        public void SetVisiblity()
        {
            throw new NotImplementedException();
        }
    }

    
}
