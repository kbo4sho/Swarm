using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using ScreenSystem.Debug;
using SwarmEngine;
using XNASwarms.Screens;

namespace XNASwarms
{
    public class ButtonSection
    {

        private SwarmScreenBase _screen;
        private Vector2 _position;
        private Texture2D _bgSprite;
        private Rectangle _rect, _innerRect;
        private string _description;
        private int _selectedEntry;
        private SpriteFont LabelFont, BigFont;
        SpriteBatch spriteBatch;
        private List<MenuEntry> menuEntries = new List<MenuEntry>();
        private Rectangle _rectBG, DestinationRectangle;

        private readonly Vector2 _containerMargin = new Vector2(10, 70);
        private readonly Vector2 _containerPadding = new Vector2(12, 12);

        private readonly Color _containerBGColor = new Color(30, 30, 30, 100);
        private readonly Color _BorderColor = new Color(30, 30, 30, 100);

        private readonly int BorderThickness = 4;
        private IDebugScreen debugScreen;




        public ButtonSection(bool flip, Vector2 position, SwarmScreenBase screen, string desc)
        {
            _rect.Width = 100;
            _rect.Height = 360;
            _screen = screen;
            _innerRect.Width = _rect.Width - BorderThickness;
            _innerRect.Height = _rect.Height - BorderThickness;
            _description = desc;

            //AddMenuItem("+ ZOOM", EntryType.ZoomIn, _screen);
            //AddMenuItem("- ZOOM", EntryType.ZoomOut, _screen);
            AddMenuItem("Mutation", EntryType.Game, _screen);
            AddMenuItem("Stable", EntryType.Stable, _screen);
            AddMenuItem("Swinger", EntryType.Swinger, _screen);
            AddMenuItem("FussBudget", EntryType.FussBudget, _screen);
            AddMenuItem("Console", EntryType.Debugger, _screen);
            //AddMenuItem("Save", EntryType.Save, _screen);
            
        }

        public void Load()
        {
            spriteBatch = new SpriteBatch(_screen.ScreenManager.GraphicsDevice);
            debugScreen = _screen.ScreenManager.Game.Services.GetService(typeof(IDebugScreen)) as IDebugScreen;
            Viewport viewport = _screen.ScreenManager.GraphicsDevice.Viewport;
            _position = new Vector2(_screen.ScreenManager.GraphicsDevice.Viewport.Width - _rect.Width, 0);// + _containerMargin;
            _bgSprite = _screen.ScreenManager.Content.Load<Texture2D>("Backgrounds/gray");
            LabelFont = _screen.ScreenManager.Fonts.DetailsFont;
            BigFont = _screen.ScreenManager.Fonts.FrameRateCounterFont;

            for (int i = 0; i < menuEntries.Count; ++i)
            {
                menuEntries[i].Initialize();
            }
            LoadSavedSwarms();
        }

        public void Update()
        {

        }

        protected virtual void UpdateMenuEntryLocations()
        {
            Vector2 position = Vector2.Zero;
            for (int i = 0; i < menuEntries.Count; ++i)
            {
                menuEntries[i].Position = new Vector2(_position.X, _position.Y + i * menuEntries[i].GetHeight() + i * _containerPadding.Y);
            }
        }

        public void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            UpdateMenuEntryLocations();

            SpriteFont font = _screen.ScreenManager.Fonts.MenuSpriteFont;

            var pos = _position;

            spriteBatch.DrawString(BigFont, _description, _containerPadding + pos + Vector2.One * 2f, Color.LightCyan);

            Vector2 transitionOffset = new Vector2(0f, (float)Math.Pow(_screen.TransitionPosition, 2) * 100f);

            spriteBatch.Draw(_bgSprite, DestinationRectangle, _rectBG, Color.LightCyan);

            for (int i = 0; i < menuEntries.Count; ++i)
            {
                bool isSelected = _screen.IsActive && (i == _selectedEntry);
                menuEntries[i].Draw(spriteBatch);
            }

