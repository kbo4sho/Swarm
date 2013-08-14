using Microsoft.Xna.Framework;
using ScreenSystem.ScreenSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwarmAPI.W8
{
    public interface IControlClient
    {
        void CreateStable();
    }

    public class ControlClient : GameComponent
    {
        SwarmScreenBase gameScreen;

        public ControlClient(Game game,  GameScreen gamescreen)
            :base(game)
        {
            gameScreen = gamescreen;
        }

        public void CreateStable()
        {
            //Create a stable basic swarm
            gameScreen.UpdatePopulation(StockRecipies.Stable_A, false);
        }
    }
}
