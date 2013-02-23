using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SwarmEngine;
using Microsoft.Xna.Framework;

namespace SwarmAnalysisEngine
{
    public class ClusterModule : IAnalysisModule
    {
        public string ModuleName
        {
            get { return "Cluster Module"; }
        }

        int ClusterItemThreshhold = 5;
        int ClusterBackCount = 100;
        List<AnalysisResult> ReadOut = new List<AnalysisResult>(); 
        public List<Cluster> Clusters;

        public ClusterModule()
        {
            Clusters = new List<Cluster>();
            List<AnalysisResult> ReadOut = new List<AnalysisResult>(); 
        }
        
        public List<AnalysisResult> Analyze(List<Individual> indvds)
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

                //TODO: Check and merge clusters that can be connected
                foreach (Cluster cluster in Clusters)
                {

                }
            }

            RemoveSmallClusters();

            return GenerateMessage();
        }

        private void RemoveSmallClusters()
        {
            Clusters.RemoveAll(s => s.Count() <= ClusterItemThreshhold);
        }


        private bool InExistingCluster(Individual individual)
        {
            for (int c = 0; c < Clusters.Count; c++)
            {
                var lastFew = Clusters[c].Skip(Math.Max(0, Clusters[c].Count() - ClusterBackCount)).Take(ClusterBackCount).ToList();

                for (int i = 0; i < lastFew.Count();i++ )
                {
                    double newdis = (individual.getX() - lastFew[i].getX()) * (individual.getX() - lastFew[i].getX()) + (individual.getY() - lastFew[i].getY()) * (individual.getY() - lastFew[i].getY());

                    if (newdis < lastFew[i].getGenome().getNeighborhoodRadius() * lastFew[i].getGenome().getNeighborhoodRadius())
                    {
                        Clusters[c].Add(individual);
                        return true;
                    }
                }
            }
            return false;
        }

        private List<AnalysisResult> GenerateMessage()
        {
            for (int i = 0; i < Clusters.Count; i++)
            {
                string clusterVisualCount = "";
                for (int c = 0; c < Clusters[i].Count() / 4; c++)
                {
                    clusterVisualCount += "+";
                }
                ReadOut.Add(new AnalysisResult() { Type = this.ModuleName, Message = "CLUSTER # " + (i + 1) + " : COUNT : " + clusterVisualCount });
            }
            ReadOut.Add(new AnalysisResult() { Type = "              ", Message = "                                                  " });
            return ReadOut;
        }
    }
}