            spriteBatch.End();

        }

        private void LoadSavedSwarms()
        {
            SaveAllSpecies allSaved = SaveHelper.Load("AllSaved");
            //var olditems = menuEntries.OfType<SavedSwarm>();
            menuEntries.RemoveAll(s => s.GetType() == typeof(SavedSwarmButton));
            if (allSaved != null)
            {
                if (allSaved.Count == 1)
                {
                    AddSavedSwarm(allSaved[0].CreadtedDt.ToString("h:mm:ss"), EntryType.Recall1,allSaved[0].GetMostUsedColors(), null);
                }
                else if (allSaved.Count == 2)
                {
                    AddSavedSwarm(allSaved[0].CreadtedDt.ToString("h:mm:ss"), EntryType.Recall1, allSaved[0].GetMostUsedColors(), null);
                    AddSavedSwarm(allSaved[1].CreadtedDt.ToString("h:mm:ss"), EntryType.Recall2, allSaved[1].GetMostUsedColors(), null);
                }
                else if (allSaved.Count == 3)
                {
                    AddSavedSwarm(allSaved[0].CreadtedDt.ToString("h:mm:ss"), EntryType.Recall1, allSaved[0].GetMostUsedColors(), null);
                    AddSavedSwarm(allSaved[1].CreadtedDt.ToString("h:mm:ss"), EntryType.Recall2, allSaved[1].GetMostUsedColors(), null);
                    AddSavedSwarm(allSaved[2].CreadtedDt.ToString("h:mm:ss"), EntryType.Recall3, allSaved[2].GetMostUsedColors(), null);
                }
                
            }
        }

        private void SaveSwarm()
        {
            SaveAllSpecies allSaved = SaveHelper.Load("AllSaved");
            if (allSaved != null && allSaved.Count() >= 3)
            {
                //Replacing
                SaveSpecies oldestSpecies = allSaved.OrderBy(s => s.CreadtedDt).First();
                allSaved.Remove(oldestSpecies);
                allSaved.Add(_screen.GetPopulationAsSaveSpecies());
                SaveHelper.Save("AllSaved", allSaved);
            }

            if (allSaved != null)
            {
                if (allSaved.Count == 1)
                {
                    SaveSpecies savespecies = _screen.GetPopulationAsSaveSpecies();
                    allSaved.Add(savespecies);
                    SaveHelper.Save("AllSaved", allSaved);
                }
                else if (allSaved.Count == 2)
                {
                    SaveSpecies savespecies = _screen.GetPopulationAsSaveSpecies();
                    allSaved.Add(savespecies);
                    SaveHelper.Save("AllSaved", allSaved);
                }
            }
            else
            {
                allSaved = new SaveAllSpecies();
                SaveSpecies savespecies = _screen.GetPopulationAsSaveSpecies();
                allSaved.Add(savespecies);
                SaveHelper.Save("AllSaved", allSaved);
            }
        }

        public void AddMenuItem(string name, EntryType type, ControlScreen screen)
        {
            MenuEntry entry = new MenuEntry(_screen, name, type, screen, _bgSprite);
            menuEntries.Add(entry);
        }

        public void AddSavedSwarm(string name, EntryType type, List<Color> colors, ControlScreen screen)
        {
            SavedSwarmButton entry = new SavedSwarmButton(_screen, name, type, colors, screen, _bgSprite);
            entry.Initialize();
            menuEntries.Add(entry);
        }

        public void ClearMenuEntries()
        {
            menuEntries.Clear();
        }

        private int GetMenuEntryAt(Vector2 position)
        {
            int index = 0;
            foreach (MenuEntry entry in menuEntries)
            {
                float width = entry.GetWidth();
                float height = entry.GetHeight();
                Rectangle rect = new Rectangle((int)(entry.Position.X),
                                               (int)(entry.Position.Y),
                                               (int)width, (int)height);
                if (rect.Contains((int)position.X, (int)position.Y) && entry.Alpha > 0.1f)
                {
                    return index;
                }
                //debugScreen.AddDebugItem("CURSOR", (int)position.X + " " + (int)position.Y, XnaSwarmsData.Debug.DebugFlagType.Important);

                ++index;
            }
            return -1;
        }


