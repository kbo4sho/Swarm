using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwarmAnalysisEngine
{
    class SpeedModule : AnalysisModule
    {
        public SpeedModule()
            : base("Speed Module", 30)
        {
        }

        protected override Analysis Analyze(List<SwarmEngine.Individual> indvds, bool sendaudiodata)
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
            return new Analysis();
        }
    }
}
