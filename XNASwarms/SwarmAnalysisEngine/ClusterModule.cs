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
using SwarmAudio;

namespace SwarmAnalysisEngine
{
    public class ClusterModule : AnalysisModule
    {
        int maxClustersToPersist = 4;
        int clusterItemThreshhold = 8;//Number of agents that must be in proximity to be identified as a cluster
        int clusterBackCount = 20;//Number to count back in existing clusters to detect a match, high makes things slow

        private List<Cluster> clusters;
        private Dictionary<int, int> trackedClusters;
        Analysis analysis;

        List<Individual> lastfew;

        public ClusterModule()
            : base("Cluster Module", 15)
        {
            clusters = new List<Cluster>();
            trackedClusters = new Dictionary<int, int>();
            trackedClusters.Add(0, -1);
            trackedClusters.Add(1, -1);
            trackedClusters.Add(2, -1);
            trackedClusters.Add(3, -1);
            analysis = new Analysis();
        }

        protected override Analysis Analyze(List<Individual> indvds, bool visible)
        {
            return DoAnalysis(indvds, visible);
        }

        private void ResetColor(Individual indvd)
        {
            //TODO Hard coded color should come from config 
            indvd.setDisplayColor(Color.MidnightBlue);
        }

        private Analysis DoAnalysis(List<Individual> indvds, bool visble)
        {
            if (indvds.Count() > 0 && visble)
            {
                //string robinstxt = "";
                //foreach (var indvd in indvds)
                //{
                //    robinstxt += "" + Normalizer.NormalizeWidthCentered((float)indvd.X) + "," + Normalizer.NormalizeHeight((float)indvd.Y) + ",";
                //}

                clusters.Clear();

                clusters.Add(new Cluster() { indvds[0] });

                for (int i = 0; i < indvds.Count; i++)
                {
                    ResetColor(indvds[i]);
                    if (!InExistingCluster(indvds[i]))
                    {
                        clusters.Add(new Cluster() { indvds[i] });
                    }
                }

                RemoveSmallClusters();

                foreach (Cluster cluster in clusters.OrderBy(c=>c.Area))
                {
                    cluster.Update();
                }

                //Update persisting clusters
                for (int n = 0; n < trackedClusters.Count; n++)
                {

                    //Try to add cluster to tracked
                    if (trackedClusters[n] == -1)
                    {
                        Cluster val = clusters.Where(c => c.All(i => !trackedClusters.ContainsValue(i.ID))).FirstOrDefault();

                        if (val != null)
                        {
                            //TODO: Why should i have to do this
                            bool really = trackedClusters.ContainsValue(val.GetPointNearestToCenter());
                            if (!really)
                            {
                                trackedClusters[n] = val.GetPointNearestToCenter();
                            }
                        }
                    }

                    bool found = false;

                    for (int c = 0; c < clusters.Count();c++)
                    {
                        if (clusters[c].Any(i => i.ID == trackedClusters[n]))
                        {
                            var multiples = clusters[c].Where(z => trackedClusters.ContainsValue(z.ID));
                            if(multiples.Count() <= 1)
                            {
                                //this cluster is being tracked
                                SetClusterColor(clusters[c], n);
                                found = true;
                            }
                        }  
                    }                     

                    if (!found)
                    {
                        trackedClusters[n] = -1;
                    }
                }

                GenerateMessages();
                GenerateFilterResult();

                if (clusters.Count > 0)
                {
                    Cluster firstTrackedCluster = clusters.FirstOrDefault(c=>c.Any(i => i.ID == trackedClusters[0]));
#if WINDOWS

                    //SoundEngine.AgentDataRefresh(cluster.GetEveryOtherIndvd());
                    //SoundEngine.SendAgentEnergy(Normalizer.Normalize120To800(cluster.AverageAgentEnergy));
                    //SoundEngine.SendXYsymmetry(cluster.Symmetry);
                    //SoundEngine.SendNumAgents(cluster.Agents);
                    //SoundEngine.SendArea(cluster.Area);
                    //SoundEngine.SendClusterXY(cluster.Center.X, cluster.Center.Y);
                    //SoundEngine.StartCluster();
                    //SoundEngine.UpdateCluster(biggestCluster.Agents,
                    //                          biggestCluster.Center,
                    //                          biggestCluster.Area,
                    //                          Normalizer.Normalize0ToOne(biggestCluster.AverageAgentEnergy),
                    //                          biggestCluster.ClusterVelocity,
                    //                          new Vector3(biggestCluster.Symmetry.X, biggestCluster.Symmetry.Y, biggestCluster.Symmetry.Z));
                    //SoundEngine.StopCluster();
#endif
                    //SoundEngine.UpdateCluster(1, new Vector2(.1f, .2f), 1.1f, 1.1f, 1, new Vector3(1, 1, 1));
                    //SoundEngine.SendClusterXY(Normalizer.NormalizeWidthCentered(cluster.Center.X), Normalizer.NormalizeHeight(cluster.Center.Y));
                    //}
                    if (firstTrackedCluster != null)
                    {
                        SoundEngine.UpdateCluster(firstTrackedCluster.Agents,
                                                  firstTrackedCluster.Center,
                                                  firstTrackedCluster.Area,
                                                  Normalizer.Normalize0ToOne(firstTrackedCluster.AverageAgentEnergy),
                                                  firstTrackedCluster.ClusterVelocity,
                                                  new Vector3(firstTrackedCluster.Symmetry.X, firstTrackedCluster.Symmetry.Y, firstTrackedCluster.Symmetry.Z));
                    }
                }

                return analysis;
            }
            return null;
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
            clusters.RemoveAll(s => s.Count() <= clusterItemThreshhold);
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

            for (int c = 0; c < clusters.Count; c++)
            {
                lastfew = clusters[c].Skip(Math.Max(0, clusters[c].Count() - clusterBackCount)).Take(clusterBackCount).ToList();

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
                    clusters[clustersIds[0]].AddRange(clusters[clustersIds[1]]);
                    clusters.RemoveAt(clustersIds[1]);
                    clusters[clustersIds[0]].Add(individual);
                    return true;
                }
                clusters[clustersIds[0]].Add(individual);
                return true;
            }
            return false;
        }

