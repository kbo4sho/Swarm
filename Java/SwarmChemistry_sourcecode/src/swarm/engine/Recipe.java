package swarm.engine;

// Recipe.java
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

import java.util.ArrayList;
import java.util.List;
import java.util.StringTokenizer;


public class Recipe {

	private ArrayList<Parameters> parameters;
	private ArrayList<Integer> popCounts;
	private String recipeText;

	private static final double populationChangeMagnitude = 0.8;
	private static final double duplicationOrDeletionRatePerParameterSets = 0.1;
	private static final double randomAdditionRatePerRecipe = 0.1;
	private static final double pointMutationRatePerParameter = 0.1;
	private static final double pointMutationMagnitude = 0.5;

	public Recipe(String text) {
		setFromText(text);
	}

	public Recipe(List<Individual> sol) {
		setFromPopulation(sol);
	}

	public boolean setFromText(String text) {
		char ch;
		int numberOfIngredients, numberOfIndividuals;
		double neighborhoodRadius, normalSpeed, maxSpeed, c1, c2, c3, c4, c5;

		StringBuffer recipeProcessed = new StringBuffer(text.length());
		for (int i = 0; i < text.length(); i++) {
			ch = text.charAt(i);
			if ((ch >= '0' && ch <= '9') || (ch == '.'))
				recipeProcessed.append(ch);
			else if (recipeProcessed.length() > 0) {
				if (recipeProcessed.charAt(recipeProcessed.length() - 1) != ' ')
					recipeProcessed.append(' ');
			}
		}

		StringTokenizer st = new StringTokenizer(recipeProcessed.toString(),
				" ");

		if (st.countTokens() % 9 != 0) {
			recipeText = "*** Formatting error ***\n" + text;
			parameters = null;
			popCounts = null;
			return false;
		}

		numberOfIngredients = st.countTokens() / 9;
		if (numberOfIngredients == 0) {
			recipeText = "*** No ingredients ***\n" + text;
			parameters = null;
			popCounts = null;
			return false;
		}
		if (numberOfIngredients > Parameters.numberOfIndividualsMax)
			numberOfIngredients = Parameters.numberOfIndividualsMax;

		parameters = new ArrayList<Parameters>();
		popCounts = new ArrayList<Integer>();

		try {
			for (int i = 0; i < numberOfIngredients; i++) {
				numberOfIndividuals = Integer.parseInt(st.nextToken());
				if (numberOfIndividuals < 1)
					numberOfIndividuals = 1;
				st.nextToken();
				st.nextToken();
				st.nextToken();
				st.nextToken();
				st.nextToken();
				st.nextToken();
				st.nextToken();
				st.nextToken();
			}

			st = new StringTokenizer(recipeProcessed.toString(), " ");

			for (int i = 0; i < numberOfIngredients; i++) {
				numberOfIndividuals = Integer.parseInt(st.nextToken());
				if (numberOfIndividuals < 1)
					numberOfIndividuals = 1;
				neighborhoodRadius = Double.parseDouble(st.nextToken());
				normalSpeed = Double.parseDouble(st.nextToken());
				maxSpeed = Double.parseDouble(st.nextToken());
				c1 = Double.parseDouble(st.nextToken());
				c2 = Double.parseDouble(st.nextToken());
				c3 = Double.parseDouble(st.nextToken());
				c4 = Double.parseDouble(st.nextToken());
				c5 = Double.parseDouble(st.nextToken());
				parameters.add(new Parameters(neighborhoodRadius,
						normalSpeed, maxSpeed, c1, c2, c3, c4, c5));
				popCounts.add(new Integer(numberOfIndividuals));
			}
		} catch (NumberFormatException nfe) {
			recipeText = "*** Formatting error ***\n" + text;
			parameters = null;
			popCounts = null;
			return false;
		}

		boundPopulationSize();
		setFromPopulation(createPopulation(300, 300));
		return true;
	}

