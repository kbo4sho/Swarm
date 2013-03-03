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
        List<AnalysisResult> ReadOut = new List<AnalysisResult>(); 
        public List<Cluster> Clusters;

        public ClusterModule()
            : base("Cluster Module", 2)
        {
            Clusters = new List<Cluster>();
            List<AnalysisResult> ReadOut = new List<AnalysisResult>(); 
        }
        
        protected override List<AnalysisResult> Analyze(List<Individual> indvds)
        {
            return GetClustersReadOut(indvds);
        }

        private List<AnalysisResult> GetClustersReadOut(List<Individual> indvds)
        {
            
            Clusters.Clear();

            Clusters.Add(new Cluster() { indvds[0] });

            for (int i = 0; i < indvds.Count; i++)
            {
                if (!InExistingCluster(indvds[i]))
                {
                    Clusters.Add(new Cluster() { indvds[i] });
                }
            }

            RemoveSmallClusters();
            SetClusterColor();
            return GenerateMessage();
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
                        indvd.setDisplayColor(Color.Blue);
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
                        indvd.setDisplayColor(Color.Green);
                    }
                    else
                    {
                        indvd.setDisplayColor(Color.Lerp(Color.Green, Color.Cyan, (float)(clusterid*.05)));
                    }
                }
            }

        }

        private List<AnalysisResult> GenerateMessage()
        {
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
                ReadOut.Add(new AnalysisResult() { Type = this.ModuleName, Message = "COUNT : " + clusterVisualCount + "  " + reducedClusterCount * 5  });
            }
            ReadOut.Add(new AnalysisResult() { Type = "              ", Message = "                                                  " });
            return ReadOut;
        }
    }
}
