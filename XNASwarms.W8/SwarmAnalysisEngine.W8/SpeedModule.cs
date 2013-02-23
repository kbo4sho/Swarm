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

        public List<AnalysisResult> Analyze(List<SwarmEngine.Individual> indvds)
        {
            for (int i = 0; i < indvds.Count; i++)
            {
                double d = indvds[i].getDx2() * indvds[i].getDx2() + indvds[i].getDy2() * indvds[i].getDy2();

                if (d > 8)
                {
                    indvds[i].setDisplayColor(Color.Yellow);
                }
                else if (d > 4)
                {
                    indvds[i].setDisplayColor(Color.Orange);
                }
                else
                {
                    indvds[i].setDisplayColor(Color.Red);
                }
            }
            return new List<AnalysisResult>();
        }
    }
}
