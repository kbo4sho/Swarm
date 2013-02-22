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
            get
            {
                return "Cluster Module";
            }
        }

        int ClusterItemThreshhold = 5;
        int ClusterBackCount = 10;

        public List<Cluster> Clusters;

        public ClusterModule()
        {
            Clusters = new List<Cluster>();
        }
        
        public List<AnalysisResult> Analyze(List<Individual> indvds)
        {
            return GetClustersReadOut(indvds);
        }

        private List<AnalysisResult> GetClustersReadOut(List<Individual> indvds)
        {
            List<AnalysisResult> ReadOut = new List<AnalysisResult>(); 
            Clusters.Clear();

            if (Clusters.Count() == 0)
            {
                Clusters.Add(new Cluster() { indvds.First() });
            }

            foreach (Individual individual in indvds)
            {
                if(!InExistingCluster(individual))
                {
                    Clusters.Add(new Cluster(){individual});
                }
            }

            RemoveSmallClusters();

            ReadOut.Add(new AnalysisResult() { Type = this.ModuleName, Message = "# OF CLUSTERS : " + Clusters.Count().ToString() });
            
            return ReadOut;
        }

        private void RemoveSmallClusters()
        {
            Clusters.RemoveAll(s => s.Count() <= ClusterItemThreshhold);
        }


        private bool InExistingCluster(Individual individual)
        {
            for (int c = 0; c < Clusters.Count; c++)
            {
                var lastFew = Clusters[c].Skip(Math.Max(0, Clusters[c].Count() - ClusterBackCount)).Take(ClusterBackCount);

                foreach (Individual indvd in lastFew)
                {
                    double newdis = (individual.getX() - indvd.getX()) * (individual.getX() - indvd.getX()) + (individual.getY() - indvd.getY()) * (individual.getY() - indvd.getY());

                    if (newdis < indvd.getGenome().getNeighborhoodRadius() * indvd.getGenome().getNeighborhoodRadius())
                    {
                        Clusters[c].Add(individual);
                        return true;
                    }
                }
            }
            return false;
        }        
    }
}
