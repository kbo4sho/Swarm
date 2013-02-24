using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SwarmEngine;

namespace SwarmAnalysisEngine
{
    public abstract class AnalysisModule
    {
        string ModuleName{get;}

        int FramesPerSecond{get;}

        List<AnalysisResult> Analyze(List<Individual> indvds);
    }
}
