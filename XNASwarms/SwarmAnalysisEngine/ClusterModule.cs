using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SwarmEngine;
using Microsoft.Xna.Framework;
#if WINDOWS
using SwarmAudio;
#endif
using System.Threading.Tasks;
using System.Threading;

namespace SwarmAnalysisEngine
{
    public class ClusterModule : AnalysisModule
    {
        int clusterItemThreshhold = 8;//Number of agents that must be in proximity to be identified as a cluster
        int clusterBackCount = 30;//Number to count back in existing clusters to detect a match, high makes things slow
        
        public List<Cluster> Clusters;
        Analysis analysis;
        double leftMostX, rightMostX, topMostY, bottomMostY;
        List<Individual> lastfew;

        public ClusterModule()
            : base("Cluster Module", 30)
        {
            Clusters = new List<Cluster>();
            analysis = new Analysis();
        }
        
        protected override Analysis Analyze(List<Individual> indvds, bool sendaudiodata)
        {
            return DoAnalysis(indvds, sendaudiodata);
        }

        private void ResetColor(Individual indvd)
        {
            //TODO Hard coded color should come from config 
            indvd.setDisplayColor(Color.MidnightBlue);
        }

        private Analysis DoAnalysis(List<Individual> indvds, bool sendaudiodata)
        {
            string robinstxt = "";
            foreach (var indvd in indvds)
            {
                robinstxt += "" + Normalizer.NormalizeWidthCentered((float)indvd.X) + "," + Normalizer.NormalizeHeight((float)indvd.Y) + ",";
            }

            Clusters.Clear();

            Clusters.Add(new Cluster() { indvds[0] });

            for (int i = 0; i < indvds.Count; i++)
            {
                ResetColor(indvds[i]);
                if (!InExistingCluster(indvds[i]))
                {
                    Clusters.Add(new Cluster() { indvds[i] });
                }
            }

            RemoveSmallClusters();
            SetClusterColor();
            GenerateMessages();
            GenerateFilterResult();

            if (sendaudiodata && Clusters.Count > 0)
            {
                Cluster biggestCluster = Clusters.OrderBy(x => x.Area).First();
#if WINDOWS
                    
                    //SoundEngine.AgentDataRefresh(cluster.GetEveryOtherIndvd());
                    //SoundEngine.SendAgentEnergy(Normalizer.Normalize120To800(cluster.AverageAgentEnergy));
                    //SoundEngine.SendXYsymmetry(cluster.Symmetry);
                    //SoundEngine.SendNumAgents(cluster.Agents);
                    //SoundEngine.SendArea(cluster.Area);
                    //SoundEngine.SendClusterXY(cluster.Center.X, cluster.Center.Y);
                //SoundEngine.StartCluster();
                SoundEngine.UpdateCluster(biggestCluster.Agents,
                                              biggestCluster.Center,
                                              biggestCluster.Area,
                                              Normalizer.Normalize0ToOne(biggestCluster.AverageAgentEnergy),
                                              biggestCluster.ClusterVelocity,
                                              new Vector3(biggestCluster.Symmetry.X, biggestCluster.Symmetry.Y, biggestCluster.Symmetry.Z));
                //SoundEngine.StopCluster();
#endif         
                    //SoundEngine.UpdateCluster(1, new Vector2(.1f, .2f), 1.1f, 1.1f, 1, new Vector3(1, 1, 1));
                    //SoundEngine.SendClusterXY(Normalizer.NormalizeWidthCentered(cluster.Center.X), Normalizer.NormalizeHeight(cluster.Center.Y));
                //}
            }

            return analysis;
        }

        private void GetArea()
        {

        }

        /// <summary>
        /// Remove the clusters that are not greater than
        /// our item threshhold
        /// </summary>
        private void RemoveSmallClusters()
        {
            Clusters.RemoveAll(s => s.Count() <= clusterItemThreshhold);
        }

        /// <summary>
        /// Find if an individual agent is in a cluster
        /// </summary>
        /// <param name="individual">Agent in cluster</param>
        /// <returns></returns>
        private bool InExistingCluster(Individual individual)
        {
            List<int> clustersIds = new List<int>();
            lastfew = new List<Individual>();

            for (int c = 0; c < Clusters.Count; c++)
            {
                lastfew = Clusters[c].Skip(Math.Max(0, Clusters[c].Count() - clusterBackCount)).Take(clusterBackCount).ToList();

                for (int i = 0; i < lastfew.Count(); i++)
                {
                    double newdis = (individual.X - lastfew[i].X) * (individual.X - lastfew[i].X) + (individual.Y - lastfew[i].Y) * (individual.Y - lastfew[i].Y);

                    if (newdis < lastfew[i].Genome.getNeighborhoodRadius() * lastfew[i].Genome.getNeighborhoodRadius())
                    {
                        clustersIds.Add(c);
                        break;
                    }
                }
            }

            if (clustersIds.Count > 0)
            {
                if (clustersIds.Count > 1)
                {
                    //Merge the clusters                    
                    Clusters[clustersIds[0]].AddRange(Clusters[clustersIds[1]]);
                    Clusters.RemoveAt(clustersIds[1]);
                    Clusters[clustersIds[0]].Add(individual);
                    return true;
                }
                Clusters[clustersIds[0]].Add(individual);
                return true;
            }
            return false;
        }

