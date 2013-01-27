using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ScreenSystem.ScreenSystem;
using XnaSwarmsData.Debug;

namespace XnxSwarmsData.Debug
{

    public interface IDebugScreen
    {
        void AddDebugItem(string label, string message, DebugFlagType flagtype);
        void AddDebugItem(string label, string message);
        void SetVisiblity();
    }


    public class DebugScreen : DrawableGameComponent, IDebugScreen
    {

        private List<DebugItem> DebugItems;
        private List<DebugItem> SavedDebugItems;

        private Rectangle DebugPanelRectangle;
        private Texture2D PanelTexture;
        private ScreenManager screenManager;
        FrameRateCounter frameratecounter;
        private int itemSpacer;
        private int PanelPadding;
        private int MaxDebugItems;

        private bool Visible;

        public DebugScreen(ScreenManager screenmanager)
            : base(screenmanager.Game)
        {
            DebugItems = new List<DebugItem>() ;
            SavedDebugItems = new List<DebugItem>() { new DebugItem("/////DEBUG/////////////////////////////", "//////////////", DebugFlagType.Important) };
            screenManager = screenmanager;
            Visible = true;
            
        }

        protected override void LoadContent()
        {
            frameratecounter = new FrameRateCounter(screenManager);
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
            if (Visible)
            {
                Vector2 largestStringSize = screenManager.Fonts.FrameRateCounterFont.MeasureString(DebugItems.OrderBy(s => s.GetFormatedMessage().Count()).Last().GetFormatedMessage().ToString());
                //DebugPanelRectangle.Width 
                DebugPanelRectangle.Width = (int)largestStringSize.X + PanelPadding * 3;
                DebugPanelRectangle.Height = (itemSpacer * DebugItems.Count) + PanelPadding * 3;

                if (DebugItems.Count > MaxDebugItems)
                {
                    DebugItems.RemoveRange(MaxDebugItems - 3, DebugItems.Count - (MaxDebugItems));
                }
            }
            base.Update(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (Visible)
            {
                screenManager.SpriteBatch.Begin();
                screenManager.SpriteBatch.Draw(PanelTexture, DebugPanelRectangle, new Color(20, 20, 20, 170));

                for (int i = DebugItems.Count - 1; i >= 0; i -= 1)
                {
                    screenManager.SpriteBatch.DrawString(screenManager.Fonts.FrameRateCounterFont, DebugItems[i].GetFormatedMessage(),
                                                          new Vector2(DebugPanelRectangle.X + PanelPadding, (DebugPanelRectangle.Y + itemSpacer * i) + PanelPadding), DebugItems[i].GetColor());
                }
                screenManager.SpriteBatch.End();

            }
            base.Draw(gameTime);
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

        public void SetVisiblity()
        {
            Visible = !Visible;
        }
        #endregion
    }
}
