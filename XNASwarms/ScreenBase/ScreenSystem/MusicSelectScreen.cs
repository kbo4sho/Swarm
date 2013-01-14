using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.SamplesFramework;
using WP7.Music;

namespace FarseerPhysics.SamplesFramework
{
    public class MusicSelectScreen : MenuScreen
    {

        public PhysicsGameScreen PhysicsGameScreen;
        public MusicSelectScreen(PhysicsGameScreen physicsgamescreen)
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
