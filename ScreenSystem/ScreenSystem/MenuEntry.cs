using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ScreenSystem.ScreenSystem
{
    public enum EntryType
    {
        Screen,
        Separator,
        ExitItem,
        NoAction,
        MainMenu,
        Restart,
        NextLevel,
        DefaultMusic,
        CustomMusic,
        BackgroundMuisc,
        ResumeGame,
        Game,
        FreePlay,
        Disabled,
    }

    /// <summary>
    /// Helper class represents a single entry in a MenuScreen. By default this
    /// just draws the entry text string, but it can be customized to display menu
    /// entries in different ways. This also provides an event that will be raised
    /// when the menu entry is selected.
    /// </summary>
    public class MenuEntry
    {
        public float _alpha { get; private set; }
        public Vector2 _baseOrigin { get; private set; }

        private float _height;
        public GameScreen _menu { get; private set; }

        /// <summary>
        /// The position at which the entry is drawn. This is set by the MenuScreen
        /// each frame in Update.
        /// </summary>
        public Vector2 _position { get; private set; }
        public float _scale { get; private set; }
        private float BaseScale;
        private GameScreen _screen;
		private GameScreen _phyisicsGameScreen;

        /// <summary>
        /// Tracks a fading selection effect on the entry.
        /// </summary>
        /// <remarks>
        /// The entries transition out of the selection effect when they are deselected.
        /// </remarks>
        public float _selectionFade { get; private set; }

        public string _text { get; private set; }
        public EntryType _type { get; private set; }
        private float _width;
        public Texture2D _menuItemBackground { get; private set; }
        public Rectangle BackgroundRectangle { get; private set; }
        private Color MenuEntryBackground;

        public MenuEntry(GameScreen menu, string text, EntryType type, GameScreen screen, Texture2D texture, GameScreen phyisicsscreen)
            : this(menu, text, type, screen, texture)
		{
			_phyisicsGameScreen = phyisicsscreen;
		}

        /// <summary>
        /// Constructs a new menu entry with the specified text.
        /// </summary>
		/// 
        public MenuEntry(GameScreen menu, string text, EntryType type, GameScreen screen, Texture2D texture)
        {
            _text = text;
            _screen = screen;
            _type = type;
            _menu = menu;
#if NETFX_CORE
            BaseScale = 1f;
#else
            BaseScale = 1f;
#endif
            _scale = 1f;
            //_width = 100;
            //_height = 40;
            _alpha = 1.0f;
            _menuItemBackground = texture;
            
        }


        /// <summary>
        /// Gets or sets the text of this menu entry.
        /// </summary>
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        /// <summary>
        /// Gets or sets the position at which to draw this menu entry.
        /// </summary>
        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public float Alpha
        {
            get { return _alpha; }
            set { _alpha = value; }
        }

        public GameScreen Screen
        {
            get { return _screen; }
        }

        public void Initialize()
        {
            SpriteFont font = _menu.ScreenManager.Fonts.MenuSpriteFont;

            _menuItemBackground = _menu.ScreenManager.Content.Load<Texture2D>("Menu/button");
            BackgroundRectangle = new Rectangle(0, 0, 100, 100);

            _baseOrigin = new Vector2(font.MeasureString(Text).X / 2, font.MeasureString(Text).Y / 2);

            _width = BackgroundRectangle.Width;
            _height = BackgroundRectangle.Height;

            SetBackgroundColor();
        }

        private void SetBackgroundColor()
        {
            if (IsDisabled())
            {
                MenuEntryBackground = new Color(30, 30, 30, 100);
                
            }
            else
            {
                MenuEntryBackground = new Color(30, 30, 30, 100);
            }
        }

        #region boolChecks
        public bool IsExitItem()
        {
            return _type == EntryType.ExitItem;
        }

        public bool IsSelectable()
        {
            return _type != EntryType.Separator && _type != EntryType.Disabled;
        }

        public bool IsSeperator()
        {
            return _type == EntryType.Separator;
        }

        public bool IsDisabled()
        {
            return _type == EntryType.Disabled;
        }

        public bool IsResumeGame()
        {
            return _type == EntryType.ResumeGame;
        }

        public bool IsNoAction()
        {
            return _type == EntryType.NoAction;
        }

        public bool IsMainMenu()
        {
            return _type == EntryType.MainMenu;
        }

        public bool IsRestart()
        {
            return _type == EntryType.Restart;
        }

        public bool IsNextLevel()
        {
            return _type == EntryType.NextLevel;
        }

        public bool IsDefaultMusic()
        {
            return _type == EntryType.DefaultMusic;
        }

        public bool IsCustomMusic()
        {
            return _type == EntryType.CustomMusic;
        }

        public bool IsBackgroundMuiscMuisc()
        {
            return _type == EntryType.BackgroundMuisc;
        }

        public bool IsGameModeGame()
        {
            return _type == EntryType.Game;
        }

        public bool IsGameModeFreePlay()
        {
            return _type == EntryType.FreePlay;
        }
        #endregion

        /// <summary>
        /// Updates the menu entry.
        /// </summary>
        public void Update(bool isSelected, GameTime gameTime)
        {
            // there is no such thing as a selected item on Windows Phone, so we always
            // force isSelected to be false
#if WINDOWS_PHONE || IPHONE
            isSelected = false;
#endif
            // When the menu selection changes, entries gradually fade between
            // their selected and deselected appearance, rather than instantly
            // popping to the new state.
            if (_type != EntryType.Separator)
            {
                float fadeSpeed = (float)gameTime.ElapsedGameTime.TotalSeconds * 4;
                if (isSelected)
                {
                    _selectionFade = Math.Min(_selectionFade + fadeSpeed, 1f);
                }
                else
                {
                    _selectionFade = Math.Max(_selectionFade - fadeSpeed, 0f);
                }
                _scale = BaseScale + 0.1f * _selectionFade;
            }
        }

        /// <summary>
        /// Draws the menu entry. This can be overridden to customize the appearance.
        /// </summary>
        public virtual void Draw()
        {

            SpriteFont font     = _menu.ScreenManager.Fonts.MenuSpriteFont;
            SpriteBatch batch   = _menu.ScreenManager.SpriteBatch;

            Color color;
            if (_type == EntryType.Separator)
            {
                color = Color.Black;
            }
            else
            {
                // Draw the selected entry in yellow, otherwise white
                color = Color.Lerp(Color.Black, Color.Black, _selectionFade);
            }
            color *= _alpha;


            //Drar the container of the item
            if (_menuItemBackground != null && !IsSeperator())
            {
                batch.Draw(_menuItemBackground,
                    Vector2.Zero, BackgroundRectangle,
                    MenuEntryBackground , 0f, Vector2.Zero, _scale, SpriteEffects.None, 0f);
            }
            // Draw text, centered on the middle of each line.
            //batch.DrawString(font, _text, _position - _baseOrigin * _scale + Vector2.One,
            //                  Color.DarkSlateGray * _alpha * _alpha, 0, Vector2.Zero, _scale, SpriteEffects.None, 0);
            batch.DrawString(font, _text, Vector2.Zero, Color.WhiteSmoke, 0, Vector2.Zero, _scale,
                              SpriteEffects.None, 0);
        }

        /// <summary>
        /// Queries how much space this menu entry requires.
        /// </summary>
        public int GetHeight()
        {
            return (int)_height;
        }

        /// <summary>
        /// Queries how wide the entry is, used for centering on the screen.
        /// </summary>
        public int GetWidth()
        {
            return (int)_width;
        }
    }
}