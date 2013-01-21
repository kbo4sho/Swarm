using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace ScreenSystem.ScreenSystem
{

    public enum MenuType
    {
        MainMenu,
        Music,
        Level,
        NextLevel,
        EndScreen,
        GameCompleted,
        Pause,
        FreePlayLevel,
    }
    /// <summary>
    /// Base class for screens that contain a menu of options. The user can
    /// move up and down to select an entry, or cancel to back out of the screen.
    /// </summary>
    public class MenuScreen : GameScreen
    {
#if WINDOWS || XBOX
        private const float NumEntries = 15;
#elif WINDOWS_PHONE
        private const float NumEntries = 4;
#else
        private const float NumEntries = 6;
#endif
        private List<MenuEntry> _menuEntries = new List<MenuEntry>();
        private string _menuTitle;
        private Vector2 _titlePosition;
        private float TitleWidth;
        private Vector2 _titleOrigin;
        private int _selectedEntry;
        private float _menuBorderTop;
        private float _menuBorderBottom;
        private float _menuBorderMargin;
        private float _menuOffset;
        private float _maxOffset;
        private float _menuItemMarginTop;

        private Texture2D _texScrollButton;
        private Texture2D _texSlider;
        private Texture2D _texScoreSection;
        private Texture2D _menuItemBackground;

        private Texture2D _background;
        private Rectangle _rectBG, DestinationRectangle;
        private Color _bgColor;

        private MenuButton _scrollUp;
        private MenuButton _scrollDown;
        private MenuButton _scrollSlider;
        private Vector2 _scrollSliderPosition;

        //private ButtonSection _scoreSection;
        private Vector2 _scoreSectionOrigin;
        private bool _scrollLock;

        private MenuType _menuType;

        /// <summary>
        /// Constructor.
        /// </summary>
        public MenuScreen(string menuTitle, MenuType type)
        {
            _menuTitle = menuTitle;
            TransitionOnTime = TimeSpan.FromSeconds(0.7);
            TransitionOffTime = TimeSpan.FromSeconds(0.7);
            _menuType = type;
            HasCursor = true;
            //////////////////////////////
            //MenuItem top margin
            //////////////////////////////
            _menuItemMarginTop = 30;

        }

        public void AddMenuItem(string name, EntryType type, GameScreen screen)
        {
            MenuEntry entry = new MenuEntry(this, name, type, screen, _menuItemBackground);
            _menuEntries.Add(entry);
        }

		public void AddLevelMenuItem(string name, EntryType type, GameScreen screen, GameScreen physicsscreen)
		{
			MenuEntry entry = new MenuEntry(this, name, type, screen,_menuItemBackground, physicsscreen);
            _menuEntries.Add(entry);
		}


        public void ClearMenuEntries()
        {
            _menuEntries.Clear();
        }

        public void AddScoreSection(string desc, int outof, int achieved, int overallloops,int maxcomboreached)
        {
           // _scoreSection = new ButtonSection(false, _titleOrigin, this, desc);
        }



        public override void LoadContent()
        {
            base.LoadContent();

            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            SpriteFont font = ScreenManager.Fonts.FrameRateCounterFont;

            _menuItemMarginTop = font.MeasureString("M").Y;
            _bgColor = Color.White;
            
            DestinationRectangle = new Rectangle(0, 0, viewport.Width, viewport.Height);

            _texScrollButton = ScreenManager.Content.Load<Texture2D>("Common/arrow");
            _texSlider = ScreenManager.Content.Load<Texture2D>("Common/scroller");
            _background = ScreenManager.Content.Load<Texture2D>("Backgrounds/MenuBG");

            _rectBG = new Rectangle(0, 0, _background.Width, _background.Height);
            

            //////////////////////////////////////
            //Loas these only if we have a score
            //section
            //////////////////////////////////////
            //if (_scoreSection != null)
            //{
            //    _scoreSection.Load();
            //}

            float scrollBarPos = viewport.Width / 2f;
            for (int i = 0; i < _menuEntries.Count; ++i)
            {
                _menuEntries[i].Initialize();
                scrollBarPos = Math.Min(scrollBarPos,
                                         (viewport.Width - _menuEntries[i].GetWidth())/3);
            }
            scrollBarPos -= _texScrollButton.Width;

            _titleOrigin = font.MeasureString(_menuTitle) / 2f;
            TitleWidth = font.MeasureString("M").Y / 2f + 20f;
            UpdateTitlePosition(viewport);
            
            _menuBorderMargin = font.MeasureString("M").Y +_menuItemMarginTop + 100; 
            _menuBorderTop = (_menuBorderMargin);
            _scrollSliderPosition = new Vector2(scrollBarPos, _menuBorderTop + 20);
            _menuBorderBottom = (viewport.Height - _menuBorderMargin);

            _menuOffset = 0f;
            _maxOffset = Math.Max(0f, ((_menuEntries.Count - 4) * _menuBorderMargin));

            _scrollUp = new MenuButton(_texScrollButton, false,
                                       new Vector2(scrollBarPos, _menuBorderTop - _texScrollButton.Height), this);
            _scrollDown = new MenuButton(_texScrollButton, true,
                                         new Vector2(scrollBarPos, _menuBorderBottom + _texScrollButton.Height), this);
            _scrollSlider = new MenuButton(_texSlider, false, _scrollSliderPosition, this);

            _scrollLock = false;
        }

        /// <summary>
        /// Returns the index of the menu entry at the position of the given mouse state.
        /// </summary>
        /// <returns>Index of menu entry if valid, -1 otherwise</returns>
        private int GetMenuEntryAt(Vector2 position)
        {
            int index = 0;
            foreach (MenuEntry entry in _menuEntries)
            {
                float width = entry.GetWidth();
                float height = entry.GetHeight();
                Rectangle rect = new Rectangle((int)(entry.Position.X - width * .5f),
                                               (int)(entry.Position.Y - height * .5f),
                                               (int)width, (int)height);
                if (rect.Contains((int)position.X, (int)position.Y) && entry.Alpha > 0.1f)
                {
                    return index;
                }
                ++index;
            }
            return -1;
        }

        /// <summary>
        /// Responds to user input, changing the selected entry and accepting
        /// or cancelling the menu.
        /// </summary>
        public override void HandleInput(InputHelper input, GameTime gameTime)
        {
            // Mouse or touch on a menu item
            int hoverIndex = GetMenuEntryAt(input.Cursor);
            if (hoverIndex > -1 && _menuEntries[hoverIndex].IsSelectable() && !_scrollLock)
            {
                _selectedEntry = hoverIndex;
            }
            else
            {
                _selectedEntry = -1;
            }

            _scrollSlider.Hover = false;
            if (input.IsCursorValid)
            {
                _scrollUp.Collide(input.Cursor);
                _scrollDown.Collide(input.Cursor);
                _scrollSlider.Collide(input.Cursor);
            }
            else
            {
                _scrollUp.Hover = false;
                _scrollDown.Hover = false;
                _scrollLock = false;
            }

            // Accept or cancel the menu? 
            if (input.IsMenuSelect() && _selectedEntry != -1)
            {
                if (_menuEntries[_selectedEntry].IsExitItem())
                {
                    ScreenManager.Game.Exit();
                }
                else if (_menuEntries[_selectedEntry].Screen == null)
                {
                    //////////////////////////////////
                    //Navigate back to our old screen
                    //////////////////////////////////
                    if (_menuEntries[_selectedEntry].IsNoAction())
                    {
                        this.ExitScreen();
                    }
                    //////////////////////////////////
                    //Resume the game ie Pause Menu
                    //////////////////////////////////
                    if (_menuEntries[_selectedEntry].IsResumeGame())
                    {
                        //////////////////
                        //Resume the music
                        //////////////////
                        //MusicHelper.ResumeSong();
                        this.ExitScreen();

                    }
                    //////////////////////////////////
                    //Restart Game
                    //////////////////////////////////
                    if (_menuEntries[_selectedEntry].IsRestart())
                    {
                        GameScreen[] scrs = ScreenManager.GetScreens();
                        GameScreen game = scrs[scrs.Length - 2] as GameScreen;
                        //game.Reset();
                        this.ExitScreen();
                    }

                    //////////////////////////////////
                    //Go to Next Level
                    //////////////////////////////////
                    if (_menuEntries[_selectedEntry].IsNextLevel())
                    {
                        GameScreen[] scrs = ScreenManager.GetScreens();
                        GameScreen game = scrs[scrs.Length - 2] as GameScreen;
                        //game.NextLevel();
                        this.ExitScreen();

                    }
                    //////////////////////////////////
                    //Navigate back to Main Menu
                    //////////////////////////////////
                    else if (_menuEntries[_selectedEntry].IsMainMenu())
                    {
                        GameScreen[] scrs = ScreenManager.GetScreens();
                        for (int i = scrs.Length - 1; i > 4; i--)
                        {
                            scrs[i].ExitScreen();
                        }
                    }
                }
                //////////////////////////////
                //Play Default Music and game
                //////////////////////////////
                else if (_menuEntries[_selectedEntry].Screen != null &&
                         _menuEntries[_selectedEntry].IsDefaultMusic())
                {
                    GameScreen screen = _menuEntries[_selectedEntry].Screen as GameScreen;
                    //////////////////////////////////////
                    //Check whether it is game or Freeplay
                    //////////////////////////////////////
                    if (_menuType == MenuType.FreePlayLevel)
                    {
                        //////////////////////////
                        //Force the FreePLay Level
                        //Index plus on becuase
                        //there is no option for 
                        //tutorial
                        //////////////////////////
                        //screen.GoToLevel(_selectedEntry + 1, 0);
                    }
                    else
                    {
                        //screen.GoToLevel(_selectedEntry, 1);
                    }
                    
                    //MusicHelper.SetMusicType(MusicType.Default);
                    ScreenManager.AddScreen(_menuEntries[_selectedEntry].Screen);
                }
                //////////////////////////////
                //Play Custom Music and game
                //////////////////////////////
                else if (_menuEntries[_selectedEntry].Screen != null &&
                         _menuEntries[_selectedEntry].IsCustomMusic())
                {
                    //MusicHelper.LoadSongFromLibrary();
                    ScreenManager.AddScreen(_menuEntries[_selectedEntry].Screen);
                }
                ///////////////////////////////////////
                //Leave the playing Music and play game
                ///////////////////////////////////////
                else if (_menuEntries[_selectedEntry].Screen != null &&
                         _menuEntries[_selectedEntry].IsBackgroundMuiscMuisc())
                {
                    GameScreen mscreen = _menuEntries[_selectedEntry].Screen as GameScreen;
                    //mscreen.GoToLevel(_selectedEntry,1);
                    //MusicHelper.SetMusicType(MusicType.Background);
                    ScreenManager.AddScreen(_menuEntries[_selectedEntry].Screen);
                }
                ////////////////////////
                //MainMenu GameMode Game
                ////////////////////////
                else if (_menuEntries[_selectedEntry].Screen != null &&
                         _menuEntries[_selectedEntry].IsGameModeGame())
                {
                    ScreenManager.AddScreen(_menuEntries[_selectedEntry].Screen);
                }
                ////////////////////////////
                //MainMenu GameMode FreePlay
                ////////////////////////////
                else if (_menuEntries[_selectedEntry].Screen != null &&
                         _menuEntries[_selectedEntry].IsGameModeFreePlay())
                {
                    ScreenManager.AddScreen(_menuEntries[_selectedEntry].Screen);
                }
                //////////////////////////////
                //Normal Add Screen
                //////////////////////////////
                else if (_menuEntries[_selectedEntry].Screen != null)
                {
                    ScreenManager.AddScreen(_menuEntries[_selectedEntry].Screen);
                }
                ///////////////////////
                //Use for tutorial
                ///////////////////////
                if (_menuEntries[_selectedEntry].Screen is IDemoScreen)
                {
                    ScreenManager.AddScreen(
                        new MessageBoxScreen((_menuEntries[_selectedEntry].Screen as IDemoScreen).GetDetails()));
                }

            }
            else if (input.IsMenuCancel())
            {
                //////////////////////////
                //Main Menu
                //////////////////////////
                if (_menuType == MenuType.MainMenu)
                {
                    ScreenManager.Game.Exit();
                }
                else if (_menuType != MenuType.MainMenu)
                {
                    if (_menuType == MenuType.Pause)
                    {
                        //MusicHelper.ResumeSong();
                    }
                    this.ExitScreen();
                }
                
            }

            if (input.IsMenuPressed())
            {
                if (_scrollUp.Hover)
                {
                    _menuOffset = Math.Max(_menuOffset - 200f * (float)gameTime.ElapsedGameTime.TotalSeconds, 0f);
                    _scrollLock = false;
                }
                if (_scrollDown.Hover)
                {
                    _menuOffset = Math.Min((_menuOffset + 200f * (float)gameTime.ElapsedGameTime.TotalSeconds), _maxOffset);
                    _scrollLock = false;
                }
                if (_scrollSlider.Hover)
                {
                    _scrollLock = true;
                }
            }
            if (input.IsMenuReleased())
            {
                _scrollLock = false;
            }
            if (_scrollLock)
            {
                _scrollSlider.Hover = true;
                _menuOffset = Math.Max(Math.Min(((input.Cursor.Y - _menuBorderTop) / (_menuBorderBottom - _menuBorderTop)) * _maxOffset, _maxOffset), 0f);
            }
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
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            Vector2 position = Vector2.Zero;
            position.Y = _menuBorderTop - _menuOffset;

            // update each menu entry's location in turn
            for (int i = 0; i < _menuEntries.Count; ++i)
            {
                position.X = ScreenManager.GraphicsDevice.Viewport.Width / 2f;
                if (ScreenState == ScreenState.TransitionOn)
                {
                    position.X -= transitionOffset * 256;
                }
                else
                {
                    position.X += transitionOffset * 256;
                }

                // set the entry's position
                _menuEntries[i].Position = position;

                if (position.Y < _menuBorderTop)
                {
                    _menuEntries[i].Alpha = 1f -
                                            Math.Min(_menuBorderTop - position.Y, _menuBorderMargin) / _menuBorderMargin;
                }
                else if (position.Y > _menuBorderBottom)
                {
                    _menuEntries[i].Alpha = 1f -
                                            Math.Min(position.Y - _menuBorderBottom, _menuBorderMargin) /
                                            _menuBorderMargin;
                }
                else
                {
                    _menuEntries[i].Alpha = 1f;
                }

                // move down for the next entry the size of this entry
                if (i == 0)
                {
                    position.Y += _menuEntries[i].GetHeight() + _menuItemMarginTop;
                }
                else
                {
                    position.Y += _menuEntries[i].GetHeight() + _menuItemMarginTop;
                }
            }
            Vector2 scrollPos = _scrollSlider.Position;
            scrollPos.Y = MathHelper.Lerp(_scrollSliderPosition.Y, _menuBorderBottom, _menuOffset / _maxOffset);
            _scrollSlider.Position = scrollPos;
            UpdateTitlePosition(ScreenManager.GraphicsDevice.Viewport);

        }

        private void UpdateTitlePosition(Viewport viewport)
        {
            _titlePosition = new Vector2(viewport.Width / 2f, TitleWidth);
        }

        /// <summary>
        /// Updates the menu.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                    bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            // Update each nested MenuEntry object.
            for (int i = 0; i < _menuEntries.Count; ++i)
            {
                bool isSelected = IsActive && (i == _selectedEntry);
                _menuEntries[i].Update(isSelected, gameTime);
            }

            _scrollUp.Update(gameTime);
            _scrollDown.Update(gameTime);
            _scrollSlider.Update(gameTime);
        }

        /// <summary>
        /// Draws the menu.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // make sure our entries are in the right place before we draw them
            UpdateMenuEntryLocations();
            

            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Fonts.MenuSpriteFont;

            spriteBatch.Begin();

            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this makes
            // the movement slow down as it nears the end).
            Vector2 transitionOffset = new Vector2(0f, (float)Math.Pow(TransitionPosition, 2) * 100f);

            //Draw background
            spriteBatch.Draw(_background, DestinationRectangle, _rectBG, _bgColor);
            
            // Draw each menu entry in turn.
            for (int i = 0; i < _menuEntries.Count; ++i)
            {
                bool isSelected = IsActive && (i == _selectedEntry);
                _menuEntries[i].Draw();
            }
            
            spriteBatch.DrawString(font, _menuTitle, _titlePosition - transitionOffset, Color.WhiteSmoke, 0,
                                   _titleOrigin, 2f, SpriteEffects.None, 0);
            //if (_scoreSection != null)
            //{
            //    //_scoreSection.Draw(transitionOffset);
            //}

            if (_menuEntries.Count > NumEntries)
            {
                _scrollUp.Draw();
                _scrollSlider.Draw();
                _scrollDown.Draw();
            }
            spriteBatch.End();
        }
    }
}