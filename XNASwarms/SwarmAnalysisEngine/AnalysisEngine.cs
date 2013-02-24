using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SwarmEngine;
using Microsoft.Xna.Framework;

namespace SwarmAnalysisEngine
{
    public abstract class AnalysisEngine
    {
        private List<AnalysisModule> Modules;

        public AnalysisEngine(List<AnalysisModule> modules)
        {
            Modules = modules;
        }
    
        public List<AnalysisResult> Run(List<Individual> indvds, float gametime)
        {
            List<AnalysisResult> results = new List<AnalysisResult>();
            for (int i = 0; i < Modules.Count; i++ )
            {
                results.AddRange(Modules[i].TryAnalysis(indvds, gametime));
            }
            return results;
        }

    }
}
