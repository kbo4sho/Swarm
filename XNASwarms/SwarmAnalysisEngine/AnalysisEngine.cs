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
            foreach (IAnalysisModule module in Modules)
            {
                results.AddRange(module.Analyze(indvds));
            }
            return results;
        }

    }
}
