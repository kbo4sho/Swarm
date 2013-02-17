using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SwarmEngine;

namespace SwarmAnalysisEngine
{
    public class Cluster : List<Individual>
    {
        public double MaxHeight;
        public double MinHeight;
    }
}
