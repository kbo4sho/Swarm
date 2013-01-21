using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ScreenSystem.ScreenSystem
{
    public class SpriteFonts
    {
        public SpriteFont DetailsFont;
        public SpriteFont FrameRateCounterFont;
        public SpriteFont MenuSpriteFont;

        public SpriteFonts(ContentManager contentManager)
        {
#if NETFX_CORE
            MenuSpriteFont = contentManager.Load<SpriteFont>("Fonts/bigmenufont");
#else
            //MenuSpriteFont = contentManager.Load<SpriteFont>("Fonts/menufont");
#endif
            //MenuSpriteFont = contentManager.Load<SpriteFont>("Fonts/menufont");
            MenuSpriteFont = contentManager.Load<SpriteFont>("Menu/menufont");
            FrameRateCounterFont = contentManager.Load<SpriteFont>("Fonts/debugfont");
            DetailsFont = contentManager.Load<SpriteFont>("Fonts/detailsfont");
        }
    }
}