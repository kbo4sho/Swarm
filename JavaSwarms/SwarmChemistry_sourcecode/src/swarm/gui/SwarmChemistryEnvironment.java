package swarm.gui;

// SwarmChemistryEnvironment.java
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

import java.awt.*;
import java.util.*;
import java.awt.event.*;
import javax.swing.*;

import swarm.engine.Parameters;
import swarm.engine.Population;
import swarm.engine.PopulationSimulator;
import swarm.engine.Recipe;

public class SwarmChemistryEnvironment implements ActionListener {
	JFrame controlFrame;
	Dimension screenDimension, controlFrameDimension;
	Insets screenInsets, controlFrameInsets;
	int totalScreenArea;
	int targetFrameSize;
	int initialSpaceSize = 300;
	JCheckBox tracking, pausing, mouseEffect;
	JButton undoing, randomAdding, showing, quitting;
	int numberOfFrames = 0;
	int howManySelected;

	int uniqueFrameID = 0;

	boolean applicationRunning;
	boolean thisIsApplet;

	boolean givenWithSpecificRecipe;
	String givenRecipeText;

	boolean randomAdditionRequest, mutationRequest, replicationRequest;

	ArrayList<Population> populations;
	ArrayList<SwarmPopulationSimulatorWindow> frames;
	SwarmPopulationSimulatorWindow[] selectedFrame = new SwarmPopulationSimulatorWindow[2];
	SwarmPopulationSimulatorWindow frameToMutate, frameToReplicate;
	java.util.List<RecipeFrame> recipeFrames;

	long previousTime, currentTime;
	long milliSecPerFrame = 70;

	public SwarmChemistryEnvironment(boolean app, String recipeText) {
		this(app, 1, true, recipeText);
	}

	public SwarmChemistryEnvironment(boolean app, int num) {
		this(app, num, false, "");
	}

	public SwarmChemistryEnvironment(boolean app, int num, boolean given,
			String recipeText) {

		thisIsApplet = app;
		recipeFrames = Collections
				.synchronizedList(new ArrayList<RecipeFrame>());

		numberOfFrames = 0;
		givenWithSpecificRecipe = given;
		givenRecipeText = recipeText;

		constructControlFrame();
		initializePopulations(num);

		applicationRunning = true;
		howManySelected = 0;
		randomAdditionRequest = mutationRequest = replicationRequest = false;
		previousTime = System.currentTimeMillis();

		while (applicationRunning) {
			int tfs = targetFrameSize;
			simulatePopulations();
			generateNewFrames();
			// simulateMotionOfFrames();
		}
	}

	public void simulatePopulations() {
		SwarmPopulationSimulatorWindow tempFrame;

		currentTime = System.currentTimeMillis();
		if (currentTime - previousTime > milliSecPerFrame)
			previousTime = currentTime;

		for (int i = 0; i < numberOfFrames; i++) {
			tempFrame = frames.get(i);

			if (tempFrame.isDisplayable()) {
				if (currentTime == previousTime) {
					if (!pausing.isSelected()) {
						tempFrame.simulateSwarmBehavior();
					}
					tempFrame.displayStates();
				}
				if (tempFrame.isToMutate) {
					tempFrame.isToMutate = false;
					frameToMutate = tempFrame;
					mutationRequest = true;
				}
				if (tempFrame.isToReplicate) {
					tempFrame.isToReplicate = false;
					frameToReplicate = tempFrame;
					replicationRequest = true;
				}
				if (tempFrame.isSelected && tempFrame.notYetNoticed) {
					tempFrame.notYetNoticed = false;
					selectedFrame[howManySelected] = tempFrame;
					howManySelected++;
				} else if (!tempFrame.isSelected && !tempFrame.notYetNoticed) {
					tempFrame.notYetNoticed = true;
					howManySelected--;
				}
			} else {
				if (tempFrame.isSelected && !tempFrame.notYetNoticed) {
					tempFrame.isSelected = false;
					tempFrame.notYetNoticed = true;
					howManySelected--;
				}
				numberOfFrames--;
				frames.remove(i);
				i--;
			}
		}
	}

