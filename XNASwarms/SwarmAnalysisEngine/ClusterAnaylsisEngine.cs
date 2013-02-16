using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SwarmAnalysisEngine
{
    public class ClusterAnaylsisEngine : AnalysisEngine
    {
        public ClusterAnaylsisEngine()
            : base(new List<IAnalysisModule>() { new ClusterModule() })
        {

        }
    }
}
