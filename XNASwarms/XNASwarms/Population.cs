using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
//   http://bingweb.binghamton.edu/~sayama/SwarmChemistry/
//

import java.util.*;*/

namespace XNASwarms
{
    public class Population:List<Individual> { //was extends AbstractList<Individual>
	    private static double maxStartingVelocity = 5;
                //Was ArrayList<Individual>
	    private List<Individual> population;
	    private String title;
        Random rand = new Random();

	    public Population(List<Individual> pop, String t) { //was Collection<Individual>
		    this.AddRange(pop);
		    title = t;
	    }

	    public Population(int n, int width, int height, String t) {
		    title = t;

		    Parameters ancestorGenome = new Parameters();

		    population = new List<Individual>();
            for (int i = 0; i < n; i++)
            {
                Individual ind = new Individual(rand.NextDouble() * width, rand.NextDouble()
                        * height, randomVelocity(), randomVelocity(),
                        new Parameters(ancestorGenome));
                
            }
	    }

	    public Population(Population a, int width, int height, String t) {
		    title = t;

		    population = new List<Individual>();
            foreach (Individual temp in a.population) //was  for(Individual temp: a)
            {
                population.Add(new Individual(rand.NextDouble() * width, rand.NextDouble()
                        * height, randomVelocity(), randomVelocity(),
                        new Parameters(temp.getGenome())));
            }
	    }

	    public Population(Population a, Population b, double rate,
			    int width, int height, String t) {
		    title = t;

		    population = new List<Individual>();
		    for (int i = 0; i < (a.size() + b.size()) / 2; i++) {
			    Population source;
			    if (rand.NextDouble() < rate)
				    source = a;
			    else
				    source = b;
			    Individual temp = source.get((int) (rand.NextDouble() * source.size()));
			    population.Add(new Individual(rand.NextDouble() * width, rand.NextDouble()
					    * height, randomVelocity(), randomVelocity(),
					    new Parameters(temp.getGenome())));
		    }
	    }

	    public static double randomVelocity() {
            Random rand = new Random();

		    return rand.NextDouble() * (maxStartingVelocity * 2) - maxStartingVelocity;
	    }

	    public void perturb(double pcm, int spaceSize) {
            Random rand = new Random();
		    int pop = population.Count();
		    pop += (int) ((rand.NextDouble() * 2.0 - 1.0) * pcm * (double) pop);
		    if (pop < 1)
			    pop = 1;
		    if (pop > Parameters.numberOfIndividualsMax)
			    pop = Parameters.numberOfIndividualsMax;

		    List<Individual> newPopulation = new List<Individual>();
		    Parameters tempParam;
		    for (int i = 0; i < pop; i++) {
			    tempParam = new Parameters(population
					    [(int) (rand.NextDouble() * population.Count())].getGenome());
			    newPopulation.Add(new Individual(rand.NextDouble() * spaceSize,
					    rand.NextDouble() * spaceSize, randomVelocity(), randomVelocity(), tempParam));
		    }
		    population = newPopulation;
	    }

	    
        //public Iterator<Individual> iterator() {
        //    return Collections.unmodifiableCollection(population).iterator();
        //} ************************* What is this?
        //SC? : I have a feeling we don't need this anymore. It seems like it is used for some kind of for loop.

	  
	    public int size() {
		    return this.Count(); //was population.size()
	    }

	   
	    public Individual get(int index) {
		    return this[index];
	    }

	    public String getTitle() {
		    return title;
	    }

	    public void setTitle(String title) {
		    this.title = title;
	    }
    }
}
