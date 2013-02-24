using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SwarmEngine;

namespace SwarmAnalysisEngine
{
    public class NameModule : AnalysisModule
    {
        public NameModule()
            : base("Name Module", 30)
        {
        }

        public override List<AnalysisResult> Analyze(List<Individual> indvds)
        {
            return new List<AnalysisResult>(){new AnalysisResult(){ Type = ModuleName, Message = "//"}};
        }
    }
}
