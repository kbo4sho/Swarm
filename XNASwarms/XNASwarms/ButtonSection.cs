using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using XnxSwarmsData.Debug;
using ScreenSystem.ScreenSystem;

namespace XNASwarms
{
    public class ButtonSection
    {

        private ControlScreen _screen;
        private Vector2 _position;
        private Texture2D _bgSprite;
        private Rectangle _rect, _innerRect;
        private string _description;
        private int _selectedEntry;
        private float _menuOffset;
        private float _maxOffset;
        private bool _scrollLock;
        private SpriteFont LabelFont, BigFont;
        SpriteBatch spriteBatch;
        private MenuButton _scrollUp;
        private List<MenuEntry> menuEntries = new List<MenuEntry>();
        private Rectangle _rectBG, DestinationRectangle;

        private readonly Vector2 _containerMargin  = new Vector2(10, 70);
        private readonly Vector2 _containerPadding = new Vector2(12,12);

        private readonly Color _containerBGColor = new Color(30, 30, 30, 100);
        private readonly Color _BorderColor = new Color(30, 30, 30, 100);

        private readonly int BorderThickness = 4;
        private readonly int _lineSpace = 40;
        private IDebugScreen debugScreen; 


        

        public ButtonSection(bool flip, Vector2 position, ControlScreen screen, string desc)
        {
            _rect.Width = 100;
            _rect.Height = 360;
            _screen = screen;
            _innerRect.Width = _rect.Width - BorderThickness;
            _innerRect.Height = _rect.Height - BorderThickness;
            _description = desc;
            
            AddMenuItem("Mutation", EntryType.Game, _screen);
            AddMenuItem("Stable", EntryType.Stable, _screen);
            AddMenuItem("Console", EntryType.Debugger, _screen);
        }

        public void Load()
        {
            spriteBatch = new SpriteBatch(_screen.ScreenManager.GraphicsDevice);
            debugScreen = _screen.ScreenManager.Game.Services.GetService(typeof(IDebugScreen)) as IDebugScreen;
            Viewport viewport = _screen.ScreenManager.GraphicsDevice.Viewport;
            //_position.Y = 0;
            //_position.X = 0;
            _position = new Vector2(_screen.ScreenManager.GraphicsDevice.Viewport.Width - _rect.Width, 0);// + _containerMargin;
            _bgSprite = _screen.ScreenManager.Content.Load<Texture2D>("Backgrounds/gray");
            LabelFont = _screen.ScreenManager.Fonts.DetailsFont;
            BigFont = _screen.ScreenManager.Fonts.FrameRateCounterFont;

            for (int i = 0; i < menuEntries.Count; ++i)
            {
                menuEntries[i].Initialize();
            }
        }

        public void Update()
        {
            _rect.Width = 250;
            _rect.Height = 300;
           // _position = new Vector2(_screen.ScreenManager.GraphicsDevice.Viewport.Width - _rect.Width, 0);// + _containerMargin;
        }

        /// <summary>
        /// Allows the screen the chance to position the menu entries. By default
        /// all menu entries are lined up in a vertical list, centered on the screen.
        /// </summary>
        protected virtual void UpdateMenuEntryLocations()
        {
            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this makes
            // the movement slow down as it nears the end).
           // float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            Vector2 position = Vector2.Zero;
            position.Y = _menuOffset;

            // update each menu entry's location in turn
            for (int i = 0; i < menuEntries.Count; ++i)
            {
                menuEntries[i].Position = new Vector2(_position.X, _position.Y + i * menuEntries[i].GetHeight() + i * _containerPadding.Y);
                //position.X = _screen.ScreenManager.GraphicsDevice.Viewport.Width;
                //if (_screen.ScreenState == ScreenState.TransitionOn)
                //{
                //    position.X -= 1 * 256;
                //}
                //else
                //{
                //    position.X += 1 * 256;
                //}

                // set the entry's position
                //menuEntries[i].Position = position;

                //if (position.Y < _menuBorderTop)
                //{
                //    menuEntries[i].Alpha = 1f -
                //                            Math.Min(_menuBorderTop - position.Y, _menuBorderMargin) / _menuBorderMargin;
                //}
                //else if (position.Y > _menuBorderBottom)
                //{
                //    menuEntries[i].Alpha = 1f -
                //                            Math.Min(position.Y - _menuBorderBottom, _menuBorderMargin) /
                //                            _menuBorderMargin;
                //}
                //else
                //{
                //    menuEntries[i].Alpha = 1f;
                //}

                // move down for the next entry the size of this entry
                //if (i == 0)
                //{
                //    menuEntries[i].Position += menuEntries[i].GetHeight() + 10;
                //}
                //else
                //{
                //    menuEntries[i]._position += menuEntries[i].GetHeight() + 10;
                //}
            }
            //Vector2 scrollPos = _scrollSlider.Position;
            //scrollPos.Y = MathHelper.Lerp(_scrollSliderPosition.Y, _menuBorderBottom, _menuOffset / _maxOffset);
            //_scrollSlider.Position = scrollPos;
            //UpdateTitlePosition(ScreenManager.GraphicsDevice.Viewport);

        }