        public void HandleInput(ScreenSystem.ScreenSystem.InputHelper input, GameTime gameTime)
        {
            // Mouse or touch on a menu item
            int hoverIndex = GetMenuEntryAt(input.Cursor);
            if (hoverIndex >= 0)
            {
                _selectedEntry = hoverIndex;
                //debugScreen.AddDebugItem("BUTTON HOVER", "Index " + hoverIndex, XnaSwarmsData.Debug.DebugFlagType.Important);
            }
            else
            {
                _selectedEntry = -1;
            }

            // Accept or cancel the menu? 
            if (input.IsMenuSelect() && _selectedEntry != -1)
            {
                if (menuEntries[_selectedEntry].IsExitItem())
                {
                    _screen.ScreenManager.Game.Exit();
                }
                else
                {
                    if (menuEntries[_selectedEntry].IsStable())
                    {
                        _screen.ScreenManager.AddScreen(new SwarmScreen1(StockRecipies.Stable_A, false));
                        this._screen.ExitScreen();
                    }
                    else if (menuEntries[_selectedEntry].IsSwinger())
                    {
                        _screen.ScreenManager.AddScreen(new SwarmScreen1(StockRecipies.Swinger, false));
                        this._screen.ExitScreen();
                    }
                    else if (menuEntries[_selectedEntry].IsFussBudget())
                    {
                        _screen.ScreenManager.AddScreen(new SwarmScreenFromSavedSpecies(StockSaveSpecies.GetLavaLamp));
                        this._screen.ExitScreen();
                    }
                    else if (menuEntries[_selectedEntry].IsGameModeGame())
                    {
                        _screen.ScreenManager.AddScreen(new SwarmScreen1(StockRecipies.Stable_A, true));
                        this._screen.ExitScreen();
                    }
                    else if (menuEntries[_selectedEntry].IsZoomIn())
                    {
                        if (this._screen.Camera.Zoom < 1.5)
                        {
                            this._screen.Camera.Zoom += .1f;
                        }
                    }
                    else if (menuEntries[_selectedEntry].IsZoomOut())
                    {
                        if (this._screen.Camera.Zoom > .5)
                        {
                            this._screen.Camera.Zoom -= .1f;
                        }
                    }
                    else if (menuEntries[_selectedEntry].IsDebugger())
                    {
                        debugScreen.SetVisiblity();
                    }
                    else if (menuEntries[_selectedEntry].IsSave())
                    {
                        SaveSwarm();
                        LoadSavedSwarms();
                    }
                    else if (menuEntries[_selectedEntry].IsRecall1())
                    {

                        SaveAllSpecies saveSpecies = SaveHelper.Load("AllSaved");
                        if (saveSpecies != null)
                        {
                            _screen.ScreenManager.AddScreen(new SwarmScreenFromSavedSpecies(saveSpecies[0]));
                            this._screen.ExitScreen();
                        }
                    }
                    else if (menuEntries[_selectedEntry].IsRecall2())
                    {

                        SaveAllSpecies saveSpecies = SaveHelper.Load("AllSaved");
                        if (saveSpecies != null)
                        {
                            _screen.ScreenManager.AddScreen(new SwarmScreenFromSavedSpecies(saveSpecies[1]));
                            this._screen.ExitScreen();
                        }
                    }
                    else if (menuEntries[_selectedEntry].IsRecall3())
                    {

                        SaveAllSpecies saveSpecies = SaveHelper.Load("AllSaved");
                        if (saveSpecies != null)
                        {
                            _screen.ScreenManager.AddScreen(new SwarmScreenFromSavedSpecies(saveSpecies[2]));
                            this._screen.ExitScreen();
                        }
                    }

                }

            }

        }
    }
}

