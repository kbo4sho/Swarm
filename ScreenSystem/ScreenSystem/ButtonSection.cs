using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ScreenSystem.ScreenSystem
{
    public class ButtonSection
    {

        private GameScreen _screen;
        private Vector2 _position;
        private Texture2D _bgSprite;
        private Rectangle _rect, _innerRect;
        private string _description;
        private int _selectedEntry;
        private float _menuOffset;
        private float _maxOffset;
        private bool _scrollLock;
        private SpriteFont LabelFont, BigFont;
        private MenuButton _scrollUp;
        private List<MenuEntry> menuEntries = new List<MenuEntry>();

        private readonly Vector2 _containerMargin  = new Vector2(10, 70);
        private readonly Vector2 _containerPadding = new Vector2(12,12);

        private readonly Color _containerBGColor = new Color(247, 147, 30);
        private readonly Color _BorderColor = new Color(247, 147, 30);

        private readonly int BorderThickness = 4;
        private readonly int _lineSpace = 40;

        

        public ButtonSection(bool flip, Vector2 position, GameScreen screen, string desc)
        {
            _position = position + _containerMargin;
            _screen = screen;
            _rect.Width = 250;
            _rect.Height = 300;
            _innerRect.Width = _rect.Width - BorderThickness;
            _innerRect.Height = _rect.Height - BorderThickness;
            _description = desc;

            AddMenuItem("Button", EntryType.Game, _screen);
        }

        public void Load()
        {
            Viewport viewport = _screen.ScreenManager.GraphicsDevice.Viewport;
            _position.Y = viewport.Height / 2 - _rect.Height;
            _position.X = viewport.Width / 2 + _containerMargin.X - _rect.Width *2;
            _bgSprite = _screen.ScreenManager.Content.Load<Texture2D>("Backgrounds/gray");
            LabelFont = _screen.ScreenManager.Fonts.DetailsFont;
            BigFont = _screen.ScreenManager.Fonts.FrameRateCounterFont;
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
            if (_scoreSection != null)
            {
                _scoreSection.Draw(transitionOffset);
            }

            if (_menuEntries.Count > NumEntries)
            {
                _scrollUp.Draw();
                _scrollSlider.Draw();
                _scrollDown.Draw();
            }
            spriteBatch.End();
        }

        public void AddMenuItem(string name, EntryType type, GameScreen screen)
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
        public void HandleInput(InputHelper input, GameTime gameTime)
        {
            // Mouse or touch on a menu item
            int hoverIndex = GetMenuEntryAt(input.Cursor);
            if (hoverIndex > -1 && menuEntries[hoverIndex].IsSelectable())// && !_scrollLock)
            {
                _selectedEntry = hoverIndex;
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
                //////////////////////////////
                //Play Custom Music and game
                //////////////////////////////
                else if (menuEntries[_selectedEntry].Screen != null &&
                         menuEntries[_selectedEntry].IsCustomMusic())
                {
                    //MusicHelper.LoadSongFromLibrary();
                    _screen.ScreenManager.AddScreen(menuEntries[_selectedEntry].Screen);
                }
                ///////////////////////////////////////
                //Leave the playing Music and play game
                ///////////////////////////////////////
                else if (menuEntries[_selectedEntry].Screen != null &&
                         menuEntries[_selectedEntry].IsBackgroundMuiscMuisc())
                {
                    GameScreen mscreen = menuEntries[_selectedEntry].Screen as GameScreen;
                    //mscreen.GoToLevel(_selectedEntry,1);
                    //MusicHelper.SetMusicType(MusicType.Background);
                    _screen.ScreenManager.AddScreen(menuEntries[_selectedEntry].Screen);
                }
                ////////////////////////
                //MainMenu GameMode Game
                ////////////////////////
                else if (menuEntries[_selectedEntry].Screen != null &&
                         menuEntries[_selectedEntry].IsGameModeGame())
                {
                    _screen.ScreenManager.AddScreen(menuEntries[_selectedEntry].Screen);
                }
                ////////////////////////////
                //MainMenu GameMode FreePlay
                ////////////////////////////
                else if (menuEntries[_selectedEntry].Screen != null &&
                         menuEntries[_selectedEntry].IsGameModeFreePlay())
                {
                    _screen.ScreenManager.AddScreen(menuEntries[_selectedEntry].Screen);
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
                if (_scrollUp.Hover)
                {
                  //  _menuOffset = Math.Max(_menuOffset - 200f * (float)gameTime.ElapsedGameTime.TotalSeconds, 0f);
                   // _scrollLock = false;
                }
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

        

        public void Draw(Vector2 animation)
        {
            SpriteBatch batch = _screen.ScreenManager.SpriteBatch;


            //Container
            batch.Draw(_bgSprite, _position - animation + Vector2.One * 2f, _rect, _BorderColor, 0f, Vector2.Zero,
                        1, true ? SpriteEffects.FlipVertically : SpriteEffects.None, 0f);

            //Inner Container
            batch.Draw(_bgSprite, (_position - animation + Vector2.One * 2f) + new Vector2(BorderThickness/2,BorderThickness/2), _innerRect, _containerBGColor, 0f, Vector2.Zero,
                        1, true ? SpriteEffects.FlipVertically : SpriteEffects.None, 0f);

            //Description
            batch.DrawString(BigFont, _description, _containerPadding + _position - animation + Vector2.One * 2f, Color.LightCyan);


        }

    }
}
