using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ScreenSystem.ScreenSystem
{
    public class MusicSelectScreen : MenuScreen
    {

        public GeneralScreen PhysicsGameScreen;
        public MusicSelectScreen(GeneralScreen physicsgamescreen)
            : base("Music", MenuType.Music)
        {
            PhysicsGameScreen = physicsgamescreen;
            AddMenuItem("Default Music", EntryType.DefaultMusic, physicsgamescreen);
            ///////////////////////////////
            //Check if the device has music
            ///////////////////////////////
//            if (MusicHelper.LibraryHasSongs())
//            {
//                AddMenuItem("Random From My Music", EntryType.CustomMusic, physicsgamescreen);
//            }
            AddMenuItem("Let Mine Play", EntryType.BackgroundMuisc, physicsgamescreen);
        }
    }
}
