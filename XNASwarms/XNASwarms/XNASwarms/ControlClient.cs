using Microsoft.Xna.Framework;
using ScreenSystem.ScreenSystem;
using SwarmEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if NETFX_CORE
using XNASwarmsXAML.W8;
#endif
#if WINDOWS
using SwarmAudio;
#endif

namespace XNASwarms
{
    public interface IControlClient
    {
        void CreateStableSwarm();
        void CreateMutationSwarm();
        void ZoomIn();
        void ZoomOut();
        SaveWorldParameters SaveWorld();
        void UpdatePopulation(SaveSpecies saveSpecies, bool mutate);
#if NETFX_CORE
        Task<SaveAllSpecies> Import();
        Task<SaveAllSpecies> Export();
#elif WINDOWS
        void StartSoundEngine();
        void StopSoundEngine();
#endif
        void ChangeWorldRandomSteering(double value);
        void ChangeWorldSeperatingForce(double value);
        void ChangeWorldAligningForce(double value);
        void ChangeWorldCohesiveForce(double value);
        void ChangeWorldNormalSpeed(int value);
        void ChangeWorldMaxSpeed(int value);
        void ChangeWorldNumberOfIndividuals(int value);
        void ChangeWorldNeighborhoodRadius(int value);
        double GetWorldRandomSteering();
        double GetWorldSeperatingForce();
        double GetWorldAligningForce();
        double GetWorldCohesiveForce();
        int GetWorldNormalSpeed();
        int GetWorldMaxSpeed();
        int GetWorldNumberOfIndividuals();
        int GetWorldNeighborhoodRadius();

        void ChangeBrushRandomSteering(double value);
        void ChangeBrushSeperatingForce(double value);
        void ChangeBrushAligningForce(double value);
        void ChangeBrushCohesiveForce(double value);
        void ChangeBrushNormalSpeed(int value);
        void ChangeBrushMaxSpeed(int value);
        void ChangeBrushNumberOfIndividuals(int value);
        void ChangeBrushNeighborhoodRadius(int value);
        void ChangeBrushIsMobile(bool value);
        void ChangeBrushStartingDirection(double value);
        void ChangeBrushIsUndo(bool value);
        double GetBrushRandomSteering();
        double GetBrushSeperatingForce();
        double GetBrushAligningForce();
        double GetBrushCohesiveForce();
        int GetBrushNormalSpeed();
        int GetBrushMaxSpeed();
        int GetBrushNumberOfIndividuals();
        int GetBrushNeighborhoodRadius();
        bool GetBrushIsMobile();
        double GetBrushStartingDirection();
        bool GetBrushIsUndo();

        void ChangeEraseDiameter(double value);
        double GetEraseDiameter();

#if NETFX_CORE || WINDOWS_PHONE
        void ChangeBrushColor(Windows.UI.Color color);
        Windows.UI.Color GetBrushColor();

        void PlayMusic();
        void PauseMusic();
#endif
    }

    /// <summary>
    /// Used to funnel all interaction with the world to one class 
    /// whether those are comming from custom UI's service calls or 
    /// Monogame controls. The static that hold control vlaues should
    /// only be alterd from this class
    /// </summary>
    public class ControlClient : IControlClient
    {
        SwarmScreenBase swarmScreen;
#if NETFX_CORE
        private IAudio audioScreen;
#endif

        public ControlClient(SwarmScreenBase swarmscreen)
        {
            swarmScreen = swarmscreen;
        }

#if NETFX_CORE
        public ControlClient(SwarmScreenBase swarmscreen, IAudio audio)
        {
            swarmScreen = swarmscreen;
            audioScreen = audio;
        }
#endif

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

        public void UpdatePopulation(SaveSpecies saveSpecies, bool mutate)
        {
            swarmScreen.UpdatePopulation(SaveSpeciesHelper.GetPopulationFromSaveSpecies(saveSpecies), mutate);
        }

#if NETFX_CORE
        public async Task<SaveAllSpecies> Import()
        {
            SaveAllSpecies import = await ImportExportHelper.Import();
            return import;
        }

        public async Task<SaveAllSpecies> Export()
        {
            SaveAllSpecies export = await ImportExportHelper.Export();
            return export;
        }

#elif WINDOWS
        public void StartSoundEngine()
        {
            //SoundEngine.PlayPause(1);
            SoundEngine.Play();
        }

        public void StopSoundEngine()
        {
            //SoundEngine.PlayPause(0);
            SoundEngine.Pause();
        }
#endif
        #region World
        public void ChangeWorldRandomSteering(double value)
        {
            StaticWorldParameters.ChanceOfRandomSteeringMax = value;
        }

        public void ChangeWorldSeperatingForce(double value)
        {
            StaticWorldParameters.SeperatingForceMax = value;
        }

        public void ChangeWorldAligningForce(double value)
        {
            StaticWorldParameters.AligningForceMax = value;
        }

        public void ChangeWorldCohesiveForce(double value)
        {
            StaticWorldParameters.CohesiveForceMax = value;
        }

