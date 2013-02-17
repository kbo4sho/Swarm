using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SwarmEngine;

namespace SwarmAnalysisEngine
{
    public class NameModule : IAnalysisModule
    {
        public string ModuleName
        {
            get 
            {
                return "Name Module";
            }
        }

        public List<AnalysisResult> Analyze(List<Individual> indvds)
        {
            return new List<AnalysisResult>(){new AnalysisResult(){ Type = ModuleName, Message = "//"}};
        }
    }
}
