using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SwarmEngine;
using Microsoft.Xna.Framework;

namespace SwarmAnalysisEngine
{
    public class ClusterModule : AnalysisModule
    {
        int ClusterItemThreshhold = 5;
        int ClusterBackCount = 30;
        List<AnalysisMessage> ReadOut = new List<AnalysisMessage>(); 
        public List<Cluster> Clusters;
        private Analysis analysis;

        public ClusterModule()
            : base("Cluster Module", 8)
        {
            Clusters = new List<Cluster>();
            List<AnalysisMessage> ReadOut = new List<AnalysisMessage>();
            analysis = new Analysis();
        }
        
        protected override Analysis Analyze(List<Individual> indvds)
        {
            return DoAnalysis(indvds);
        }

        private void ResetColor(Individual indvd)
        {
            indvd.setDisplayColor(Color.MidnightBlue);
        }


        private Analysis DoAnalysis(List<Individual> indvds)
        {
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

            analysis.Messages = GenerateMessage();
            analysis.FilterResult = GenerateFilterResult();

            return analysis;
        }

        private void RemoveSmallClusters()
        {
            Clusters.RemoveAll(s => s.Count() <= ClusterItemThreshhold);
        }

        private bool InExistingCluster(Individual individual)
        {
            List<int> clustersIds = new List<int>();

            for (int c = 0; c < Clusters.Count; c++)
            {
                var lastfew = Clusters[c].Skip(Math.Max(0, Clusters[c].Count() - ClusterBackCount)).Take(ClusterBackCount).ToList();

                for (int i = 0; i < lastfew.Count(); i++)
                {
                    double newdis = (individual.getX() - lastfew[i].getX()) * (individual.getX() - lastfew[i].getX()) + (individual.getY() - lastfew[i].getY()) * (individual.getY() - lastfew[i].getY());

                    if (newdis < lastfew[i].getGenome().getNeighborhoodRadius() * lastfew[i].getGenome().getNeighborhoodRadius())
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
                        indvd.setDisplayColor(Color.Lerp(Color.LightPink,Color.LightBlue,clusterid*.15f));
                    }
                    else
                    {
                        indvd.setDisplayColor(Color.Lerp(Color.Green, Color.Cyan, (float)(clusterid*.05)));
                    }
                }
            }
        }

        private List<AnalysisMessage> GenerateMessage()
        {
            ReadOut.Clear();

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
                ReadOut.Add(new AnalysisMessage() { Type = this.ModuleName, Message = "COUNT : " + clusterVisualCount + "  " + reducedClusterCount * 5  });
            }
            ReadOut.Add(new AnalysisMessage() { Type = "              ", Message = "                                                  " });
            return ReadOut;
        }

        private FilterResult GenerateFilterResult()
        {
            FilterResult filterresult = new FilterResult() { Type = FilterType.ClusterCenter, ClusterCenters = new List<Vector2>() };
            foreach (Cluster cluster in Clusters)
            {
                double leftindvd = cluster.OrderBy(point => point.getX()).First().getX();
                double rightindvd = cluster.OrderByDescending(point => point.getX()).First().getX();
                double topindvd = cluster.OrderBy(point => point.getY()).First().getY();
                double bottomindvd = cluster.OrderByDescending(point => point.getY()).First().getY();

                filterresult.ClusterCenters.Add(new Vector2((float)(leftindvd + rightindvd)*.5f,(float)(topindvd + bottomindvd)*.5f));
            }
            return filterresult;

        }
    }
}