        public void ChangeWorldNormalSpeed(int value)
        {
            StaticWorldParameters.normalSpeedMax = value;
        }

        public void ChangeWorldMaxSpeed(int value)
        {
            StaticWorldParameters.maxSpeedMax = value;
        }

        public void ChangeWorldNumberOfIndividuals(int value)
        {
            StaticWorldParameters.numberOfIndividualsMax = value;
        }

        public void ChangeWorldNeighborhoodRadius(int value)
        {
            StaticWorldParameters.neighborhoodRadiusMax = value;
        }

        public double GetWorldRandomSteering()
        {
            return StaticWorldParameters.ChanceOfRandomSteeringMax;
        }

        public double GetWorldSeperatingForce()
        {
            return StaticWorldParameters.SeperatingForceMax;
        }

        public double GetWorldAligningForce()
        {
            return StaticWorldParameters.AligningForceMax;
        }

        public double GetWorldCohesiveForce()
        {
            return StaticWorldParameters.CohesiveForceMax;
        }

        public int GetWorldNormalSpeed()
        {
            return StaticWorldParameters.normalSpeedMax;
        }

        public int GetWorldMaxSpeed()
        {
            return StaticWorldParameters.maxSpeedMax;
        }

        public int GetWorldNumberOfIndividuals()
        {
            return StaticWorldParameters.numberOfIndividualsMax;
        }

        public int GetWorldNeighborhoodRadius()
        {
            return StaticWorldParameters.neighborhoodRadiusMax;
        }
        #endregion

        #region Brush
        public void ChangeBrushRandomSteering(double value)
        {
            StaticBrushParameters.ChanceOfRandomSteeringMax = value;
        }

        public void ChangeBrushSeperatingForce(double value)
        {
            StaticBrushParameters.SeperatingForceMax = value;
        }

        public void ChangeBrushAligningForce(double value)
        {
            StaticBrushParameters.AligningForceMax = value;
        }

        public void ChangeBrushCohesiveForce(double value)
        {
            StaticBrushParameters.CohesiveForceMax = value;
        }

        public void ChangeBrushNormalSpeed(int value)
        {
            StaticBrushParameters.normalSpeedMax = value;
        }

        public void ChangeBrushMaxSpeed(int value)
        {
            StaticBrushParameters.maxSpeedMax = value;
        }

        public void ChangeBrushNumberOfIndividuals(int value)
        {
            StaticBrushParameters.numberOfIndividualsMax = value;
        }

        public void ChangeBrushNeighborhoodRadius(int value)
        {
            StaticBrushParameters.neighborhoodRadiusMax = value;
        }

        public void ChangeBrushIsMobile(bool value)
        {
            StaticBrushParameters.IsMobile = value;
        }

        public void ChangeBrushStartingDirection(double value)
        {
            StaticBrushParameters.StartingDirection = value;
        }

        public void ChangeBrushIsUndo(bool value)
        {
            StaticBrushParameters.IsUndo = value;
        }

        public double GetBrushRandomSteering()
        {
            return StaticBrushParameters.ChanceOfRandomSteeringMax;
        }

        public double GetBrushSeperatingForce()
        {
            return StaticBrushParameters.SeperatingForceMax;
        }

        public double GetBrushAligningForce()
        {
            return StaticBrushParameters.AligningForceMax;
        }

        public double GetBrushCohesiveForce()
        {
            return StaticBrushParameters.CohesiveForceMax;
        }

        public int GetBrushNormalSpeed()
        {
            return StaticBrushParameters.normalSpeedMax;
        }

        public int GetBrushMaxSpeed()
        {
            return StaticBrushParameters.maxSpeedMax;
        }

        public int GetBrushNumberOfIndividuals()
        {
            return StaticBrushParameters.numberOfIndividualsMax;
        }

        public int GetBrushNeighborhoodRadius()
        {
            return StaticBrushParameters.neighborhoodRadiusMax;
        }

        public bool GetBrushIsMobile()
        {
            return StaticBrushParameters.IsMobile;
        }

        public double GetBrushStartingDirection()
        {
            return StaticBrushParameters.StartingDirection;
        }

        public bool GetBrushIsUndo()
        {
            return StaticBrushParameters.IsUndo;
        }

#if NETFX_CORE  || WINDOWS_PHONE
        public void ChangeBrushColor(Windows.UI.Color color)
        {
            StaticBrushParameters.Color = color;
        }

        public Windows.UI.Color GetBrushColor()
        {
            return StaticBrushParameters.Color;
        }

        public void PlayMusic()
        {
            audioScreen.Play();
        }

        public void PauseMusic()
        {
            audioScreen.Pause();
        }
#endif

        #endregion

        #region Erase
        public void ChangeEraseDiameter(double value)
        {
            StaticEraseParameters.Diameter = value;
        }

        public double GetEraseDiameter()
        {
            return StaticEraseParameters.Diameter;
        }
        #endregion
    }
}