        private void SetClusterColor()
        {
            for (int clusterid = 0; clusterid < Clusters.Count; clusterid++)
            {
                //TODO set colors from config file
                foreach (Individual indvd in Clusters[clusterid])
                {
                    if (clusterid == 0)
                    {
                        indvd.setDisplayColor(Color.Red);
                    }
                    else if (clusterid == 1)
                    {
                        indvd.setDisplayColor(Color.Chartreuse);
                    }
                    else if (clusterid == 2)
                    {
                        indvd.setDisplayColor(Color.Yellow);
                    }
                    else if (clusterid == 3)
                    {
                        indvd.setDisplayColor(Color.Orange);
                    }
                    else if (clusterid == 4)
                    {
                        indvd.setDisplayColor(Color.Lerp(Color.LightPink, Color.LightBlue, clusterid * .15f));
                    }
                    else
                    {
                        indvd.setDisplayColor(Color.Lerp(Color.LawnGreen, Color.Cyan, (float)((clusterid - 5) * .25)));
                    }
                }
            }
        }

        /// <summary>
        /// Generate Message for game console
        /// </summary>
        /// <returns></returns>
        private void GenerateMessages()
        {
            analysis.Messages.Clear();

            for (int i = 0; i < Clusters.Count; i++)
            {
                string clusterVisualCount = "";
                int reducedClusterCount = Clusters[i].Count() / 5;
                for (int c = 0; c < reducedClusterCount; c++)
                {
                    if (reducedClusterCount - c > 10)
                    {
                        clusterVisualCount += "||";
                    }
                    else
                    {
                        clusterVisualCount += " |";
                    }
                    
                }
                string clusterSpeed = Clusters[i].Average(x => (x.Dx2 * x.Dx2) + (x.Dy2 * x.Dy2)).ToString();

                analysis.Messages.Add(new AnalysisMessage() { Type = this.ModuleName, Message = "COUNT : " + clusterVisualCount + "  " + reducedClusterCount * 5 + " SPEED : " + clusterSpeed });
            }
            analysis.Messages.Add(new AnalysisMessage() { Type = "              ", Message = "                                                  " });
        }

        private void GenerateFilterResult()
        {
            analysis.FilterResult = new FilterResult() { Type = FilterType.ClusterCenter, ClusterPoints = new List<Vector2>() };

            foreach (Cluster cluster in Clusters)
            {
                leftMostX = cluster.OrderBy(point => point.X).First().X;
                rightMostX = cluster.OrderByDescending(point => point.X).First().X;
                topMostY = cluster.OrderBy(point => point.Y).First().Y;
                bottomMostY = cluster.OrderByDescending(point => point.Y).First().Y;

                float verticalCenter = (float)(topMostY + bottomMostY) * .5f;
                float horizontalCenter = (float)(leftMostX + rightMostX) * .5f;

                cluster.Center = new Vector2(Normalizer.NormalizeWidthCentered(horizontalCenter), Normalizer.NormalizeHeight(verticalCenter));
                AssignClusterCenterPoint(new Vector2(horizontalCenter, verticalCenter));

                //2|1
                //3|4

                //1
                AssignQuadCenterPoint(cluster.Where(i => i.X > horizontalCenter && i.Y < verticalCenter));
                //2
                AssignQuadCenterPoint(cluster.Where(i => i.X < horizontalCenter && i.Y < verticalCenter));
                //3
                AssignQuadCenterPoint(cluster.Where(i => i.X < horizontalCenter && i.Y > verticalCenter));
                //4
                AssignQuadCenterPoint(cluster.Where(i => i.X > horizontalCenter && i.Y > verticalCenter));

                ///////////////////////////
                cluster.SetAreaFromFourPoints(analysis.FilterResult.ClusterPoints);
                cluster.SetSymmetryFromFourPoints(analysis.FilterResult.ClusterPoints);
            }
            if (Clusters.Count > 0)
            {
                Cluster biggestCluster = Clusters.OrderBy(x => x.Area).First();

                analysis.Messages.Add(new AnalysisMessage() { Type = this.ModuleName, Message = "AVERAGE AGENT ENERGY : " + biggestCluster.AverageAgentEnergy });
                analysis.Messages.Add(new AnalysisMessage() { Type = this.ModuleName, Message = "NORMALIZED AGENT ENERGY : " + Normalizer.Normalize0ToOne(biggestCluster.AverageAgentEnergy) });
                analysis.Messages.Add(new AnalysisMessage() { Type = this.ModuleName, Message = "NORMALIZED SWARM ENERGY : " + Normalizer.Normalize0ToOne(biggestCluster.ClusterVelocity) });
                analysis.Messages.Add(new AnalysisMessage() { Type = this.ModuleName, Message = "AGENT SYMMETRY : " + biggestCluster.Symmetry.X + ", " + biggestCluster.Symmetry.Y + ", " + biggestCluster.Symmetry.Z });
                analysis.Messages.Add(new AnalysisMessage() { Type = this.ModuleName, Message = "AREA : " + biggestCluster.Area });
                analysis.Messages.Add(new AnalysisMessage() { Type = this.ModuleName, Message = "CENTER : " + biggestCluster.Center.X + ", " + biggestCluster.Center.Y });
            }
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
                analysis.FilterResult.ClusterPoints.Add(GetSphereDifference(positions));
            }
        }

        private Vector2 GetSphereDifference(Vector3[] positions)
        {
            BoundingSphere sphere = BoundingSphere.CreateFromPoints(positions);
            return new Vector2(sphere.Center.X,sphere.Center.Y);
        }

        private void AssignClusterCenterPoint(Vector2 clusterCenter)
        {
            analysis.FilterResult.ClusterPoints.Add(clusterCenter);
        }

    }
}
