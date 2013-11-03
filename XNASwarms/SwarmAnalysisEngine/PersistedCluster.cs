using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SwarmAnalysisEngine
{
    public struct PersistedCluster
    {
        public int IdentifyingAgent;
        public int ColorID;
        public int[] PastIdentifyingAgents;
    }
}
