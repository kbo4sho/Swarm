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
            Individual oneBeforeIndividiual = new Individual();
            string returnstring = "";
            bool IsNewCluster = false;
            List<Cluster> tempClusters = new List<Cluster>();

            if (Clusters.Count() == 0)
            {
                Clusters.Add(new Cluster());
            }
            tempClusters.Clear();

            Clusters.Select(s => s.MinHeight = 0);
            Clusters.Select(s => s.MaxHeight = 0);
            foreach (Individual individual in indvds)
            {
              
                foreach(Cluster cluster in Clusters)
                {
                    //tempClusters.Clear();
                    Vector2 indvdPosition = new Vector2((float)individual.getX(),(float)individual.getY());
                    Vector2 beforeIndvdPosition =  new Vector2((float)oneBeforeIndividiual.getX(),(float)oneBeforeIndividiual.getY());
                    double distance = Vector2.Distance(indvdPosition,beforeIndvdPosition);

                    double newdis = (individual.getX() - oneBeforeIndividiual.getX()) * (individual.getX() - oneBeforeIndividiual.getX()) + (individual.getY() - oneBeforeIndividiual.getY()) * (individual.getY() - oneBeforeIndividiual.getY());
                    double oneBeforeNeighborhood = oneBeforeIndividiual.getX() + (oneBeforeIndividiual.getGenome().getNeighborhoodRadius() * oneBeforeIndividiual.getGenome().getNeighborhoodRadius());

                    Vector2 oneBeforePosition = new Vector2((float)oneBeforeIndividiual.getX(),(float)oneBeforeIndividiual.getY());

                    //if (cluster.Count > 0)
                    //{
                    //    if (individual.getY() > cluster.OrderBy(s => s.getY()).First().getY())
                    //    {
                    //        cluster.MaxHeight = individual.getY();
                    //    }
                    //    if (individual.getY() < cluster.OrderByDescending(s => s.getY()).First().getY())
                    //    {
                    //        cluster.MinHeight = individual.getY();
                    //    }
                    //}

                    if (newdis < oneBeforeIndividiual.getGenome().getNeighborhoodRadius() * oneBeforeIndividiual.getGenome().getNeighborhoodRadius() ||
                        oneBeforePosition.Y < cluster.MaxHeight && oneBeforePosition.Y > cluster.MinHeight)
                    {
                        ReadOut.Add(new AnalysisResult() { Type = this.ModuleName, Message = "ADDED NEIGHBOR" });
                        cluster.Add(individual);
                    }
                    else if (!cluster.Contains(individual))
                    {
                        IsNewCluster = true;
                        ReadOut.Add(new AnalysisResult() { Type = this.ModuleName, Message = "NEW CLUSTER" });
                        ReadOut.Add(new AnalysisResult() { Type = this.ModuleName, Message = "CLUSTER MAX HEIGHT: " + cluster.MaxHeight });
                        ReadOut.Add(new AnalysisResult() { Type = this.ModuleName, Message = "CLUSTER MIN HEIGHT: " + cluster.MinHeight });
                    }
                }

                if (IsNewCluster)
                {
                    //tempClusters.Add(new Cluster() { individual });
                }

                oneBeforeIndividiual = individual;
            }

           


            //Add my tempCluster back in to the main stack
            if (tempClusters.Count > 0)
            {
                //Clusters.AddRange(tempClusters);
            }

            ReadOut.Add(new AnalysisResult() { Type = this.ModuleName, Message = Clusters.Count().ToString() });
            return ReadOut;// Clusters.Count().ToString() + "///" + returnstring;
        }


        
    }
}
