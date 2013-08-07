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
        SaveWorldParameters SaveWorld();
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

        public SaveWorldParameters SaveWorld()
        {
            SaveWorldParameters world = new SaveWorldParameters();
            world.numberOfIndividualsMax = StaticWorldParameters.numberOfIndividualsMax;
            world.neighborhoodRadiusMax = StaticWorldParameters.neighborhoodRadiusMax;
            world.normalSpeedMax = StaticWorldParameters.normalSpeedMax;
            world.maxSpeedMax = StaticWorldParameters.maxSpeedMax;
            world.c1Max = StaticWorldParameters.CohesiveForceMax;
            world.c2Max = StaticWorldParameters.AligningForceMax;
            world.c3Max = StaticWorldParameters.SeperatingForceMax;
            world.c4Max = StaticWorldParameters.ChanceOfRandomSteeringMax;
            world.c5Max = StaticWorldParameters.TendencyOfPaceKeepingMax;
            return world;
        }
    }
}
