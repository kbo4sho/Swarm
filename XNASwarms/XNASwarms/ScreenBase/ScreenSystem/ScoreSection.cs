using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.SamplesFramework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Samples_XNA.ScreenSystem
{
    public class ScoreSection
    {

        private GameScreen _screen;
        private Vector2 _position;
        private Texture2D _bgSprite;
        private Rectangle _rect, _innerRect;
        private string _description;
        private int _achieved;
        private int _outof;
        private bool _flip;
        private Color _color;
        private int _overAllLoops;
        private int _maxComboReached;
        private SpriteFont LabelFont, BigFont;

        private readonly Vector2 _containerMargin  = new Vector2(10, 70);
        private readonly Vector2 _containerPadding = new Vector2(12,12);

        private readonly Color _containerBGColor = new Color(247, 147, 30);
        private readonly Color _BorderColor = new Color(247, 147, 30);

        private readonly int BorderThickness = 4;
        private readonly int _lineSpace = 40;

        private readonly int _perecentComplete;

        public ScoreSection(bool flip, Vector2 position, GameScreen screen, string desc, int outof, int achieved, int overallloops, int maxcomboreached)
        {
            _flip = flip;
            _position = position + _containerMargin;
            _screen = screen;
            _rect.Width = 250;
            _rect.Height = 300;
            _innerRect.Width = _rect.Width - BorderThickness;
            _innerRect.Height = _rect.Height - BorderThickness;
            _description = desc;
            _perecentComplete = (int)(((float)achieved / (float)outof) * 100);
            _achieved = achieved;
            _outof = outof;
            _color = Color.Black;
            _overAllLoops = overallloops;
            _maxComboReached = maxcomboreached;
            
        }

        public void Load()
        {
            Viewport viewport = _screen.ScreenManager.GraphicsDevice.Viewport;
            _position.Y = viewport.Height / 2 - _rect.Height;
            _position.X = viewport.Width / 2 + _containerMargin.X - _rect.Width *2;
            _bgSprite = _screen.ScreenManager.Content.Load<Texture2D>("Backgrounds/blank");
            LabelFont = _screen.ScreenManager.Fonts.DetailsFont;
            BigFont = _screen.ScreenManager.Fonts.MenuSpriteFont;
        }

        public void Draw(Vector2 animation)
        {
            SpriteBatch batch = _screen.ScreenManager.SpriteBatch;


            //Container
            batch.Draw(_bgSprite, _position - animation + Vector2.One * 2f, _rect, _BorderColor, 0f, Vector2.Zero,
                        1, _flip ? SpriteEffects.FlipVertically : SpriteEffects.None, 0f);

            //Inner Container
            batch.Draw(_bgSprite, (_position - animation + Vector2.One * 2f) + new Vector2(BorderThickness/2,BorderThickness/2), _innerRect, _containerBGColor, 0f, Vector2.Zero,
                        1, _flip ? SpriteEffects.FlipVertically : SpriteEffects.None, 0f);

            //Description
            batch.DrawString(BigFont, _description, _containerPadding + _position - animation + Vector2.One * 2f, _color);

            ////////////////////////////////
            //Dont show progress if 0 or 100
            ////////////////////////////////
            if (_perecentComplete != 0 && _perecentComplete != 100)
            {
                //Stages
                batch.DrawString(LabelFont, _perecentComplete + "% Complete", (_containerPadding + _position + new Vector2(0, _lineSpace * 2)) - animation + Vector2.One * 2f, _color);
            }

            //Loops
            batch.DrawString(LabelFont, "Loops ", (_containerPadding + _position + new Vector2(0, _lineSpace * 3)) - animation + Vector2.One * 2f, _color);
            batch.DrawString(BigFont, _overAllLoops.ToString(), (_containerPadding + _position + new Vector2(0, _lineSpace * 3.5f)) - animation + Vector2.One * 2f, _color);

            //Combos
            batch.DrawString(LabelFont, "Highest Combo " , (_containerPadding + _position + new Vector2(0, _lineSpace * 5)) - animation + Vector2.One * 2f, _color);
            batch.DrawString(BigFont, _maxComboReached.ToString(), (_containerPadding + _position + new Vector2(0, _lineSpace * 5.5f)) - animation + Vector2.One * 2f, _color);

        }

    }
}
