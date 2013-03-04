using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SwarmEngine;
using Microsoft.Xna.Framework;

namespace SwarmAnalysisEngine
{
    public abstract class AnalysisModule
    {
        protected string ModuleName { get; private set; }
        private float TimePerFrame;
        private float TotalElapsed;
        

        public AnalysisModule(string modulename, float fps)
        {
            ModuleName = modulename;
            TimePerFrame = (float)1 / fps;
        }

        public virtual Analysis TryAnalysis(List<Individual> indvds, float gametime)
        {
            if (CanAnalyize(gametime))
            {
                return Analyze(indvds);
            }

            return new Analysis();
        }

        private bool CanAnalyize(float gametime)
        {
            TotalElapsed += gametime;
            if (TotalElapsed > TimePerFrame)
            {
                TotalElapsed -= TimePerFrame;
                return true;
            }

            return false;
        }

        protected virtual Analysis Analyze(List<Individual> indvds)
        {
            return null;
        }

        
    }
}
