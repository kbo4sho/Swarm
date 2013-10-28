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
        public float Agents{ get{return this.Count();}}  //Count of agents currently in cluster
        public Vector2 NormalizedCenter { get; set; }
        public Vector2 Center {get;set;}
        public float Area { get; set; }
        public float AverageAgentEnergy { get { return (float)this.Average(i => ((i.Dx2 * i.Dx2) + (i.Dy2 * i.Dy2))); } } // Energy if individuals in cluster
        //TODO: This is the same agent velocity for now
        public float ClusterVelocity { get { return (float)this.Average(i => ((i.Dx2 * i.Dx2) + (i.Dy2 * i.Dy2))); } } // Average velocity of individuals in a cluster
        public Vector3 Symmetry { get; set; }

        double leftMostX, rightMostX, topMostY, bottomMostY;
        // 3|2
        //  1
        // 4|5
        List<Vector2> symmetryPoints;

        public Cluster()
        {
            symmetryPoints = new List<Vector2>();
        }

        #region Symmetry

        public void SetSymmetryFromFourPoints(List<Vector2> points)
        {
            //2|1
            //3|4

            if (points.Count == 5)
            {
                //Top
                float x2a = Math.Abs(points[1].X - points[2].X);
                float x2b = Math.Abs(points[1].Y - points[2].Y);
                float x2c = (float)Math.Sqrt(((x2a * x2a) + (x2b * x2b)));
                float x2 = Normalizer.NormalizePointOneToTen(x2c);
                
                //Bottom
                float x1a = Math.Abs(points[3].X - points[4].X);
                float x1b = Math.Abs(points[3].Y - points[4].Y);
                float x1c = (float)Math.Sqrt(((x1a * x1a) + (x1b * x1b)));
                float x1 = Normalizer.NormalizePointOneToTen(x1c);
                
                float xRatio = x1 / x2;

                //Left
                float y1a = Math.Abs(points[2].Y - points[3].Y);
                float y1b = Math.Abs(points[2].X - points[3].X);
                float y1c = (float)Math.Sqrt(((y1a * y1a) + (y1b * y1b)));
                float y1 = Normalizer.NormalizePointOneToTen(y1c);

                //Right
                float y2a = Math.Abs(points[1].Y - points[4].Y);
                float y2b = Math.Abs(points[1].X - points[4].X);
                float y2c = (float)Math.Sqrt(((y2a * y2a) + (y2b * y2b)));
                float y2 = Normalizer.NormalizePointOneToTen(y2c);


                float yRatio = y1 / y2;

                float longestX = Math.Max(x1, x2);
                float longestY = Math.Max(y1, y2);

                float xyRatio = longestX / longestY;

                this.Symmetry = new Vector3(xRatio,yRatio, xyRatio);
            }
        }

        #endregion

        #region Area
        public void SetAreaFromFourPoints(List<Vector2> points)
        {
            if (points.Count == 5)
            {
                float x2a = Math.Abs(points[1].X - points[2].X);
                float x2b = Math.Abs(points[1].Y - points[2].Y);
                float x2c = (float)Math.Sqrt(((x2a * x2a) + (x2b * x2b)));
                float baseOne = x2c;

                float x1a = Math.Abs(points[3].X - points[4].X);
                float x1b = Math.Abs(points[3].Y - points[4].Y);
                float x1c = (float)Math.Sqrt(((x1a * x1a) + (x1b * x1b)));
                float baseTwo = x1c;

                float height = Math.Abs(points[1].Y - points[3].Y);

                SetArea(baseOne, baseTwo, height);
            }
        }

        private void SetArea(float baseOne, float baseTwo, float height)
        {
            this.Area = Normalizer.NormalizeToScreenArea((baseOne + baseTwo) / 2 * height);
        }
        #endregion

        #region IndvdsLocations
        private bool EveryOther;

        public float[] GetEveryOtherIndvd()
        {
            List<Individual> indvds = new List<Individual>();
            if (EveryOther)
            {
                //Odd
                indvds = this.ToList().Where((c, k) => k % 2 != 0).ToList<Individual>();
            }
            else
            {
                //Even
                indvds = this.ToList().Where((c, k) => k % 2 == 0).ToList<Individual>();
            }
            EveryOther = !EveryOther;
            float[] items = GetFloatsFromPostions(indvds);
            return items;
        }

        private float[] GetFloatsFromPostions(List<Individual> list)
        {
            float[] items = new float[100];
            int index = 0;
            list.RemoveAt(0);
            foreach (Individual indvd in list)
            {
                items[index] = (float)indvd.X;
                index++;
                items[index] = (float)indvd.Y;
                index++;
            }
            return items;
        }

        #endregion

        internal void Update()
        {
            leftMostX = this.OrderBy(point => point.X).First().X;
            rightMostX = this.OrderByDescending(point => point.X).First().X;
            topMostY = this.OrderBy(point => point.Y).First().Y;
            bottomMostY = this.OrderByDescending(point => point.Y).First().Y;

            float verticalCenter = (float)(topMostY + bottomMostY) * .5f;
            float horizontalCenter = (float)(leftMostX + rightMostX) * .5f;

            Center = new Vector2(horizontalCenter,verticalCenter);
            NormalizedCenter = new Vector2(Normalizer.NormalizeWidthCentered(horizontalCenter), Normalizer.NormalizeHeight(verticalCenter));
            AssignClusterCenterPoint(new Vector2(horizontalCenter, verticalCenter));

            //2|1
            //3|4

            //1
            AssignQuadCenterPoint(this.Where(i => i.X > horizontalCenter && i.Y < verticalCenter));
            //2
            AssignQuadCenterPoint(this.Where(i => i.X < horizontalCenter && i.Y < verticalCenter));
            //3
            AssignQuadCenterPoint(this.Where(i => i.X < horizontalCenter && i.Y > verticalCenter));
            //4
            AssignQuadCenterPoint(this.Where(i => i.X > horizontalCenter && i.Y > verticalCenter));

            ///////////////////////////
            this.SetAreaFromFourPoints(symmetryPoints);
            this.SetSymmetryFromFourPoints(symmetryPoints);
        }

        private void AssignQuadCenterPoint(IEnumerable<Individual> quadItems)
        {
            if (quadItems.Count() > 0)
            {
                leftMostX = quadItems.OrderBy(point => point.X).First().X;
                rightMostX = quadItems.OrderByDescending(point => point.X).First().X;
                topMostY = quadItems.OrderByDescending(point => point.Y).First().Y;
                bottomMostY = quadItems.OrderBy(point => point.Y).First().Y;

                Vector3[] positions;

                positions = new Vector3[3] {
                        new Vector3((float)(leftMostX + rightMostX) * .5f, (float)(topMostY + bottomMostY) * .5f,0),
                        new Vector3((float)leftMostX, (float)(bottomMostY + topMostY)*.5f, 0),
                        new Vector3((float)(leftMostX + rightMostX) * .5f, (float)topMostY, 0)
                    };

                //TODO Couldn't this just be the center of the Quad?
                symmetryPoints.Add(GetSphereDifference(positions));
            }
        }

        private Vector2 GetSphereDifference(Vector3[] positions)
        {
            BoundingSphere sphere = BoundingSphere.CreateFromPoints(positions);
            return new Vector2(sphere.Center.X, sphere.Center.Y);
        }

        private void AssignClusterCenterPoint(Vector2 clusterCenter)
        {
            symmetryPoints.Add(clusterCenter);
        }


        public int GetPointNearestToCenter()
        {
            int identifyingAgentID = -1;
            float clostestDistance = 100;

            for (int n = 0; n < this.Count; n++)
            {
                var distance = Vector2.Distance(this[n].Position, Center);
                if(distance < clostestDistance)
                {
                    clostestDistance = distance;
                    identifyingAgentID = n;
                }
            }
       
            return identifyingAgentID;
        }
    }
}
