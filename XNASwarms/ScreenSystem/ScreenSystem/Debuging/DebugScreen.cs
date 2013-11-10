using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ScreenSystem.ScreenSystem;
using SwarmAnalysisEngine;
using ScreenSystem.ScreenSystem.Debuging;

namespace ScreenSystem.Debug
{

    public interface IDebugScreen
    {
        void AddDebugItem(string label, string message, DebugFlagType flagtype);
        void AddDebugItem(string label, string message);
        void AddAnaysisResult(List<Analysis> analysisresult);
        void SetVisiblity();
    }


    public class DebugScreen : DrawableGameComponent, IDebugScreen
    {

        private IDebugComponent frameRateCounter;
        private List<DebugItem> DebugItems;

        private int itemSpacer = 10;
        private int panelPadding = 10;
        private int maxDebugItems;
        private ScreenManager screenManager;
        private Vector2 debugPanelTopLeft;
        private bool consoleVisible;

        public DebugScreen(ScreenManager screenmanager, bool visible)
            : this(screenmanager, visible, new FrameRateCounter(true))
        {
        }

        public DebugScreen(ScreenManager screenManager, bool visible, IDebugComponent frameRateCounter)
            : base(screenManager.Game)
        {
            this.frameRateCounter = frameRateCounter;
            this.screenManager = screenManager;
            consoleVisible = visible;
            
            DebugItems = new List<DebugItem>();
        }

        protected override void LoadContent()
        {
            debugPanelTopLeft = new Vector2(screenManager.GraphicsDevice.Viewport.Width - 300, 20);
            maxDebugItems = (screenManager.GraphicsDevice.Viewport.Height - (panelPadding * 2)) / itemSpacer;
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            frameRateCounter.Update(gameTime);
            RemoveOldDebugItems();
            base.Update(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            frameRateCounter.Draw(gameTime, screenManager);

            if (consoleVisible)
            {
                screenManager.SpriteBatch.Begin();

                for (int i = DebugItems.Count - 1; i >= 0; i -= 1)
                {
                    screenManager.SpriteBatch.DrawString(screenManager.Fonts.FrameRateCounterFont, DebugItems[i].GetFormatedMessage(),
                    new Vector2(debugPanelTopLeft.X + panelPadding, (debugPanelTopLeft.Y + itemSpacer * i) + panelPadding), DebugItems[i].GetColor());
                }

                screenManager.SpriteBatch.End();
            }

            base.Draw(gameTime);
        }

        private void AddDebugItemSpacer()
        {
            DebugItems.Insert(0, new DebugItem("///////////////////////////////////////", "//////////////", DebugFlagType.Important));
        }

        private void RemoveOldDebugItems()
        {
            if (DebugItems.Count > maxDebugItems)
            {
                DebugItems.RemoveRange(maxDebugItems, DebugItems.Count() - maxDebugItems);
            }
        }

        #region IDebugScreen
        public void AddDebugItem(string label, string message)
        {
            AddDebugItem(label, message, DebugFlagType.Normal);
        }

        public void AddDebugItem(string label, string message, DebugFlagType flagtype)
        {
            if (consoleVisible)
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
            if (consoleVisible)
            {
                foreach (Analysis analysis in analysisresult.Where(a=>a != null).ToList<Analysis>())
                {
                    if (this.consoleVisible)
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
                        //FilterResults.Add(analysis.FilterResult);
                    }
                }
            }
        }

        public void SetVisiblity()
        {
            consoleVisible = !consoleVisible;
        }
        #endregion

    }
}
