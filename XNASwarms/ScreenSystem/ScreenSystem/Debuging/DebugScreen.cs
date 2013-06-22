using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ScreenSystem.ScreenSystem;
using SwarmAnalysisEngine;

namespace ScreenSystem.Debug
{

    public interface IDebugScreen
    {
        void AddDebugItem(string label, string message, DebugFlagType flagtype);
        void AddDebugItem(string label, string message);
        void AddAnaysisResult(List<Analysis> analysisresult);
        void SetVisiblity();
        void ResetDebugItemsToNormal();
        void AddSpacer();
    }


    public class DebugScreen : DrawableGameComponent, IDebugScreen
    {
        private List<DebugItem> DebugItems;

        private List<FilterResult> FilterResults;
        private Texture2D LineTexture;

        private Rectangle DebugPanelRectangle;
        private Texture2D PanelTexture;
        private ScreenManager screenManager;
        FrameRateCounter frameratecounter;
        private int itemSpacer;
        private int PanelPadding;
        private int MaxDebugItems;

        Color centerXColor;
        private bool ConsoleVisible;


        public DebugScreen(ScreenManager screenmanager, bool visible)
            : base(screenmanager.Game)
        {
            DebugItems = new List<DebugItem>();
            FilterResults = new List<FilterResult>();
            screenManager = screenmanager;
            ConsoleVisible = visible;
        }

        protected override void LoadContent()
        {
            frameratecounter = new FrameRateCounter(screenManager, ConsoleVisible);
            screenManager.Game.Components.Add(frameratecounter);
            LineTexture = screenManager.Content.Load<Texture2D>("centermarker");
            PanelTexture = screenManager.Content.Load<Texture2D>("Backgrounds/gray");
            itemSpacer = 10;
            PanelPadding = 10;
            DebugPanelRectangle = new Rectangle(10, 10, 280, screenManager.GraphicsDevice.Viewport.Height - 50);
            MaxDebugItems = (DebugPanelRectangle.Height - (PanelPadding * 2)) / itemSpacer;
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (ConsoleVisible)
            {
                Vector2 largestStringSize = new Vector2(100, 20);//screenManager.Fonts.FrameRateCounterFont.MeasureString(DebugItems.OrderBy(s => s.GetFormatedMessage().Length).Last().GetFormatedMessage().ToString());
                DebugPanelRectangle.Width = (int)largestStringSize.X + PanelPadding * 3;
                DebugPanelRectangle.Height = (itemSpacer * DebugItems.Count) + PanelPadding * 3;

                if (DebugItems.Count > MaxDebugItems)
                {
                    List<DebugItem> tempItems = new List<DebugItem>();
                    foreach (var item in DebugItems.Skip(MaxDebugItems - DebugItems.Where(d => d.GetFlagType() != DebugFlagType.Normal).Count()))
                    {
                        if (item.GetFlagType() == DebugFlagType.Normal)
                        {
                            tempItems.Add(item);
                        }
                    }

                    foreach (var temp in tempItems)
                    {
                        DebugItems.Remove(temp);
                    }
                }
            }
            base.Update(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (ConsoleVisible)
            {
                screenManager.SpriteBatch.Begin();
                screenManager.SpriteBatch.Draw(PanelTexture, DebugPanelRectangle, new Color(20, 20, 20, 170));

                for (int i = DebugItems.Count - 1; i >= 0; i -= 1)
                {
                    screenManager.SpriteBatch.DrawString(screenManager.Fonts.FrameRateCounterFont, DebugItems[i].GetFormatedMessage(),
                                                          new Vector2(DebugPanelRectangle.X + PanelPadding, (DebugPanelRectangle.Y + itemSpacer * i) + PanelPadding), DebugItems[i].GetColor());
                }

                DrawAnalysisFilters();

                screenManager.SpriteBatch.End();
            }

            base.Draw(gameTime);
        }

        private void DrawAnalysisFilters()
        {
            if (FilterResults != null)
            {
                for (int i = FilterResults.Count - 1; i >= 0; i -= 1)
                {
                    for (int c = 0; c < FilterResults[i].ClusterPoints.Count(); c++ )
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

        private void AddDebugItemSpacer()
        {
            DebugItems.Insert(0, new DebugItem("///////////////////////////////////////", "//////////////", DebugFlagType.Important));
        }

        #region IDebugScreen
        public void AddDebugItem(string label, string message)
        {
            AddDebugItem(label, message, DebugFlagType.Normal);
        }

        public void AddDebugItem(string label, string message, DebugFlagType flagtype)
        {
            if (ConsoleVisible)
            {
                switch (flagtype)
                {
                    case DebugFlagType.Normal:
                        DebugItems.Insert(0, new DebugItem(label, message, flagtype));
                        break;
                    case DebugFlagType.Odd:
                        DebugItems.Insert(0, new DebugItem(label, message, flagtype));
                        break;
                    case DebugFlagType.Important:
                        DebugItems.Insert(0, new DebugItem(label, message, flagtype));
                        break;
                }
            }
        }

        public void AddAnaysisResult(List<Analysis> analysisresult)
        {
            if (ConsoleVisible)
            {
                foreach (Analysis analysis in analysisresult)
                {
                    if (this.ConsoleVisible)
                    {
                        if (analysis.Messages != null)
                        {
                            foreach (AnalysisMessage message in analysis.Messages)
                            {
                                AddDebugItem(message.Type, message.Message, DebugFlagType.Normal);
                            }
                        }
                    }

                    if (analysis.FilterResult != null)
                    {
                        FilterResults.Add(analysis.FilterResult);
                    }
                }
            }
        }

        public void SetVisiblity()
        {
            frameratecounter.SetVisiblity();
            ConsoleVisible = !ConsoleVisible;
        }

        public void AddSpacer()
        {
            AddDebugItemSpacer();
        }

        public void ResetDebugItemsToNormal()
        {
            foreach (DebugItem item in DebugItems.Where(i=>i.GetFlagType() != DebugFlagType.Normal))
            {
               item.ResetFlag();
            }
        }
        #endregion

    }
}
