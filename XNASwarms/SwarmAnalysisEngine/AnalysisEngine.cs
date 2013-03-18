using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SwarmEngine;
using Microsoft.Xna.Framework;
using System.Threading.Tasks;

namespace SwarmAnalysisEngine
{
    public abstract class AnalysisEngine
    {
        private List<AnalysisModule> Modules;

        public AnalysisEngine(List<AnalysisModule> modules)
        {
            Modules = modules;
        }
    
        public List<Analysis> Run(List<Individual> indvds, float gametime)
        {
            List<Analysis> analysis = new List<Analysis>();
            //Parallel.For(0, Modules.Count, i =>
            for (int i = 0; i < Modules.Count; i++ )
            {
                analysis.Add(Modules[i].TryAnalysis(indvds, gametime));
            }
            return analysis;
        }

    }
}
