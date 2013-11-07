using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SwarmEngine;
using Microsoft.Xna.Framework;

namespace SwarmAnalysisEngine
{
    public class NameModule : AnalysisModule
    {
        public NameModule()
            : base("Name Module", 30)
        {
        }

        protected override Analysis Analyze(List<Individual> indvds, Rectangle viewport)
        {
            return new Analysis(){ Messages = new List<AnalysisMessage>(){ new AnalysisMessage(){ Type = ModuleName, Message = "//"}}};
        }
    }
}
