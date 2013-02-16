using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SwarmAnalysisEngine
{
    public abstract class AnalysisEngine
    {
        private List<IAnalysisModule> Modules;

        public AnalysisEngine(List<IAnalysisModule> modules)
        {
            Modules = modules;
        }
    
        public List<AnalysisResult> Run()
        {
            List<AnalysisResult> results = new List<AnalysisResult>();
            foreach (IAnalysisModule module in Modules)
            {
                results.Add(module.Analyze());
            }
            return results;
        }

    }
}