	public void generateNewFrames() {
		Population tempPopulation;

		if (howManySelected == 2) {
			howManySelected = 0;
			selectedFrame[0].isSelected = false;
			selectedFrame[0].notYetNoticed = true;
			selectedFrame[1].isSelected = false;
			selectedFrame[1].notYetNoticed = true;

			if (selectedFrame[0] == selectedFrame[1]) {
				// Mutation
				mutationRequest = true;
				frameToMutate = selectedFrame[0];
			} else {
				// Mixing
				tempPopulation = new Population(
						selectedFrame[0].population,
						selectedFrame[1].population, Math.random() * 0.6 + 0.2,
						initialSpaceSize, initialSpaceSize, "Mixed");
				addFrame(tempPopulation,
						(selectedFrame[0].getLocation().x + selectedFrame[1]
								.getLocation().x) / 2, (selectedFrame[0]
								.getLocation().y + selectedFrame[1]
								.getLocation().y) / 2);
			}
		}

		if (mutationRequest) {
			// Mutation
			mutationRequest = false;
			//boolean mutated = false;

			// Obtain a recipe from population

			Recipe tempRecipe = new Recipe(frameToMutate.population);
			boolean mutated = tempRecipe.mutate();
			
			if (mutated)
				addFrame(new Population(tempRecipe.createPopulation(
						targetFrameSize, targetFrameSize), "Mutated"),
						frameToMutate.getLocation().x + 30, frameToMutate
								.getLocation().y + 30);
			else
				addFrame(new Population(tempRecipe.createPopulation(
						targetFrameSize, targetFrameSize), "Mutation failed"),
						frameToMutate.getLocation().x + 30, frameToMutate
								.getLocation().y + 30);
		}

		if (randomAdditionRequest) {
			randomAdditionRequest = false;
			tempPopulation = new Population(
					(int) (Math.random() * Parameters.numberOfIndividualsMax) + 1,
					initialSpaceSize, initialSpaceSize, "Randomly generated");
			addFrame(
					tempPopulation,
					(int) Math.round(Math.random()
							* (screenDimension.width - screenInsets.right
									- targetFrameSize - screenInsets.left)
							+ screenInsets.left),
					(int) Math
							.round(Math.random()
									* (screenDimension.height
											- screenInsets.bottom
											- targetFrameSize
											- screenInsets.top - controlFrameDimension.height)
									+ screenInsets.top
									+ controlFrameDimension.height));
		}

		if (replicationRequest) {
			replicationRequest = false;

			Recipe tempRecipe = new Recipe(
					frameToReplicate.population);
			addFrame(new Population(tempRecipe.createPopulation(
					targetFrameSize, targetFrameSize), "Replicated"),
					frameToReplicate.getLocation().x + 30, frameToReplicate
							.getLocation().y + 30);
		}
	}

	public int overlap(int x1, int y1, int w1, int h1, int x2, int y2, int w2,
			int h2) {

		int overlapX = Math.max(Math.min(x1 + w1 - 1, x2 + w2 - 1)
				- Math.max(x1, x2), 0);
		int overlapY = Math.max(Math.min(y1 + h1 - 1, y2 + h2 - 1)
				- Math.max(y1, y2), 0);

		return overlapX * overlapY;
	}

