using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FarseerPhysics.SamplesFramework
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
            MenuSpriteFont = contentManager.Load<SpriteFont>("Fonts/menufont");
#endif
            MenuSpriteFont = contentManager.Load<SpriteFont>("Fonts/menufont");
            MenuSpriteFont = contentManager.Load<SpriteFont>("Fonts/menufont");
            FrameRateCounterFont = contentManager.Load<SpriteFont>("Fonts/frameratecounterfont");
            DetailsFont = contentManager.Load<SpriteFont>("Fonts/detailsfont");
        }
    }
}