	public void boundPopulationSize() {
		int totalPopulation = 0;
		double rescalingRatio;

		int numberOfIngredients = parameters.size();

		for (int i = 0; i < numberOfIngredients; i++)
			totalPopulation += popCounts.get(i).intValue();

		if (totalPopulation > Parameters.numberOfIndividualsMax)
			rescalingRatio = (double) (Parameters.numberOfIndividualsMax - numberOfIngredients)
					/ (totalPopulation == numberOfIngredients ? 1.0
							: (double) (totalPopulation - numberOfIngredients));

		else
			rescalingRatio = 1;

		if (rescalingRatio != 1)
			for (int i = 0; i < numberOfIngredients; i++)
				popCounts.set(i, new Integer(1 + (int) Math
						.floor((double) (popCounts.get(i).intValue() - 1)
								* rescalingRatio)));
	}

	public void setFromPopulation(List<Individual> sol) {
		parameters = new ArrayList<Parameters>();
		popCounts = new ArrayList<Integer>();

		Parameters tempParam;

		for (int i = 0; i < sol.size(); i++) {
			tempParam = sol.get(i).getGenome();

			boolean alreadyInParameters = false;
			for (int j = 0; j < parameters.size(); j++) {
				if (parameters.get(j).equals(tempParam)) {
					alreadyInParameters = true;
					popCounts.set(j, new Integer(
							popCounts.get(j).intValue() + 1));
				}
			}
			if (alreadyInParameters == false) {
				parameters.add(tempParam);
				popCounts.add(new Integer(1));
			}
		}

		setRecipeText();
	}

	private void setRecipeText() {
		Parameters tempParam;

		recipeText = "";

		for (int i = 0; i < parameters.size(); i++) {
			tempParam = parameters.get(i);
			recipeText += "" + popCounts.get(i).intValue() + " * ("
					+ shorten(tempParam.getNeighborhoodRadius()) + ", "
					+ shorten(tempParam.getNormalSpeed()) + ", "
					+ shorten(tempParam.getMaxSpeed()) + ", "
					+ shorten(tempParam.getC1()) + ", " + shorten(tempParam.getC2())
					+ ", " + shorten(tempParam.getC3()) + ", "
					+ shorten(tempParam.getC4()) + ", " + shorten(tempParam.getC5())
					+ ")\n";
		}
	}

	private double shorten(double d) {
		return Math.round(d * 100.0) / 100.0;
	}

	public ArrayList<Individual> createPopulation(int width, int height) {
		if (parameters == null)
			return null;

		ArrayList<Individual> newPopulation = new ArrayList<Individual>();
		Parameters tempParam;

		for (int i = 0; i < parameters.size(); i++) {
			tempParam = parameters.get(i);
			for (int j = 0; j < popCounts.get(i).intValue(); j++)
				newPopulation.add(new Individual(Math.random() * width,
						Math.random() * height, Math.random() * 10 - 5, Math
								.random() * 10 - 5, new Parameters(
								tempParam)));
		}

		return newPopulation;
	}
	 
	public boolean mutate() {
		boolean mutated = false;
		int numberOfIngredients = parameters.size();

		// Insertions, duplications and deletions

		for (int j = 0; j < numberOfIngredients; j++) {
			if (Math.random() < duplicationOrDeletionRatePerParameterSets) {
				if (Math.random() < .5) { // Duplication
					mutated = true;
					parameters.add(j + 1, parameters
							.get(j));
					popCounts.add(j + 1, popCounts
							.get(j));
					numberOfIngredients++;
					j++;
				} else { // Deletion
					if (numberOfIngredients > 1) {
						mutated = true;
						parameters.remove(j);
						popCounts.remove(j);
						numberOfIngredients--;
						j--;
					}
				}
			}
		}

		if (Math.random() < randomAdditionRatePerRecipe) { // Addition
			mutated = true;
			parameters.add(new Parameters());
			popCounts.add(new Integer((int) (Math.random()
					* Parameters.numberOfIndividualsMax * 0.5) + 1));
		}

		// Then Point Mutations

		Parameters tempParam;

		for (int j = 0; j < numberOfIngredients; j++) {
			tempParam = new Parameters(parameters.get(j));
			tempParam.inducePointMutations(pointMutationRatePerParameter,
					pointMutationMagnitude);
			if (!parameters.get(j).equals(tempParam)) {
				mutated = true;
				parameters.set(j, tempParam);
			}
		}

		boundPopulationSize();
		
		return mutated;
	}
	
	public String getRecipeText() {
		setRecipeText();
		return recipeText;
	}
}
