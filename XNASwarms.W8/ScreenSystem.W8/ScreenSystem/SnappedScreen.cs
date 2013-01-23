using ScreenSystem.ScreenSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenSystem.W8.ScreenSystem
{
    public class SnappedScreen : MenuScreen
    {
        public SnappedScreen()
            : base("", MenuType.Pause)
        {
            AddMenuItem("-------------", EntryType.Separator, null);
            AddMenuItem("Return to\nAudioFox", EntryType.Disabled, null);
            AddMenuItem("-------------", EntryType.Separator, null);
        }
    }
}
