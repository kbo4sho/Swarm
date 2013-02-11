using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

/*package swarm.engine;

// SwarmPopulation.java
//
// Part of:
// SwarmChemistry.java
// Public Release Version 1.2.0
//
// 2006-2009 (c) Copyright by Hiroki Sayama. All rights reserved.
//
// Send any correspondences to:
//   Hiroki Sayama, D.Sc.
//   Assistant Professor, Department of Bioengineering
//   Binghamton University, State University of New York
//   P.O. Box 6000, Binghamton, NY 13902-6000, USA
//   Tel: +1-607-777-4439  Fax: +1-607-777-5780
//   Email: sayama@binghamton.edu
//
// For more information about this software, see:
// http://bingweb.binghamton.edu/~sayama/SwarmChemistry/
//

import java.util.*;*/

namespace SwarmEngine
{
    public class Population : List<Species>
    {
        private static double maxStartingVelocity = 5;
        private String title;
        Random rand = new Random();


        public Population():this(new List<Species>(),"kbothing")
        {
        }

        public Population(List<Species> species, String t)
        { 
            this.AddRange(species);
            title = t;
        }

        //public Population(int n, int width, int height, String t)
        //{
        //    title = t;

        //    Parameters ancestorGenome = new Parameters();

        //    speciesPopulation = new List<Species>();
        //    for (int i = 0; i < n; i++)
        //    {
        //        Individual ind = new Individual(rand.NextDouble() * width, rand.NextDouble()
        //                * height, randomVelocity(), randomVelocity(),
        //                new Parameters(ancestorGenome));

        //    }
        //}

        //public Population(Population a, int width, int height, String t)
        //{
        //    title = t;

        //    population = new List<Species>();
        //    foreach (Species temp in a.population) //was  for(Individual temp: a)
        //    {
        //        foreach (Individual ind in temp)
        //        {
        //            population.Add(new Individual(rand.NextDouble() * width, rand.NextDouble()
        //                    * height, randomVelocity(), randomVelocity(),
        //                    new Parameters(temp.getGenome())));
        //        }
        //    }
        //}

        //public Population(Population a, Population b, double rate,
        //        int width, int height, String t)
        //{
        //    title = t;

        //    population = new List<Species>();
        //    for (int i = 0; i < (a.size() + b.size()) / 2; i++)
        //    {
        //        Population source;
        //        if (rand.NextDouble() < rate)
        //            source = a;
        //        else
        //            source = b;
        //        Species temp = source.get((int)(rand.NextDouble() * source.size()));
        //        population.Add(new Individual(rand.NextDouble() * width, rand.NextDouble()
        //                * height, randomVelocity(), randomVelocity(),
        //                new Parameters(temp.getGenome())));
        //    }
        //}

        private double randomVelocity()
        {
            return rand.NextDouble() * (maxStartingVelocity * 2) - maxStartingVelocity;
        }

        //public void perturb(double pcm, int spaceSize)
        //{
        //    int speciesCount = population.Count();
        //    speciesCount += (int)((rand.NextDouble() * 2.0 - 1.0) * pcm * (double)speciesCount);
           
        //    if (speciesCount < 1)
        //    {
        //        speciesCount = 1;
        //    }
        //    if (speciesCount > Parameters.numberOfIndividualsMax)
        //    {
        //        speciesCount = Parameters.numberOfIndividualsMax;
        //    }

        //    List<Species> newPopulation = new List<Species>();
        //    Parameters tempParam;
        //    for (int i = 0; i < speciesCount; i++)
        //    {
        //        tempParam = new Parameters(population
        //                [(int)(rand.NextDouble() * population.Count())][(int)(rand.NextDouble() * population.Count())].getGenome());

        //        foreach(Species spcs in population)
        //        {
        //            Species newSpecies = new Species(rand.NextDouble() * spaceSize, rand.NextDouble() * spaceSize, randomVelocity(), randomVelocity(), tempParam);
        //            foreach(Individual in spc
        //            newPopulation.Add();
        //        }
                
        //    }
        //    population = newPopulation;
        //}

    
        //public Iterator<Individual> iterator() {
        //    return Collections.unmodifiableCollection(population).iterator();
        //} ************************* What is this?
        //SC? : I have a feeling we don't need this anymore. It seems like it is used for some kind of for loop.


        public int size()
        {
            return this.Count(); //was population.size()
        }


        public Species get(int index)
        {
            return this[index];
        }

        public String getTitle()
        {
            return title;
        }

        public void setTitle(String title)
        {
            this.title = title;
        }

    }
}
