using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace SwarmEngine
{
    public class PopulationSimulator
    {
        public Population Population
        {
            get;
            private set;
        }
        private Random rand = new Random();
        Individual currentInd;
        Parameters param;
        Individual tempSwarm2;

        int minRankInXOrder, maxRankInXOrder, minRankInYOrder, maxRankInYOrder;

        double tempX, tempY, minX, maxX, minY, maxY, neighborhoodRadiusSquared;
        double tempAx, tempAy;
        double localCenterX = 0;
        double localCenterY = 0;
        double localDX = 0;
        double localDY = 0;
        double tempDX = 0;
        double tempDY = 0;

        double tempX2, tempY2;

        private Species swarmInBirthOrder, swarmInYOrder;
        private Species swarmInXOrder;


        #region Constuctor

        public PopulationSimulator(int width, int height)
        {
            swarmInBirthOrder = new Species();
            swarmInXOrder = new Species();
            swarmInYOrder = new Species();
            Population = new Population();

        }
        #endregion

        public void stepSimulation(List<Individual> temporaryIndividuals, int weightOfTemporaries)
        {
            int numberOfSwarm = swarmInBirthOrder.Count();
            updateInternalState();
            for (int i = 0; i < numberOfSwarm; i++)
            {
                currentInd = swarmInBirthOrder[i];
                param = currentInd.Genome;
                tempX = currentInd.X;
                tempY = currentInd.Y;

                neighborhoodRadiusSquared = param.getNeighborhoodRadius()
                        * param.getNeighborhoodRadius();

                List<Individual> neighbors = new List<Individual>();

                // Detecting neighbors using sorted lists
                minX = tempX - param.getNeighborhoodRadius();
                maxX = tempX + param.getNeighborhoodRadius();
                minY = tempY - param.getNeighborhoodRadius();
                maxY = tempY + param.getNeighborhoodRadius();
                minRankInXOrder = currentInd.RankInXOrder;
                maxRankInXOrder = currentInd.RankInXOrder; 
                minRankInYOrder = currentInd.RankInYOrder;
                maxRankInYOrder = currentInd.RankInYOrder;

                //TODO: Make this faster
                for (int j = currentInd.RankInXOrder - 1; j >= 0; j--)
                {
                    if (swarmInXOrder[j].X >= minX)
                        minRankInXOrder = j;
                    else
                        break;
                }
                for (int j = currentInd.RankInXOrder + 1; j < numberOfSwarm; j++)
                {
                    if (swarmInXOrder[j].X <= maxX)
                        maxRankInXOrder = j;
                    else
                        break;
                }
                for (int j = currentInd.RankInYOrder - 1; j >= 0; j--)
                {
                    if (swarmInYOrder[j].Y >= minY)
                        minRankInYOrder = j;
                    else
                        break;
                }
                for (int j = currentInd.RankInYOrder + 1; j < numberOfSwarm; j++)
                {
                    if (swarmInYOrder[j].Y <= maxY)
                        maxRankInYOrder = j;
                    else
                        break;
                }

                if (maxRankInXOrder - minRankInXOrder < maxRankInYOrder
                        - minRankInYOrder)
                {
                    for (int j = minRankInXOrder; j < maxRankInXOrder; j++)
                    {
                        tempSwarm2 = swarmInXOrder[j];
                        if (currentInd != tempSwarm2)
                            if (tempSwarm2.RankInYOrder >= minRankInYOrder
                                    && tempSwarm2.RankInYOrder <= maxRankInYOrder)
                            {
                                if ((tempSwarm2.X - currentInd.X)
                                        * (tempSwarm2.X - currentInd.X)
                                        + (tempSwarm2.Y - currentInd.Y)
                                        * (tempSwarm2.Y - currentInd.Y) < neighborhoodRadiusSquared)
                                    neighbors.Add(tempSwarm2);
                            }
                    }
                }
                else
                {
                    for (int j = minRankInYOrder; j < maxRankInYOrder; j++)
                    {
                        tempSwarm2 = swarmInYOrder[j];
                        if (currentInd != tempSwarm2)
                            if (tempSwarm2.RankInXOrder >= minRankInXOrder
                                    && tempSwarm2.RankInXOrder <= maxRankInXOrder)
                            {
                                if ((tempSwarm2.X - currentInd.X)
                                        * (tempSwarm2.X - currentInd.X)
                                        + (tempSwarm2.Y - currentInd.Y)
                                        * (tempSwarm2.Y - currentInd.Y) < neighborhoodRadiusSquared)
                                    neighbors.Add(tempSwarm2);
                            }
                    }
                }

                for (int k = 0; k < temporaryIndividuals.Count; k++)
                {
                    if ((temporaryIndividuals[k].X - currentInd.X)
                        * (temporaryIndividuals[k].X - currentInd.X)
                        + (temporaryIndividuals[k].Y - currentInd.Y)
                        * (temporaryIndividuals[k].Y - currentInd.Y) < neighborhoodRadiusSquared)
                        for (int j = 0; j < weightOfTemporaries; j++)
                            neighbors.Add(temporaryIndividuals[k]);
                }

                // simulating the behavior of swarm agents

                int n = neighbors.Count();

                if (n == 0)
                {

                    tempAx = rand.NextDouble() - 0.5;
                    tempAy = rand.NextDouble() - 0.5;
                }
                else
                {
                    localCenterX = 0;
                    localCenterY = 0;
                    localDX = 0;
                    localDY = 0;
                    for (int j = 0; j < n; j++)
                    {
                        tempSwarm2 = neighbors[j];
                        localCenterX += tempSwarm2.X;
                        localCenterY += tempSwarm2.Y;
                        localDX += tempSwarm2.Dx;
                        localDY += tempSwarm2.Dy;
                    }
                    localCenterX /= n;
                    localCenterY /= n;
                    localDX /= n;
                    localDY /= n;

                    tempAx = tempAy = 0;

                    tempAx += (localCenterX - tempX) * param.getC1();
                    tempAy += (localCenterY - tempY) * param.getC1();

                    tempAx += (localDX - currentInd.Dx) * param.getC2();
                    tempAy += (localDY - currentInd.Dy) * param.getC2();

                    for (int j = 0; j < n; j++)
                    {
                        tempSwarm2 = neighbors[j];
                        tempX2 = tempSwarm2.X;
                        tempY2 = tempSwarm2.Y;
                        double d = (tempX - tempX2) * (tempX - tempX2) + (tempY - tempY2)
                                * (tempY - tempY2);
                        if (d == 0)
                            d = 0.001;
                        tempAx += (tempX - tempX2) / d * param.getC3();
                        tempAy += (tempY - tempY2) / d * param.getC3();
                    }

                    //rand = new Random();
                    //if (rand.NextDouble() < param.getC4())
                    //{
                    //    tempAx += rand.NextDouble() * 10 - 5;
                    //    tempAy += rand.NextDouble() * 10 - 5;
                    //}
                }


                currentInd.accelerate(tempAx, tempAy, param.getMaxSpeed());

                tempDX = currentInd.Dx2;
                tempDY = currentInd.Dy2;
                double f = Math.Sqrt(tempDX * tempDX + tempDY * tempDY);
                if (f == 0)
                    f = 0.001;
                currentInd.accelerate(tempDX * (param.getNormalSpeed() - f) / f
                        * param.getC5(),
                        tempDY * (param.getNormalSpeed() - f) / f * param.getC5(),
                        param.getMaxSpeed());

            };
        }

        private void updateInternalState()
        {
            for (int i = 0; i < swarmInBirthOrder.Count(); i++)
            {
                swarmInBirthOrder[i].stepSimulation();
            }
            sortInternalLists();

            resetRanks();
        }

        public void EraseIndividual(Individual match)
        {
            if (Population.Sum(s => s.Count) >= 2 &&
                match != null)
            {
                swarmInXOrder.Remove(swarmInBirthOrder[swarmInBirthOrder.IndexOf(match)]);
                swarmInYOrder.Remove(swarmInBirthOrder[swarmInBirthOrder.IndexOf(match)]);
                Population.TryRemoveFromExisitingSpecies(match);
                swarmInBirthOrder.Remove(match);
            }

        }

        public void UndoIndividual()
        {
            if (Population.Sum(s => s.Count) >= 2)
            {
                swarmInXOrder.Remove(swarmInBirthOrder[swarmInBirthOrder.IndexOf(swarmInBirthOrder.Last())]);
                swarmInYOrder.Remove(swarmInBirthOrder[swarmInBirthOrder.IndexOf(swarmInBirthOrder.Last())]);
                Population.TryRemoveFromExisitingSpecies(swarmInBirthOrder.Last());
                swarmInBirthOrder.Remove(swarmInBirthOrder.Last());
            }
        }

        public void EmitIndividual(Individual indvd)
        {
            AddIndividual(indvd);
            if (Population.Sum(s => s.Count) > StaticWorldParameters.numberOfIndividualsMax -1)
            {
                swarmInXOrder.Remove(swarmInBirthOrder[swarmInBirthOrder.IndexOf(swarmInBirthOrder.First())]);
                swarmInYOrder.Remove(swarmInBirthOrder[swarmInBirthOrder.IndexOf(swarmInBirthOrder.First())]);
                Population.TryRemoveFromExisitingSpecies(swarmInBirthOrder.First());
                swarmInBirthOrder.Remove(swarmInBirthOrder.First());
            }
        }

        private void AddIndividual(Individual indvd)
        {
            if (Population.Sum(s => s.Count) < StaticWorldParameters.numberOfIndividualsMax)
            {
                InitCollections(indvd);
                Population.TryAddToExistingSpecies(indvd);
            }
        }

        private void InitCollections(Individual indvd)
        {
            swarmInBirthOrder.Add(indvd);
            swarmInXOrder.Add(indvd);
            swarmInYOrder.Add(indvd);
        }

        private void sortInternalLists()
        {
            swarmInXOrder.Sort((x, y) => x.X.CompareTo(y.X));
            swarmInYOrder.Sort((x, y) => x.Y.CompareTo(y.Y));
        }

        private void resetRanks()
        {
            for (int i = 0; i < swarmInXOrder.Count(); i++)
            {
                Individual tempSwarm = swarmInXOrder[i];
                if (tempSwarm.RankInXOrder != -1)
                    tempSwarm.setRankInXOrder(i);
                else
                    swarmInXOrder[i--] = null;
            }

            for (int i = 0; i < swarmInYOrder.Count(); i++)
            {
                Individual tempSwarm = swarmInYOrder[i];
                if (tempSwarm.RankInYOrder != -1)
                    tempSwarm.setRankInYOrder(i);
                else
                    swarmInYOrder[i--] = null;
            }
        }

        public List<Individual> GetSwarmInXOrder()
        {
            //sortInternalLists();
            return swarmInXOrder;
        }

        public List<Individual> GetSwarmInYOrder()
        {
            //sortInternalLists();
            return swarmInYOrder;
        }

        public List<Individual> GetSwarmInBirthOrder()
        {
            //sortInternalLists();
            return swarmInBirthOrder;
        }

        private void ClearPopulation()
        {
            swarmInBirthOrder.Clear();
            swarmInXOrder.Clear();
            swarmInYOrder.Clear();
        }
        //public void UpdatePopulation(string recipiText, bool mutate)
        //{
        //    foreach(var spec in new Population(new Recipe(recipiText).CreatePopulation(0, 0), "CrumpulaAtion"))
        //    {
        //        foreach (var indvd in spec)
        //        {
        //            EmitIndividual(indvd);
        //        }
        //    }
        //    UpdatePopulation(mutate);
        //}

        //public void UpdatePopulation(bool mutate)
        //{
        //    //ClearPopulation();

        //    for (int i = 0; i < Population.Count; i++)
        //    {
        //        for (int j = 0; j < Population[i].Count; j++)
        //        {
        //            if (mutate)
        //            {
        //                Population[i][j].Genome.inducePointMutations(rand.NextDouble(), 1);
        //            }
        //        }
        //    }

        //    if (mutate)
        //    {
        //        Population.ReassignSpecies();
        //        Population.ReassignAllColors();
        //    }

        //    //Population = tempPopulation;
        //}
    }
}

