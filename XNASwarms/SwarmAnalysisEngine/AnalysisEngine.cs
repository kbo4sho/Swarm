using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SwarmEngine;

namespace SwarmAnalysisEngine
{
    public abstract class AnalysisEngine
    {
        private List<IAnalysisModule> Modules;

        public AnalysisEngine(List<IAnalysisModule> modules)
        {
            Modules = modules;
        }
    
        public List<AnalysisResult> Run(List<Individual> indvds)
        {
            List<AnalysisResult> results = new List<AnalysisResult>();
            for (int i = 0; i < Modules.Count; i++ )
            {
                results.AddRange(Modules[i].Analyze(indvds));
            }
            return results;
        }

    }
}
