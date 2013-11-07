using Microsoft.Xna.Framework;
using ScreenSystem.Debug;
using SwarmAnalysisEngine;
using SwarmEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNASwarms.Analysis.Components;

namespace XNASwarms.W8.Analysis.Components
{
    public class SwarmAnalysisComponent : IAnalysisComponent
    {
        AnalysisEngine analysisEngine;
        IDebugScreen debugScreen;
        private bool active;

        public SwarmAnalysisComponent(IDebugScreen debugScreen)
        {
            this.debugScreen = debugScreen;
            analysisEngine = new ClusterAnaylsisEngine();
        }

        public void Update(List<Individual> swarmInXOrder, Rectangle viewport, GameTime gameTime)
        {
            debugScreen.AddAnaysisResult(analysisEngine.Run(swarmInXOrder, viewport, (float)gameTime.ElapsedGameTime.TotalSeconds, active));
        }

        public void SetVisiblity()
        {
            active = !active;
            analysisEngine.ResetIndvividuals();
        }
    }
}