        private void SetClusterColor(Cluster cluster, int persitedID)
        {
            foreach (Individual indvd in cluster)
            {
                if (persitedID == 0)
                {
                    indvd.setDisplayColor(Color.Red);
                }
                else if (persitedID == 1)
                {
                    indvd.setDisplayColor(Color.Green);
                }
                else if (persitedID == 2)
                {
                    indvd.setDisplayColor(Color.Yellow);
                }
                else if (persitedID == 3)
                {
                    indvd.setDisplayColor(Color.Blue);
                }
                else
                {
                    indvd.setDisplayColor(Color.DodgerBlue);
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

            for (int i = 0; i < clusters.Count; i++)
            {
                string clusterVisualCount = "";
                int reducedClusterCount = clusters[i].Count() / 5;
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
                string clusterSpeed = clusters[i].Average(x => (x.Dx2 * x.Dx2) + (x.Dy2 * x.Dy2)).ToString();

                //analysis.Messages.Add(new AnalysisMessage() { Type = this.ModuleName, Message = "COUNT : " + clusterVisualCount + "  " + reducedClusterCount * 5 + " SPEED : " + clusterSpeed });
            }
            analysis.Messages.Add(new AnalysisMessage() { Type = "              ", Message = "                                                  " });
        }

        private void GenerateFilterResult()
        {
            analysis.FilterResult = new FilterResult() { Type = FilterType.ClusterCenter, ClusterPoints = new List<Vector2>() };
            //TODO: need to send back analysis sysmtery points

            if (clusters.Count > 0)
            {
                Cluster biggestCluster = clusters.OrderBy(x => x.Area).First();
                analysis.Messages.Add(new AnalysisMessage() { Type = this.ModuleName, Message = "PERSISTED CLUSTERS " + trackedClusters.Count});
                analysis.Messages.Add(new AnalysisMessage() { Type = this.ModuleName, Message = "CLUSTER ONE        " + trackedClusters[0]});
                analysis.Messages.Add(new AnalysisMessage() { Type = this.ModuleName, Message = "CLUSTER TWO        " + trackedClusters[1]});
                analysis.Messages.Add(new AnalysisMessage() { Type = this.ModuleName, Message = "CLUSTER THREE      " + trackedClusters[2] });
                analysis.Messages.Add(new AnalysisMessage() { Type = this.ModuleName, Message = "CLUSTER FOUR       " + trackedClusters[3] });
                //analysis.Messages.Add(new AnalysisMessage() { Type = this.ModuleName, Message = "NORMALIZED AGENT ENERGY : " + Normalizer.Normalize0ToOne(biggestCluster.AverageAgentEnergy) });

                //analysis.Messages.Add(new AnalysisMessage() { Type = this.ModuleName, Message = "AVERAGE AGENT ENERGY : " + biggestCluster.AverageAgentEnergy });
                //analysis.Messages.Add(new AnalysisMessage() { Type = this.ModuleName, Message = "NORMALIZED AGENT ENERGY : " + Normalizer.Normalize0ToOne(biggestCluster.AverageAgentEnergy) });
                //analysis.Messages.Add(new AnalysisMessage() { Type = this.ModuleName, Message = "NORMALIZED SWARM ENERGY : " + Normalizer.Normalize0ToOne(biggestCluster.ClusterVelocity) });
                //analysis.Messages.Add(new AnalysisMessage() { Type = this.ModuleName, Message = "AGENT SYMMETRY : " + biggestCluster.Symmetry.X + ", " + biggestCluster.Symmetry.Y + ", " + biggestCluster.Symmetry.Z });
                //analysis.Messages.Add(new AnalysisMessage() { Type = this.ModuleName, Message = "AREA : " + biggestCluster.Area });
                //analysis.Messages.Add(new AnalysisMessage() { Type = this.ModuleName, Message = "CENTER : " + biggestCluster.NormalizedCenter.X + ", " + biggestCluster.NormalizedCenter.Y });
            }
        }
    }
}
