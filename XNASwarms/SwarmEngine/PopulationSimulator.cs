using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace SwarmEngine
{
    public class PopulationSimulator
    {
        private Population population;
        Random rand = new Random();

        private List<Individual> swarmInBirthOrder, swarmInXOrder,
                swarmInYOrder;

        #region Constuctor
        public PopulationSimulator(Population newPopulation)
        {
            population = newPopulation;

            swarmInBirthOrder = new List<Individual>(population.size());
            swarmInXOrder = new List<Individual>(population.size());
            swarmInYOrder = new List<Individual>(population.size());

            for (int i = 0; i < population.Count; i++)
            {
                for (int j = 0; j < population[i].Count; j++)
                {

                    addIndividual(population[i][j]);
                }

            }
        }

        public PopulationSimulator(int width, int height, params Recipe[] recipes)
        {

            population = new Population(recipes[0].createPopulation(width, height), "CrumpulaAtion");

            //Population pop1 = new Population(recipes[1].createPopulation(width, height), "Crumpulation");
            //population = new Population(pop, pop1, 1, 10, 10, "CrumpulatTion");

            swarmInBirthOrder = new List<Individual>(population.size());
            swarmInXOrder = new List<Individual>(population.size());
            swarmInYOrder = new List<Individual>(population.size());

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
            //Individual tempSwarm, tempSwarm2;
            //Parameters param;

            //double tempX, tempY, tempX2, tempY2, tempDX, tempDY;
            //double localCenterX, localCenterY, localDX, localDY, tempAx, tempAy, d;
            //int n;

            List<Individual> neighbors = new List<Individual>();

            int numberOfSwarm = swarmInBirthOrder.Count();

            for (int i = 0; i < numberOfSwarm; i++)
            {
                Individual currentInd = swarmInBirthOrder[i];
                Parameters param = currentInd.getGenome();
                double tempX = currentInd.getX();
                double tempY = currentInd.getY();

                double neighborhoodRadiusSquared = param.getNeighborhoodRadius()
                        * param.getNeighborhoodRadius();

                neighbors.Clear();

                // Detecting neighbors using sorted lists

                double minX = tempX - param.getNeighborhoodRadius();
                double maxX = tempX + param.getNeighborhoodRadius();
                double minY = tempY - param.getNeighborhoodRadius();
                double maxY = tempY + param.getNeighborhoodRadius();
                int minRankInXOrder = currentInd.getRankInXOrder();
                int maxRankInXOrder = currentInd.getRankInXOrder();
                int minRankInYOrder = currentInd.getRankInYOrder();
                int maxRankInYOrder = currentInd.getRankInYOrder();

                for (int j = currentInd.getRankInXOrder() - 1; j >= 0; j--)
                {
                    if (swarmInXOrder[j].getX() >= minX)
                        minRankInXOrder = j;
                    else
                        break;
                }
                for (int j = currentInd.getRankInXOrder() + 1; j < numberOfSwarm; j++)
                {
                    if (swarmInXOrder[j].getX() <= maxX)
                        maxRankInXOrder = j;
                    else
                        break;
                }
                for (int j = currentInd.getRankInYOrder() - 1; j >= 0; j--)
                {
                    if (swarmInYOrder[j].getY() >= minY)
                        minRankInYOrder = j;
                    else
                        break;
                }
                for (int j = currentInd.getRankInYOrder() + 1; j < numberOfSwarm; j++)
                {
                    if (swarmInYOrder[j].getY() <= maxY)
                        maxRankInYOrder = j;
                    else
                        break;
                }

                if (maxRankInXOrder - minRankInXOrder < maxRankInYOrder
                        - minRankInYOrder)
                {
                    for (int j = minRankInXOrder; j <= maxRankInXOrder; j++)
                    {
                        Individual tempSwarm2 = swarmInXOrder[j];
                        if (currentInd != tempSwarm2)
                            if (tempSwarm2.getRankInYOrder() >= minRankInYOrder
                                    && tempSwarm2.getRankInYOrder() <= maxRankInYOrder)
                            {
                                if ((tempSwarm2.getX() - currentInd.getX())
                                        * (tempSwarm2.getX() - currentInd.getX())
                                        + (tempSwarm2.getY() - currentInd.getY())
                                        * (tempSwarm2.getY() - currentInd.getY()) < neighborhoodRadiusSquared)
                                    neighbors.Add(tempSwarm2);
                            }
                    }
                }
                else
                {
                    for (int j = minRankInYOrder; j <= maxRankInYOrder; j++)
                    {
                        Individual tempSwarm2 = swarmInYOrder[j];
                        if (currentInd != tempSwarm2)
                            if (tempSwarm2.getRankInXOrder() >= minRankInXOrder
                                    && tempSwarm2.getRankInXOrder() <= maxRankInXOrder)
                            {
                                if ((tempSwarm2.getX() - currentInd.getX())
                                        * (tempSwarm2.getX() - currentInd.getX())
                                        + (tempSwarm2.getY() - currentInd.getY())
                                        * (tempSwarm2.getY() - currentInd.getY()) < neighborhoodRadiusSquared)
                                    neighbors.Add(tempSwarm2);
                            }
                    }
                }

                for (int k = 0; k < temporaryIndividuals.Count; k++)
                {
                    if ((temporaryIndividuals[k].getX() - currentInd.getX())
                        * (temporaryIndividuals[k].getX() - currentInd.getX())
                        + (temporaryIndividuals[k].getY() - currentInd.getY())
                        * (temporaryIndividuals[k].getY() - currentInd.getY()) < neighborhoodRadiusSquared)
                        for (int j = 0; j < weightOfTemporaries; j++)
                            neighbors.Add(temporaryIndividuals[k]);
                }

                // simulating the behavior of swarm agents

                int n = neighbors.Count();

                double tempAx, tempAy;
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
                        Individual tempSwarm2 = neighbors[j];
                        localCenterX += tempSwarm2.getX();
                        localCenterY += tempSwarm2.getY();
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
                        Individual tempSwarm2 = neighbors[j];
                        double tempX2 = tempSwarm2.getX();
                        double tempY2 = tempSwarm2.getY();
                        double d = (tempX - tempX2) * (tempX - tempX2) + (tempY - tempY2)
                                * (tempY - tempY2);
                        if (d == 0)
                            d = 0.001;
                        tempAx += (tempX - tempX2) / d * param.getC3();
                        tempAy += (tempY - tempY2) / d * param.getC3();
                    }

                    rand = new Random();
                    if (rand.NextDouble() < param.getC4())
                    {
                        tempAx += rand.NextDouble() * 10 - 5;
                        tempAy += rand.NextDouble() * 10 - 5;
                    }
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

            }

            updateInternalState();
        }

        private void updateInternalState()
        {
            foreach (Individual ind in swarmInBirthOrder)
            {
                ind.stepSimulation();
            }

            sortInternalLists();

            resetRanks();
        }

        /*private void sortInternalLists() {
            IComparable myCollection;

            Collections.sort(swarmInXOrder, new Comparator<Individual>() {
			   
                public int compare(Individual o1, Individual o2) {
                    return Double.compare(o1.getX(), o2.getX());
                }		
            });
            Collections.sort(swarmInYOrder, new Comparator<Individual>() {
			   
                public int compare(Individual o1, Individual o2) {
                    return Double.compare(o1.getY(), o2.getY());
                }		
            });
        }*/

        /*private static Individual compareIndividual(Individual o1, Individual o2)
        {
            //TODO : put real sort logic in here
            return o1;
        }

        private void sortInternalLists()
        {
            swarmInXOrder.Sort();
        }*/

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
            swarmInXOrder.Sort((x, y) => x.getX().CompareTo(y.getX()));
            swarmInYOrder.Sort((x, y) => x.getY().CompareTo(y.getY()));
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

        public List<Individual> getSwarmInXOrder()
        {
            sortInternalLists();
            //return Collections.unmodifiableList(swarmInXOrder);
            //TODO : Fix commented out lines
            return swarmInXOrder;
        }

        public List<Individual> getSwarmInYOrder()
        {
            sortInternalLists();
            //return Collections.unmodifiableList(swarmInYOrder);
            //TODO : Fix commented out lines
            return swarmInYOrder;
        }

        public List<Individual> getSwarmInBirthOrder()
        {
            sortInternalLists();
            //return Collections.unmodifiableList(swarmInYOrder);
            //TODO : Fix commented out lines
            return swarmInBirthOrder;
        }



        public Population getPopulation()
        {
            return population;
        }

        public List<Individual> getSwarmSortedBySpecies()
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
            List<string> discoverdRecipies = population.Select(s => s.Select(d => d.getGenome().getRecipe()).Distinct().ToList<string>()).First();

            if (discoverdRecipies != null && discoverdRecipies.Count() > 0)
            {
                List<Species> species = new List<Species>();
                foreach (var rcpe in discoverdRecipies)
                {
                    species.Add(new Species(population.Select(s => s.Where(t => t.getGenome().getRecipe().ToString() == rcpe.ToString()).ToList()).First()));
                }

                population.Clear();
                population.AddRange(species);
            }
            
            
        }
    }
}

