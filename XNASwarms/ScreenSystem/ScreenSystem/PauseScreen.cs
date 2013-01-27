using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ScreenSystem.ScreenSystem
{
    public class PauseScreen : MenuScreen
    {
        private string _details = "Details";

        public PauseScreen(string title, string details)
            : base(title, MenuType.Pause)
        {
            IsPopup = true;
            _details = details;

            AddMenuItem("Return", EntryType.ResumeGame, null);
            AddMenuItem("Restart", EntryType.Restart, null);
            AddMenuItem("MainMenu", EntryType.MainMenu, null);
        }        
        
    }
}