        /// <summary>
        /// Draws the menu.
        /// </summary>
        public void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            // make sure our entries are in the right place before we draw them
            UpdateMenuEntryLocations();

            //SpriteBatch spriteBatch = _screen.ScreenManager.SpriteBatch;
            SpriteFont font = _screen.ScreenManager.Fonts.MenuSpriteFont;

            var pos = _position; //= _screen.Camera.ConvertScreenToWorld(_position);

            //Container
            spriteBatch.Draw(_bgSprite, pos, _rect, _BorderColor, 0f, Vector2.Zero,
                        1, true ? SpriteEffects.FlipVertically : SpriteEffects.None, 0f);

            //Inner Container
            spriteBatch.Draw(_bgSprite, pos, _innerRect, _containerBGColor, 0f, Vector2.Zero,
                        1, true ? SpriteEffects.FlipVertically : SpriteEffects.None, 0f);

            //Description
            spriteBatch.DrawString(BigFont, _description, _containerPadding + pos + Vector2.One * 2f, Color.LightCyan);
           

            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this makes
            // the movement slow down as it nears the end).
            Vector2 transitionOffset = new Vector2(0f, (float)Math.Pow(_screen.TransitionPosition, 2) * 100f);

            //Draw background
            spriteBatch.Draw(_bgSprite, DestinationRectangle, _rectBG, Color.LightCyan);

            // Draw each menu entry in turn.
            for (int i = 0; i < menuEntries.Count; ++i)
            {
                bool isSelected = _screen.IsActive && (i == _selectedEntry);
                menuEntries[i].Draw(spriteBatch);
            }

            spriteBatch.End();

            //spriteBatch.DrawString(font, _menuTitle, _titlePosition - transitionOffset, Color.WhiteSmoke, 0,
            //                       _titleOrigin, 2f, SpriteEffects.None, 0);
            //if (_scoreSection != null)
            //{
            //    _scoreSection.Draw(transitionOffset);
            //}

            //if (menuEntries.Count > NumEntries)
            //{
            //    //_scrollUp.Draw();
            //    //_scrollSlider.Draw();
            //    //_scrollDown.Draw();
            //}

        }

        public void AddMenuItem(string name, EntryType type, ControlScreen screen)
        {
            MenuEntry entry = new MenuEntry(_screen, name, type, screen, _bgSprite);
            menuEntries.Add(entry);
        }

        public void ClearMenuEntries()
        {
            menuEntries.Clear();
        }

        /// <summary>
        /// Returns the index of the menu entry at the position of the given mouse state.
        /// </summary>
        /// <returns>Index of menu entry if valid, -1 otherwise</returns>
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

