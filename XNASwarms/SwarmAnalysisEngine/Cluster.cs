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
        public float Agents{ get{return this.Count();}}
        public Vector2 Center { get; set; }
        public float Area { get; set; }
        public float AverageAgentEnergy { get { return (float)this.Average(i => ((i.getDx2() * i.getDx2()) + (i.getDy2() * i.getDy2()))); } }
        //TODO: This is the same agent velocity for now
        public float ClusterVelocity { get { return (float)this.Average(i => ((i.getDx2() * i.getDx2()) + (i.getDy2() * i.getDy2()))); } }
        public Vector3 Symmetry { get; set; }


        public void SetAreaFromFourPoints(List<Vector2> points)
        {
            if (points.Count == 5)
            {
                float baseOne = Math.Abs(points[1].X - points[2].X);
                float baseTwo = Math.Abs(points[3].X - points[4].X);

                float height = Math.Abs(points[1].Y - points[3].Y);

                SetArea(baseOne, baseTwo, height);
            }
        }

        private void SetArea(float baseOne, float baseTwo, float height)
        {
            this.Area = Normalizer.NormalizeToScreenArea((baseOne + baseTwo) / 2 * height);
        }
    }
}
