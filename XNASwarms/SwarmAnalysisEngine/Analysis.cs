using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwarmAnalysisEngine
{
    public class Analysis
    {
        public List<AnalysisMessage> Messages;
        public FilterResult FilterResult;

        public Analysis()
        {
            Messages = new List<AnalysisMessage>();
        }
    }
}
