using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwarmEngine
{
    public class PopulationSimulator
    {
        private Population population;
        Random rand = new Random();
        Individual currentInd;
        Parameters param;
        Individual tempSwarm2;

        private Species swarmInBirthOrder, swarmInXOrder, swarmInYOrder;

        #region Constuctor
        //public PopulationSimulator(Population newPopulation)
        //{
        //    population = newPopulation;

        //    swarmInBirthOrder = new List<Individual>(population.size());
        //    swarmInXOrder = new List<Individual>(population.size());
        //    swarmInYOrder = new List<Individual>(population.size());

        //    for (int i = 0; i < population.Count; i++)
        //    {
        //        for (int j = 0; j < population[i].Count; j++)
        //        {

        //            addIndividual(population[i][j]);
        //        }

        //    }
        //}

        public PopulationSimulator(int width, int height, params Recipe[] recipes)
            :this (width, height, new Population(recipes[0].createPopulation(width, height), "CrumpulaAtion"))
        {
        }

        public PopulationSimulator(int width, int height, Population pop)
        {
            population = pop;

            swarmInBirthOrder = new Species();
            swarmInXOrder = new Species();
            swarmInYOrder = new Species();

            for (int i = 0; i < population.Count; i++)
            {
                for (int j = 0; j < population[i].Count; j++)
                {
                    addIndividual(population[i][j]);
                }
            }
        }
        #endregion

        public void stepSimulation(List<Individual> temporaryIndividuals, int weightOfTemporaries)
        {
            int numberOfSwarm = swarmInBirthOrder.Count();

            int minRankInXOrder, maxRankInXOrder, minRankInYOrder, maxRankInYOrder;

            double tempX, tempY, minX, maxX, minY, maxY, neighborhoodRadiusSquared;
            double tempAx, tempAy;

            double tempX2, tempY2;

            //Parallel.For(0, numberOfSwarm, i => //might need to put back all the instance decalartions to use Parallel might be faster this way
            for (int i = 0; i < numberOfSwarm; i++)
            {
                currentInd = swarmInBirthOrder[i];
                param = currentInd.getGenome();
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
                minRankInXOrder = currentInd.getRankInXOrder();
                maxRankInXOrder = currentInd.getRankInXOrder();
                minRankInYOrder = currentInd.getRankInYOrder();
                maxRankInYOrder = currentInd.getRankInYOrder();

                //TODO: Make this faster
                for (int j = currentInd.getRankInXOrder() - 1; j >= 0; j--)
                {
                    if (swarmInXOrder[j].X >= minX)
                        minRankInXOrder = j;
                    else
                        break;
                }
                for (int j = currentInd.getRankInXOrder() + 1; j < numberOfSwarm; j++)
                {
                    if (swarmInXOrder[j].X <= maxX)
                        maxRankInXOrder = j;
                    else
                        break;
                }
                for (int j = currentInd.getRankInYOrder() - 1; j >= 0; j--)
                {
                    if (swarmInYOrder[j].Y >= minY)
                        minRankInYOrder = j;
                    else
                        break;
                }
                for (int j = currentInd.getRankInYOrder() + 1; j < numberOfSwarm; j++)
                {
                    if (swarmInYOrder[j].Y <= maxY)
                        maxRankInYOrder = j;
                    else
                        break;
                }

                if (maxRankInXOrder - minRankInXOrder < maxRankInYOrder
                        - minRankInYOrder)
                {
                    for (int j = minRankInXOrder; j <= maxRankInXOrder; j++)
                    {
                        tempSwarm2 = swarmInXOrder[j];
                        if (currentInd != tempSwarm2)
                            if (tempSwarm2.getRankInYOrder() >= minRankInYOrder
                                    && tempSwarm2.getRankInYOrder() <= maxRankInYOrder)
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
                    for (int j = minRankInYOrder; j <= maxRankInYOrder; j++)
                    {
                        tempSwarm2 = swarmInYOrder[j];
                        if (currentInd != tempSwarm2)
                            if (tempSwarm2.getRankInXOrder() >= minRankInXOrder
                                    && tempSwarm2.getRankInXOrder() <= maxRankInXOrder)
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
                    double localCenterX = 0;
                    double localCenterY = 0;
                    double localDX = 0;
                    double localDY = 0;
                    for (int j = 0; j < n; j++)
                    {
                        tempSwarm2 = neighbors[j];
                        localCenterX += tempSwarm2.X;
                        localCenterY += tempSwarm2.Y;
                        localDX += tempSwarm2.getDx();
                        localDY += tempSwarm2.getDy();
                    }
                    localCenterX /= n;
                    localCenterY /= n;
                    localDX /= n;
                    localDY /= n;

                    tempAx = tempAy = 0;

                    tempAx += (localCenterX - tempX) * param.getC1();
                    tempAy += (localCenterY - tempY) * param.getC1();

                    tempAx += (localDX - currentInd.getDx()) * param.getC2();
                    tempAy += (localDY - currentInd.getDy()) * param.getC2();

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

                double tempDX = currentInd.getDx2();
                double tempDY = currentInd.getDy2();
                double f = Math.Sqrt(tempDX * tempDX + tempDY * tempDY);
                if (f == 0)
                    f = 0.001;
                currentInd.accelerate(tempDX * (param.getNormalSpeed() - f) / f
                        * param.getC5(),
                        tempDY * (param.getNormalSpeed() - f) / f * param.getC5(),
                        param.getMaxSpeed());

            };

            updateInternalState();
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

        public void addIndividual(Individual b)
        {
            swarmInBirthOrder.Add(b);

            swarmInXOrder.Add(b);
            swarmInYOrder.Add(b);

            //population.Add(b);

            sortInternalLists();

            //if (swarmInXOrder.Count < 1) 
            //{
            //    swarmInXOrder.Add(b);
            //    swarmInYOrder.Add(b);
            //} else {
            //    zif ((b.getX() - swarmInXOrder[0].getX()) < (swarmInXOrder[swarmInXOrder.Count - 1].getX() - b.getX())) 
            //    {
            //        int i = 0;
            //        while (i < swarmInXOrder.Count()) 
            //        {
            //            if (swarmInXOrder[i].getX() >= b.getX())
            //                break;
            //            i++;
            //        }
            //        swarmInXOrder.Insert(i, b);
            //    } 
            //    else 
            //    {
            //        int i = swarmInXOrder.Count;
            //        while (i > 0) 
            //        {
            //            if (swarmInXOrder[i - 1].getX() <= b.getX())
            //                break;
            //            i--;
            //        }
            //        swarmInXOrder.Insert(i, b);
            //    }

            //    if ((b.getY() - swarmInYOrder[0].getY()) < (swarmInYOrder[swarmInYOrder.Count - 1].getY() - b.getY())) 
            //    {
            //        int i = 0;
            //        while (i < swarmInYOrder.Count) 
            //        {
            //            if (swarmInYOrder[i].getY() >= b.getY())
            //                break;
            //            i++;
            //        }
            //        swarmInYOrder.Insert(i, b);
            //    } 
            //    else 
            //    {
            //        int i = swarmInYOrder.Count;
            //        while (i > 0) 
            //        {
            //            if (swarmInYOrder[i - 1].getY() <= b.getY())
            //                break;
            //            i--;
            //        }
            //        swarmInYOrder.Insert(i, b);
            //    }

            //}
        }

        private void sortInternalLists()
        {
            swarmInXOrder.Sort((x, y) => x.X.CompareTo(y.X));
            swarmInYOrder.Sort((x, y) => x.Y.CompareTo(y.Y));
            //swarmInXOrder = new Species(swarmInXOrder.AsParallel().OrderByDescending(x => x.getX()).ToList<Individual>());
            //swarmInYOrder = new Species(swarmInYOrder.AsParallel().OrderByDescending(x => x.getY()).ToList<Individual>());
        }


        //public void removeIndividual(Individual b)
        //{
        //    swarmInBirthOrder.Remove(b);
        //    swarmInXOrder.Remove(b);
        //    swarmInYOrder.Remove(b);
        //    population.Remove(b);
        //}

        private void resetRanks()
        {
            for (int i = 0; i < swarmInXOrder.Count(); i++)
            {
                Individual tempSwarm = swarmInXOrder[i];
                if (tempSwarm.getRankInXOrder() != -1)
                    tempSwarm.setRankInXOrder(i);
                else
                    swarmInXOrder[i--] = null;
            }

            for (int i = 0; i < swarmInYOrder.Count(); i++)
            {
                Individual tempSwarm = swarmInYOrder[i];
                if (tempSwarm.getRankInYOrder() != -1)
                    tempSwarm.setRankInYOrder(i);
                else
                    swarmInYOrder[i--] = null;
            }
        }

        public List<Individual> GetSwarmInXOrder()
        {
            sortInternalLists();
            return swarmInXOrder;
        }

        public List<Individual> GetSwarmInYOrder()
        {
            sortInternalLists();
            return swarmInYOrder;
        }

        public List<Individual> GetSwarmInBirthOrder()
        {
            sortInternalLists();
            return swarmInBirthOrder;
        }

        //public List<Individual> GetNeighbors()
        //{
        //    return neighbors;
        //}



        public Population GetPopulation()
        {
            return population;
        }

        public List<Individual> GetSwarmSortedBySpecies()
        {
            List<Individual> allSpecies = new List<Individual>();
            foreach (Species spcs in population)
            {
                allSpecies.AddRange(spcs.ToList());
            }
            return allSpecies.OrderBy(s=>s.getGenome()).ToList();
        }

        public void DetermineSpecies()
        {
            //Reorganize the population in to a species
            List<string> discoverdRecipies = population.AsParallel().Select(s => s.Select(d => d.getGenome().getRecipe()).Distinct().ToList<string>()).First();

            if (discoverdRecipies != null && discoverdRecipies.Count() > 0)
            {
                List<Species> species = new List<Species>();
                foreach (var rcpe in discoverdRecipies)
                {
                    species.Add(new Species(population.AsParallel().Select(s => s.Where(t => t.getGenome().getRecipe().ToString() == rcpe.ToString()).ToList()).First()));
                }

                population.Clear();
                population.AddRange(species);
            }
            
            
        }
    }
}

