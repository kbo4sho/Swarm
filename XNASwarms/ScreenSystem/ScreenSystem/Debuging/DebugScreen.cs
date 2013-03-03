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
        void AddAnaysisResult(List<AnalysisResult> analysisresult);
        void AddAnalysisFilterData(List<FilterResult> filterresult);
        void SetVisiblity();
        void ResetDebugItemsToNormal();
        void AddSpacer();
    }


    public class DebugScreen : DrawableGameComponent, IDebugScreen
    {
        private List<DebugItem> DebugItems;
        private List<DebugItem> SavedDebugItems;

        private List<FilterResult> FilterResults;

        private Rectangle DebugPanelRectangle;
        private Texture2D PanelTexture;
        private ScreenManager screenManager;
        FrameRateCounter frameratecounter;
        private int itemSpacer;
        private int PanelPadding;
        private int MaxDebugItems;

        private bool ConsoleVisible;


        public DebugScreen(ScreenManager screenmanager, bool visible)
            : base(screenmanager.Game)
        {
            DebugItems = new List<DebugItem>();
            SavedDebugItems = new List<DebugItem>(); 
            screenManager = screenmanager;
            ConsoleVisible = visible;
        }

        protected override void LoadContent()
        {
            frameratecounter = new FrameRateCounter(screenManager, ConsoleVisible);
            screenManager.Game.Components.Add(frameratecounter);

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
                Vector2 largestStringSize = new Vector2(30,20);//screenManager.Fonts.FrameRateCounterFont.MeasureString(DebugItems.OrderBy(s => s.GetFormatedMessage().Length).Last().GetFormatedMessage().ToString());
                DebugPanelRectangle.Width = (int)largestStringSize.X + PanelPadding * 3;
                DebugPanelRectangle.Height = (itemSpacer * DebugItems.Count) + PanelPadding * 3;

                if (DebugItems.Count > MaxDebugItems)
                {
                    //DebugItems.RemoveAll(s=>s.GetFlagType() == DebugFlagType.Normal) ..ToList());
                    List<DebugItem> tempItems = new List<DebugItem>();
                    foreach (var item in DebugItems.Skip(MaxDebugItems - DebugItems.Where(d => d.GetFlagType() != DebugFlagType.Normal).Count()))
                    {
                        if(item.GetFlagType() == DebugFlagType.Normal)
                        {
                            tempItems.Add(item);
                        }
                    }

                    foreach (var temp in tempItems)
                    {
                        DebugItems.Remove(temp);
                    }

                    //DebugItems.RemoveRange(MaxDebugItems - DebugItems.Where(d => d.GetFlagType() != DebugFlagType.Normal).Count(), DebugItems.Count - (MaxDebugItems- DebugItems.Where(d => d.GetFlagType() != DebugFlagType.Normal).Count())));
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
            for (int i = FilterResults.Count - 1; i >= 0; i -= 1)
            {

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
            
            switch (flagtype)
            {
                case DebugFlagType.Normal:
                    DebugItems.Insert(0, new DebugItem(label, message, flagtype));
                    break;
                case DebugFlagType.Odd:
                    DebugItems.Insert(0, new DebugItem(label, message,flagtype));
                    //SavedDebugItems.Insert(0, new DebugItem(label, message, flagtype));
                    break;
                case DebugFlagType.Important:
                    DebugItems.Insert(0, new DebugItem(label, message, flagtype));
                    //SavedDebugItems.Insert(0, new DebugItem(label, message, flagtype));
                    break;
            }  
        }

        public void AddAnaysisResult(List<AnalysisResult> analysisresult)
        {
            if (this.ConsoleVisible)
            {
                foreach (AnalysisResult result in analysisresult)
                {
                    AddDebugItem(result.Type, result.Message);
                }
            }
        }

        public void AddAnalysisFilterData(List<FilterResult> filterresult)
        {
            throw new NotImplementedException();
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