        /// <summary>
        /// Responds to user input, changing the selected entry and accepting
        /// or cancelling the menu.
        /// </summary>
        public void HandleInput(InputHelper input, GameTime gameTime)
        {
            // Mouse or touch on a menu item
            int hoverIndex = GetMenuEntryAt(input.Cursor);
            if (hoverIndex >= 0)// && !_scrollLock)
            {
                _selectedEntry = hoverIndex;
                //debugScreen.AddDebugItem("BUTTON HOVER", "Index " + hoverIndex, XnaSwarmsData.Debug.DebugFlagType.Important);
            }
            else
            {
                _selectedEntry = -1;
            }

            //_scrollSlider.Hover = false;
            //if (input.IsCursorValid)
            //{
            //    _scrollUp.Collide(input.Cursor);
            //    _scrollDown.Collide(input.Cursor);
            //    _scrollSlider.Collide(input.Cursor);
            //}
            //else
            //{
            //    _scrollUp.Hover = false;
            //    _scrollDown.Hover = false;
            //    _scrollLock = false;
            //}

            // Accept or cancel the menu? 
            if (input.IsMenuSelect() && _selectedEntry != -1)
            {
                if (menuEntries[_selectedEntry].IsExitItem())
                {
                    _screen.ScreenManager.Game.Exit();
                }
                else if (menuEntries[_selectedEntry].Screen == null)
                {
                    //////////////////////////////////
                    //Navigate back to our old screen
                    //////////////////////////////////
                    if (menuEntries[_selectedEntry].IsNoAction())
                    {
                        //this.ExitScreen();
                    }
                    //////////////////////////////////
                    //Resume the game ie Pause Menu
                    //////////////////////////////////
                    if (menuEntries[_selectedEntry].IsResumeGame())
                    {
                        //////////////////
                        //Resume the music
                        //////////////////
                        //MusicHelper.ResumeSong();
                        //this.ExitScreen();

                    }
                    //////////////////////////////////
                    //Restart Game
                    //////////////////////////////////
                    if (menuEntries[_selectedEntry].IsRestart())
                    {
                        GameScreen[] scrs = _screen.ScreenManager.GetScreens();
                        GameScreen game = scrs[scrs.Length - 2] as GameScreen;
                        //game.Reset();
                        //this.ExitScreen();
                    }

                    //////////////////////////////////
                    //Go to Next Level
                    //////////////////////////////////
                    if (menuEntries[_selectedEntry].IsNextLevel())
                    {
                        GameScreen[] scrs = _screen.ScreenManager.GetScreens();
                        GameScreen game = scrs[scrs.Length - 2] as GameScreen;
                        //game.NextLevel();
                        //this.ExitScreen();

                    }
                    //////////////////////////////////
                    //Navigate back to Main Menu
                    //////////////////////////////////
                    else if (menuEntries[_selectedEntry].IsMainMenu())
                    {
                        GameScreen[] scrs = _screen.ScreenManager.GetScreens();
                        for (int i = scrs.Length - 1; i > 4; i--)
                        {
                            scrs[i].ExitScreen();
                        }
                    }
                }
                //////////////////////////////
                //Play Default Music and game
                //////////////////////////////
                else if (menuEntries[_selectedEntry].Screen != null &&
                         menuEntries[_selectedEntry].IsDefaultMusic())
                {
                    GameScreen screen = menuEntries[_selectedEntry].Screen as GameScreen;
                    //////////////////////////////////////
                    //Check whether it is game or Freeplay
                    //////////////////////////////////////
                    //if (_menuType == MenuType.FreePlayLevel)
                    //{
                    //    //////////////////////////
                    //    //Force the FreePLay Level
                    //    //Index plus on becuase
                    //    //there is no option for 
                    //    //tutorial
                    //    //////////////////////////
                    //    //screen.GoToLevel(_selectedEntry + 1, 0);
                    //}
                    //else
                    //{
                    //    //screen.GoToLevel(_selectedEntry, 1);
                    //}

                    //MusicHelper.SetMusicType(MusicType.Default);
                    _screen.ScreenManager.AddScreen(menuEntries[_selectedEntry].Screen);
                }
                
                ////////////////////////
                //MainMenu GameMode Game
                ////////////////////////
                else if (menuEntries[_selectedEntry].Screen != null &&
                         menuEntries[_selectedEntry].IsStable())
                {
                    _screen.ScreenManager.AddScreen(new SwarmScreen1(StockRecipies.Stable_A, false));
                    this._screen.ExitScreen();
                }
                ////////////////////////
                //MainMenu GameMode Game
                ////////////////////////
                else if (menuEntries[_selectedEntry].Screen != null &&
                         menuEntries[_selectedEntry].IsGameModeGame())
                {
                    _screen.ScreenManager.AddScreen(new SwarmScreen1(StockRecipies.Stable_A,true));
                    this._screen.ExitScreen();
                }
                ////////////////////////
                //MainMenu GameMode Game
                ////////////////////////
                else if (menuEntries[_selectedEntry].Screen != null &&
                         menuEntries[_selectedEntry].IsDebugger())
                {
                    debugScreen.SetVisiblity();
                }
                //////////////////////////////
                //Normal Add Screen
                //////////////////////////////
                else if (menuEntries[_selectedEntry].Screen != null)
                {
                    _screen.ScreenManager.AddScreen(menuEntries[_selectedEntry].Screen);
                }
                ///////////////////////
                //Use for tutorial
                ///////////////////////
                if (menuEntries[_selectedEntry].Screen is IDemoScreen)
                {
                    _screen.ScreenManager.AddScreen(
                        new MessageBoxScreen((menuEntries[_selectedEntry].Screen as IDemoScreen).GetDetails()));
                }

            }
            //else if (input.IsMenuCancel())
            //{
            //    //////////////////////////
            //    //Main Menu
            //    //////////////////////////
            //    if (_menuType == MenuType.MainMenu)
            //    {
            //        _screen.ScreenManager.Game.Exit();
            //    }
            //    else if (_menuType != MenuType.MainMenu)
            //    {
            //        if (_menuType == MenuType.Pause)
            //        {
            //            //MusicHelper.ResumeSong();
            //        }
            //        //this.ExitScreen();
            //    }

            //}

            if (input.IsMenuPressed())
            {
                //if (_scrollUp.Hover)
                //{
                //  //  _menuOffset = Math.Max(_menuOffset - 200f * (float)gameTime.ElapsedGameTime.TotalSeconds, 0f);
                //   // _scrollLock = false;
                //}
                //if (_scrollDown.Hover)
                //{
                //    _menuOffset = Math.Min((_menuOffset + 200f * (float)gameTime.ElapsedGameTime.TotalSeconds), _maxOffset);
                //    _scrollLock = false;
                //}
                //if (_scrollSlider.Hover)
                //{
                //    _scrollLock = true;
                //}
            }
            if (input.IsMenuReleased())
            {
                //_scrollLock = false;
            }
            if (_scrollLock)
            {
                //_scrollSlider.Hover = true;
                //_menuOffset = Math.Max(Math.Min(((input.Cursor.Y - _menuBorderTop) / (_menuBorderBottom - _menuBorderTop)) * _maxOffset, _maxOffset), 0f);
            }
        }


    }
}
