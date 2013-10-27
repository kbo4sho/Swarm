using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace XNASwarms.Screens.UI
{
    class SavedSwarmButton : MenuEntry
    {
        Color Color1, Color2, Color3;

        public SavedSwarmButton(ControlScreen menu, string text, EntryType type, List<Color> colors, ControlScreen screen, Texture2D texture) 
            : base(menu,text,type,screen,texture)
        {
            if (colors != null && colors.Count > 0)
            {
                Color1 = colors[0];
                if (colors.Count > 1)
                {
                    Color2 = colors[1];
                }
                if (colors.Count > 2)
                {
                    Color3 = colors[2];
                }
            }
        }

       

        public override void Draw(SpriteBatch spritebatch)
        {
            if (_menuItemBackground != null && !IsSeperator())
            {
                Rectangle tempRect = new Rectangle(0,0,(int)(BackgroundRectangle.Width * .1), (int)(BackgroundRectangle.Height * .1));
                Vector2 padding = new Vector2(3, 3);

                spritebatch.Draw(_menuItemBackground,
                    _position + new Vector2(30,0) + padding, tempRect,
                    Color3, 0f, Vector2.Zero, _scale, SpriteEffects.None, 0f);

                spritebatch.Draw(_menuItemBackground,
                    _position + new Vector2(15, 0) + padding, tempRect,
                    Color2, 0f, Vector2.Zero, _scale, SpriteEffects.None, 0f);

                spritebatch.Draw(_menuItemBackground,
                    _position + padding, tempRect,
                    Color1, 0f, Vector2.Zero, _scale, SpriteEffects.None, 0f);
            }
            base.Draw(spritebatch);
        }
    }
}
