using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SwarmAnalysisEngine
{
    public interface IAnalysisModule
    {
        AnalysisResult Analyze();
    }
}
