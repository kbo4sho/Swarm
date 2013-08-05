using Microsoft.Xna.Framework;
using ScreenSystem.ScreenSystem;
using SwarmEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNASwarms
{
    public interface IControlClient
    {
        void CreateStableSwarm();
        void CreateMutationSwarm();
        void ZoomIn();
        void ZoomOut();
    }

    public class ControlClient : IControlClient
    {
        SwarmScreenBase swarmScreen;

        public ControlClient(SwarmScreenBase swarmscreen)
        {
            swarmScreen = swarmscreen;
        }

        public void CreateStableSwarm()
        {
            swarmScreen.UpdatePopulation(StockRecipies.Stable_A, false);
        }

        public void CreateMutationSwarm()
        {
            swarmScreen.UpdatePopulation(StockRecipies.Stable_A, true);
        }

        public void ZoomIn()
        {
            swarmScreen.Camera.Zoom += .1f;
        }

        public void ZoomOut()
        {
            swarmScreen.Camera.Zoom -= .1f;
        }
    }
}