	public void simulateMotionOfFrames() {
		SwarmPopulationSimulatorWindow tempFrame, tempFrame2;
		Dimension simulatorDimension;

		if (numberOfFrames == 0)
			return;

		simulatorDimension = frames.get(0).getSize();

		int totalFrameArea = 0;

		int dx, dy, x1, y1, ox1, oy1, x2, y2, w1, h1, w2, h2, ov, cx1, cy1, cx2, cy2;
		Point tempPoint;
		Dimension tempDimension;

		for (int i = 0; i < numberOfFrames; i++) {
			tempFrame = frames.get(i);
			if (tempFrame.getExtendedState() == JFrame.NORMAL) {

				tempPoint = tempFrame.getLocation();
				x1 = ox1 = tempPoint.x;
				y1 = oy1 = tempPoint.y;
				tempDimension = tempFrame.getSize();
				w1 = tempDimension.width;
				h1 = tempDimension.height;
				totalFrameArea += w1 * h1;

				dx = dy = 0;

				for (int j = 0; j < numberOfFrames; j++) {
					if (i != j) {
						tempFrame2 = frames.get(j);
						if (tempFrame2.getExtendedState() == JFrame.NORMAL) {

							tempPoint = tempFrame2.getLocation();
							x2 = tempPoint.x;
							y2 = tempPoint.y;
							tempDimension = tempFrame2.getSize();
							w2 = tempDimension.width;
							h2 = tempDimension.height;

							ov = overlap(x1, y1, w1, h1, x2, y2, w2, h2);

							cx1 = x1 + w1 / 2;
							cy1 = y1 + h1 / 2;
							cx2 = x2 + w2 / 2;
							cy2 = y2 + h2 / 2;

							if (cx1 == cx2 && cy1 == cy2) {
								cx2++;
								cy2++;
							}

							dx += (cx1 - cx2) * ov / (w1 * h1 * 2);
							dy += (cy1 - cy2) * ov / (w1 * h1 * 2);

						}
					}
				}

				x1 += dx;
				if (x1 < screenInsets.left)
					x1 = screenInsets.left;
				if (x1 > screenDimension.width - screenInsets.right - w1)
					x1 = screenDimension.width - screenInsets.right - w1;

				y1 += dy;
				if (y1 < screenInsets.top + controlFrameDimension.height)
					y1 = screenInsets.top + controlFrameDimension.height;
				if (y1 > screenDimension.height - screenInsets.bottom - h1)
					y1 = screenDimension.height - screenInsets.bottom - h1;

				if ((dx != 0 || dy != 0) && x1 == ox1 && y1 == oy1) {
					x1 += Math.random() > 0.5 ? -1 : 1;
					y1 += Math.random() > 0.5 ? -1 : 1;
					if (x1 < screenInsets.left)
						x1 = screenInsets.left;
					if (x1 > screenDimension.width - screenInsets.right - w1)
						x1 = screenDimension.width - screenInsets.right - w1;
					if (y1 < screenInsets.top + controlFrameDimension.height)
						y1 = screenInsets.top + controlFrameDimension.height;
					if (y1 > screenDimension.height - screenInsets.bottom - h1)
						y1 = screenDimension.height - screenInsets.bottom - h1;
				}

				tempFrame.setLocation(x1, y1);
			}
		}

		if ((double) totalFrameArea > 0.6 * (double) totalScreenArea) {
			targetFrameSize *= 0.95;
			for (int i = 0; i < numberOfFrames; i++)
				frames.get(i).rescale(targetFrameSize);
		} else if ((double) totalFrameArea < 0.4 * (double) totalScreenArea) {
			targetFrameSize *= 1.05;
			if (targetFrameSize > screenDimension.height)
				targetFrameSize = screenDimension.height;
			for (int i = 0; i < numberOfFrames; i++)
				frames.get(i).rescale(targetFrameSize);
		}
	}

	public void closeApplet() {
		for (int i = 0; i < numberOfFrames; i++) {
			frames.get(i).dispose();
			frames.set(i, null);
		}
		frames.clear();
		for (int i = 0; i < recipeFrames.size(); i++) {
			recipeFrames.get(i).dispose();
			recipeFrames.set(i, null);
		}
		recipeFrames.clear();
		controlFrame.dispose();
		applicationRunning = false;
		System.gc();
	}

