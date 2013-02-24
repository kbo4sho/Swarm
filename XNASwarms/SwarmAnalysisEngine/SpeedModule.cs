using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwarmAnalysisEngine
{
    class SpeedModule : IAnalysisModule
    {
        public string ModuleName
        {
            get { return "Speed Module"; }
        }

        public int FramesPerSecond
        {
            get { return 30; }
        }

        public List<AnalysisResult> Analyze(List<SwarmEngine.Individual> indvds)
        {
            for (int i = 0; i < indvds.Count; i++)
            {
                double d = indvds[i].getDx2() * indvds[i].getDx2() + indvds[i].getDy2() * indvds[i].getDy2();

                if (d > 12)
                {
                    indvds[i].setDisplayColor(Color.Red);
                }
                else if (d > 8)
                {
                    indvds[i].setDisplayColor(Color.DarkOrange);
                }
                else if (d > 5)
                {
                    indvds[i].setDisplayColor(Color.Yellow);
                }
                else
                {
                    indvds[i].setDisplayColor(Color.Aquamarine);
                }
            }
            return new List<AnalysisResult>();
        }
    }
}
