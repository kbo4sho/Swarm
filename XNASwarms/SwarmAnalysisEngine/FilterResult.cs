using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwarmAnalysisEngine
{
    /// <summary>
    /// Oject to transport data on the cluster to display 
    /// as a visual filter
    /// </summary>
    public class FilterResult
    {
        public List<Vector2> ClusterPoints
        {
            get;
            set;
        }

        public FilterType Type
        {
            get;
            set;
        }
    }
}
