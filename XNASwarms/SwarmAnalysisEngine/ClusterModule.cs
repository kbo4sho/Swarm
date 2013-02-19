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
            bool IsNewCluster = false;
            List<Cluster> tempClusters = new List<Cluster>();

            if (Clusters.Count() == 0)
            {
                Clusters.Add(new Cluster() { indvds.First() });
            }
            tempClusters.Clear();

            Clusters.Select(s => s.MinHeight = 0);
            Clusters.Select(s => s.MaxHeight = 0);

            foreach (Individual individual in indvds)
            {
                Vector2 indvdPosition = new Vector2((float)individual.getX(), (float)individual.getY());
                Vector2 beforeIndvdPosition = new Vector2((float)oneBeforeIndividiual.getX(), (float)oneBeforeIndividiual.getY());

                //ReadOut.Add(new AnalysisResult() { Type = this.ModuleName, Message = "INDVIDUAL # " + individual.getRankInXOrder() });

                for (int c = 0; c < Clusters.Count; c++)
                {
                    tempClusters.Clear();

                    #region Check Last Ten
                    var lastTenAdded = Clusters[c].Skip(Math.Max(0, Clusters[c].Count() - 20)).Take(20);

                    foreach (Individual indvd in lastTenAdded)
                    {
                        double newdis = (individual.getX() - indvd.getX()) * (individual.getX() - indvd.getX()) + (individual.getY() - indvd.getY()) * (individual.getY() - indvd.getY());
                        IsNewCluster = true;
                        if (newdis < indvd.getGenome().getNeighborhoodRadius() * indvd.getGenome().getNeighborhoodRadius())//||oneBeforePosition.Y < Clusters[c].MaxHeight && oneBeforePosition.Y > Clusters[c].MinHeight)
                        {
                            //ReadOut.Add(new AnalysisResult() { Type = this.ModuleName, Message = "ADDED ONE TO CLUSTER # " + c });
                            Clusters[c].Add(individual);
                            IsNewCluster = false;
                            break;

                        }
                        else
                        {
                            //ReadOut.Add(new AnalysisResult() { Type = this.ModuleName, Message = "NO ACTION TO CLUSTER # " + c });
                            //Clusters[c].Add(individual);
                        }
                    }
                    #endregion
                    //foreach (Individual indvd in Clusters[c])
                    //{
                    //    double newdis = (individual.getX() - indvd.getX()) * (individual.getX() - indvd.getX()) + (individual.getY() - indvd.getY()) * (individual.getY() - indvd.getY());

                    //    if (newdis < oneBeforeIndividiual.getGenome().getNeighborhoodRadius())//||oneBeforePosition.Y < Clusters[c].MaxHeight && oneBeforePosition.Y > Clusters[c].MinHeight)
                    //    {
                    //        ReadOut.Add(new AnalysisResult() { Type = this.ModuleName, Message = "ADDED NEIGHBOR" });
                    //        Clusters[c].Add(individual);

                    //    }
                    //    else if (c != 0 && !Clusters[c].Contains(individual))
                    //    {
                    //        IsNewCluster = true;
                    //        ReadOut.Add(new AnalysisResult() { Type = this.ModuleName, Message = "NEW CLUSTER" });
                    //        ReadOut.Add(new AnalysisResult() { Type = this.ModuleName, Message = "CLUSTER MAX HEIGHT: " + Clusters[c].MaxHeight });
                    //        ReadOut.Add(new AnalysisResult() { Type = this.ModuleName, Message = "CLUSTER MIN HEIGHT: " + Clusters[c].MinHeight });
                    //    }
                    //    else
                    //    {
                    //        ReadOut.Add(new AnalysisResult() { Type = this.ModuleName, Message = "ADDED NON NEIGHBOR" });
                    //        Clusters[c].Add(individual);
                    //    }
                    //}

                    

                    if (IsNewCluster)
                    {
                        tempClusters.Add(new Cluster() { individual });
                    }

                    //if (Clusters[c].Count > 0)
                    //{
                    //    if (individual.getY() > Clusters[c].OrderBy(s => s.getY()).First().getY())
                    //    {
                    //        Clusters[c].MaxHeight = individual.getY();
                    //    }
                    //    if (individual.getY() < Clusters[c].OrderByDescending(s => s.getY()).First().getY())
                    //    {
                    //        Clusters[c].MinHeight = individual.getY();
                    //    }
                    //}

                }



                if (tempClusters.Count > 0)
                {
                    Clusters.AddRange(tempClusters);
                }

                //oneBeforeIndividiual = individual;
            }

           


            //Add my tempCluster back in to the main stack
            if (tempClusters.Count > 0)
            {
                Clusters.AddRange(tempClusters);
            }

            ReadOut.Add(new AnalysisResult() { Type = this.ModuleName, Message = "////////////////////////////////////////////" });
            ReadOut.Add(new AnalysisResult() { Type = this.ModuleName, Message = "# OF CLUSTERS " + Clusters.Count().ToString() });
            ReadOut.Add(new AnalysisResult() { Type = this.ModuleName, Message = "////////////////////////////////////////////" });
            return ReadOut;// Clusters.Count().ToString() + "///" + returnstring;
        }


        
    }
}
