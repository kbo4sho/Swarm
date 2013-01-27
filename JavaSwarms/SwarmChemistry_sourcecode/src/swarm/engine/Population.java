package swarm.engine;

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

import java.util.*;

public class Population extends AbstractList<Individual> {
	private static final double maxStartingVelocity = 5;
	private ArrayList<Individual> population;
	private String title;

	public Population(Collection<Individual> pop, String t) {
		population = new ArrayList<Individual>(pop);
		title = t;
	}

	public Population(int n, int width, int height, String t) {
		title = t;

		Parameters ancestorGenome = new Parameters();

		population = new ArrayList<Individual>();
		for (int i = 0; i < n; i++)
			population.add(new Individual(Math.random() * width, Math
					.random()
					* height, randomVelocity(), randomVelocity(),
					new Parameters(ancestorGenome)));
	}

	public Population(Population a, int width, int height, String t) {
		title = t;

		population = new ArrayList<Individual>();
		for (Individual temp: a) {
			population.add(new Individual(Math.random() * width, Math
					.random()
					* height, randomVelocity(), randomVelocity(),
					new Parameters(temp.getGenome())));
		}
	}

	public Population(Population a, Population b, double rate,
			int width, int height, String t) {
		title = t;

		population = new ArrayList<Individual>();
		for (int i = 0; i < (a.size() + b.size()) / 2; i++) {
			Population source;
			if (Math.random() < rate)
				source = a;
			else
				source = b;
			Individual temp = source.get((int) (Math.random() * source.size()));
			population.add(new Individual(Math.random() * width, Math
					.random()
					* height, randomVelocity(), randomVelocity(),
					new Parameters(temp.getGenome())));
		}
	}

	private static double randomVelocity() {
		return Math.random() * (maxStartingVelocity * 2) - maxStartingVelocity;
	}

	public void perturb(double pcm, int spaceSize) {
		int pop = population.size();
		pop += (int) ((Math.random() * 2.0 - 1.0) * pcm * (double) pop);
		if (pop < 1)
			pop = 1;
		if (pop > Parameters.numberOfIndividualsMax)
			pop = Parameters.numberOfIndividualsMax;

		ArrayList<Individual> newPopulation = new ArrayList<Individual>();
		Parameters tempParam;
		for (int i = 0; i < pop; i++) {
			tempParam = new Parameters(population
					.get((int) (Math.random() * population.size())).getGenome());
			newPopulation.add(new Individual(Math.random() * spaceSize,
					Math.random() * spaceSize, randomVelocity(), randomVelocity(), tempParam));
		}
		population = newPopulation;
	}

	@Override
	public Iterator<Individual> iterator() {
		return Collections.unmodifiableCollection(population).iterator();
	}

	@Override
	public int size() {
		return population.size();
	}

	@Override
	public Individual get(int index) {
		return population.get(index);
	}

	public String getTitle() {
		return title;
	}

	public void setTitle(String title) {
		this.title = title;
	}
}