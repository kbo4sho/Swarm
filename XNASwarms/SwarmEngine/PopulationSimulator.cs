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
        public PopulationSimulator(int width, int height, params Recipe[] recipes)
            :this (width, height, new Population(recipes[0].CreatePopulation(width, height), "CrumpulaAtion"))
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
                    AddIndividual(population[i][j]);
                }
            }
        }
        #endregion

        public void stepSimulation(List<Individual> temporaryIndividuals, int weightOfTemporaries)
        {
            int numberOfSwarm = swarmInBirthOrder.Count();

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
                minRankInYOrder = currentInd.getRankInYOrder();
                maxRankInYOrder = currentInd.getRankInYOrder();

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

                tempDX = currentInd.getDx2();
                tempDY = currentInd.getDy2();
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

        public void EmitIndividual(Individual indvd)
        {
            if (population.Count > 0)
            {
                
                swarmInXOrder.Remove(swarmInBirthOrder[swarmInBirthOrder.IndexOf(swarmInBirthOrder.First())]);
                swarmInYOrder.Remove(swarmInBirthOrder[swarmInBirthOrder.IndexOf(swarmInBirthOrder.First())]);
                swarmInBirthOrder.Remove(swarmInBirthOrder.First());
                population.TryRemoveFromExisitingSpecies();
            }
            AddIndividual(indvd);
        }

        private void AddIndividual(Individual indvd)
        {
            swarmInBirthOrder.Add(indvd);
            swarmInXOrder.Add(indvd);
            swarmInYOrder.Add(indvd);
            population.TryAddToExistingSpecies(indvd);
        }

        private void sortInternalLists()
        {
            //swarmInXOrder = (from entry in swarmInXOrder orderby entry.Value.X descending select entry)
            //.ToDictionary(pair => pair.Key, pair => pair.Value);

            //swarmInYOrder = ((from entry in swarmInYOrder orderby entry.Y descending select entry)
            //.ToList<Individual>()) as Species;


            swarmInXOrder.Sort((x, y) => x.X.CompareTo(y.X));
            swarmInYOrder.Sort((x, y) => x.Y.CompareTo(y.Y));
            
            //swarmInYOrder.OrderBy(x => x.Y).ToList<Individual>();
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
                if (tempSwarm.getRankInYOrder() != -1)
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

        public Population GetPopulation()
        {
            return population;
        }

        public List<Individual> GetSwarmSortedBySpecies()
        {
            List<Individual> allSpecies = new List<Individual>();
            for (int i = 0; i < population.Count(); i++)
            {
                allSpecies.AddRange(population[i].ToList());
            }
            return allSpecies.OrderBy(s=>s.Genome).ToList();
        }

        public void DetermineSpecies()
        {
            //Reorganize the population in to a species
            List<string> discoverdRecipies = population.Select(s => s.Select(d => d.Genome.getRecipe()).Distinct().ToList<string>()).First();

            if (discoverdRecipies != null && discoverdRecipies.Count() > 0)
            {
                List<Species> species = new List<Species>();
                foreach (var rcpe in discoverdRecipies)
                {
                    species.Add(new Species(population.Select(s => s.Where(t => t.Genome.getRecipe().ToString() == rcpe.ToString()).ToList()).First()));
                }

                population.Clear();
                population.AddRange(species);
            }
            
            
        }

        private void ClearPopulation()
        {
            swarmInBirthOrder.Clear();
            swarmInXOrder.Clear();
            swarmInYOrder.Clear();
        }

        public void UpdatePopulation(string recipiText, bool mutate)
        {
            ClearPopulation();
            population = new Population(new Recipe(recipiText).CreatePopulation(0, 0), "CrumpulaAtion");
            UpdatePopulation(population, mutate);
        }

        public void UpdatePopulation(Population tempPopulation, bool mutate)
        {
            ClearPopulation();
            population = tempPopulation;
            for (int i = 0; i < population.Count; i++)
            {
                for (int j = 0; j < population[i].Count; j++)
                {
                    if (mutate)
                    {
                        population[i][j].Genome.inducePointMutations(rand.NextDouble(), 1);
                    }
                    AddIndividual(population[i][j]);
                }
            }
        }
    }
}