	public void constructControlFrame() {
		controlFrame = new JFrame("Swarm Chemistry Simulator");
		controlFrame
				.setDefaultCloseOperation(WindowConstants.DO_NOTHING_ON_CLOSE);
		controlFrame.setVisible(true);
		controlFrame.setBackground(Color.white);
		controlFrame.addWindowListener(new WindowAdapter() {
			public void windowClosing(WindowEvent e) {
				if (thisIsApplet)
					closeApplet();
				else
					System.exit(0);
			}
		});

		Font font = new Font("dialog", Font.BOLD, 16);
		UIManager.put("Button.font", font);

		controlFrame.setLayout(new FlowLayout());
		controlFrame.getContentPane().add(
				tracking = new JCheckBox("Automatic zoom", false));
		controlFrame.getContentPane().add(
				pausing = new JCheckBox("Pause", false));
		controlFrame.getContentPane().add(
				mouseEffect = new JCheckBox("Interaction w. mouse cursor",
						false));
		controlFrame.getContentPane().add(
				undoing = new JButton("Undo selection"));
		undoing.addActionListener(this);
		controlFrame.getContentPane().add(
				randomAdding = new JButton("Add a random swarm"));
		randomAdding.addActionListener(this);
		controlFrame.getContentPane().add(
				showing = new JButton("Bring all windows to front"));
		showing.addActionListener(this);
		controlFrame.getContentPane().add(quitting = new JButton("Quit"));
		quitting.addActionListener(this);

		screenDimension = controlFrame.getToolkit().getScreenSize();
		targetFrameSize = screenDimension.width / 5;
		screenInsets = controlFrame.getToolkit().getScreenInsets(
				controlFrame.getGraphicsConfiguration());
		controlFrameInsets = controlFrame.getInsets();
		controlFrame.setSize(screenDimension.width, 40 + controlFrameInsets.top
				+ controlFrameInsets.bottom);
		controlFrame.setLocation(0, screenInsets.top);
		controlFrame.setVisible(true);
		controlFrameDimension = controlFrame.getSize();
		totalScreenArea = (screenDimension.width - screenInsets.left - screenInsets.right)
				* (screenDimension.height - screenInsets.top - screenInsets.bottom)
				- controlFrameDimension.width * controlFrameDimension.height;
	}

	public void addFrame(Population pop, int x, int y) {

		numberOfFrames++;

		populations.add(pop);

		SwarmPopulationSimulatorWindow tempFrame = new SwarmPopulationSimulatorWindow(
				targetFrameSize, initialSpaceSize, pop, populations,
				++uniqueFrameID, tracking, mouseEffect, recipeFrames);
		tempFrame.setLocation(x, y);
		frames.add(tempFrame);
	}

	public void initializePopulations(int num) {
		Population tempPopulation;

		populations = new ArrayList<Population>();
		frames = new ArrayList<SwarmPopulationSimulatorWindow>();

		for (int i = 0; i < num; i++) {

			if (givenWithSpecificRecipe) {
				givenWithSpecificRecipe = false;
				tempPopulation = new Population((new Recipe(
						givenRecipeText)).createPopulation(initialSpaceSize,
						initialSpaceSize), "Created from a given recipe");
			} else {
				tempPopulation = new Population(
						(int) (Math.random() * Parameters.numberOfIndividualsMax) + 1,
						initialSpaceSize, initialSpaceSize,
						"Randomly generated");
			}

			addFrame(
					tempPopulation,
					(int) Math.round(Math.random()
							* (screenDimension.width - screenInsets.right
									- targetFrameSize - screenInsets.left)
							+ screenInsets.left),
					(int) Math
							.round(Math.random()
									* (screenDimension.height
											- screenInsets.bottom
											- targetFrameSize
											- screenInsets.top - controlFrameDimension.height)
									+ screenInsets.top
									+ controlFrameDimension.height));
		}
	}

	public void actionPerformed(ActionEvent e) {
		Object src = e.getSource();

		if (src == undoing) {
			for (int i = 0; i < numberOfFrames; i++)
				frames.get(i).isSelected = false;
		}

		else if (src == randomAdding) {
			randomAdditionRequest = true;
		}

		else if (src == showing) {
			SwarmPopulationSimulatorWindow tempFrame;
			for (int i = 0; i < numberOfFrames; i++) {
				tempFrame = frames.get(i);
				if (tempFrame.isDisplayable()) {
					tempFrame.setState(JFrame.NORMAL);
					tempFrame.toFront();
				}
			}

			RecipeFrame rcf;
			for (int i = 0; i < recipeFrames.size(); i++) {
				rcf = recipeFrames.get(i);
				rcf.setState(JFrame.NORMAL);
				rcf.toFront();
			}
			controlFrame.toFront();
		}

		else if (src == quitting) {
			if (thisIsApplet)
				closeApplet();
			else
				System.exit(0);
		}
	}
}
