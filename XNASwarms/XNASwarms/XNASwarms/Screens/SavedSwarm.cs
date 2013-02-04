using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace XNASwarms.Screens
{
    class SavedSwarm : MenuEntry
    {
        public SavedSwarm(ControlScreen menu, string text, EntryType type, ControlScreen screen, Texture2D texture) 
            : base(menu,text,type,screen,texture)
        {
        }
    }
}
