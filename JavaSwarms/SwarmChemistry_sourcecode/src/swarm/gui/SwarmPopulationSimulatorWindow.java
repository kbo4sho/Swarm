package swarm.gui;

// SwarmPopulationSimulator.java
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
import java.util.List;
import java.awt.event.*;
import javax.swing.*;

import swarm.engine.Individual;
import swarm.engine.Parameters;
import swarm.engine.Population;
import swarm.engine.PopulationSimulator;

public class SwarmPopulationSimulatorWindow extends JFrame implements ActionListener {
	PopulationSimulator simulator;
	public int width, height, originalWidth, originalHeight;
	private JPanel cp;
	private Insets ins;
	private int menuHeight;
	private Graphics img, sfg;

	private JCheckBox tracking, mouseEffect;

	private JMenuItem menuMutate, menuMix, menuReplicate, menuEdit, menuKill;
	private JMenuBar mb;

	private double currentMidX, currentMidY, currentScalingFactor;
	private double swarmRadius = 3;
	private double swarmDiameter;
	private int mouseX, mouseY;
	private int weightOfMouseCursor = 20;
	private boolean isMouseIn;

	public int frameNumber;
	public RecipeFrame displayedRecipe;
	public Population population;
	private ArrayList<Population> originalPopulationList;
	private java.util.List<RecipeFrame> recipeFrames;

	public Image im;
	public boolean isSelected, notYetNoticed, isToMutate, isToReplicate;

	public SwarmPopulationSimulatorWindow(int frameSize, int spaceSize,
			Population sol, ArrayList<Population> solList, int num,
			JCheckBox tr, JCheckBox mo, java.util.List<RecipeFrame> rcfs) {
		super("Swarm #" + num + ": " + sol.getTitle());
		simulator = new PopulationSimulator(sol);

		frameNumber = num;
		displayedRecipe = null;
		recipeFrames = rcfs;
		population = sol;
		originalPopulationList = solList;

		width = height = frameSize;
		originalWidth = originalHeight = spaceSize;
		tracking = tr;
		mouseEffect = mo;

		Font font = new Font("dialog", Font.BOLD, 16);
		UIManager.put("Menu.font", font);
		UIManager.put("MenuItem.font", font);

		menuMutate = new JMenuItem("Mutate");
		menuMutate.addActionListener(this);
		menuMix = new JMenuItem("Mix");
		menuMix.addActionListener(this);
		menuReplicate = new JMenuItem("Replicate");
		menuReplicate.addActionListener(this);
		menuEdit = new JMenuItem("Edit");
		menuEdit.addActionListener(this);
		menuKill = new JMenuItem("Kill");
		menuKill.addActionListener(this);

		mb = new JMenuBar();
		mb.add(menuReplicate);
		mb.add(menuMutate);
		mb.add(menuMix);
		mb.add(menuEdit);
		mb.add(menuKill);
		setJMenuBar(mb);

		cp = new JPanel(new BorderLayout());
		cp.setBackground(Color.white);
		setContentPane(cp);

		setDefaultCloseOperation(WindowConstants.DO_NOTHING_ON_CLOSE);

		setVisible(true);

		ins = getInsets();
		menuHeight = mb.getHeight();
		setSize(width + ins.left + ins.right, height + ins.top + ins.bottom
				+ menuHeight);

		while (sfg == null)
			sfg = getGraphics();

		synchronized (this) {
			while (im == null)
				im = createImage(width, height);
			while (img == null)
				img = im.getGraphics();
		}
		clearImage();

		addWindowListener(new WindowAdapter() {
			public void windowClosing(WindowEvent e) {
				if (displayedRecipe != null) {
					displayedRecipe.orphanize();
				}
				dispose();
			}
		});

		addComponentListener(new ComponentAdapter() {
			public void componentResized(ComponentEvent e) {
				ins = getInsets();
				menuHeight = mb.getHeight();
				width = getWidth() - ins.left - ins.right;
				height = getHeight() - ins.top - ins.bottom - menuHeight;
				synchronized (SwarmPopulationSimulatorWindow.this) {
					im = null;
					img = null;
					while (im == null)
						im = createImage(width, height);
					while (img == null)
						img = im.getGraphics();
				}
				redraw();
			}
		});

		addMouseListener(new MouseAdapter() {
			public void mouseClicked(MouseEvent me) {
				if (me.getModifiers() == InputEvent.BUTTON3_MASK) {
					outputRecipe();
				} else if (isSelected == false)
					isSelected = true;
				else
					notYetNoticed = true;
			}

			public void mouseEntered(MouseEvent me) {
				isMouseIn = true;
			}

			public void mouseExited(MouseEvent me) {
				isMouseIn = false;
			}
		});

		mouseX = mouseY = -100;

		addMouseMotionListener(new MouseMotionAdapter() {
			public void mouseDragged(MouseEvent me) {
				mouseX = me.getX() - ins.left;
				mouseY = me.getY() - ins.top - menuHeight;
			}

			public void mouseMoved(MouseEvent me) {
				mouseX = me.getX() - ins.left;
				mouseY = me.getY() - ins.top - menuHeight;
			}
		});

		isSelected = false;
		notYetNoticed = true;
		isToMutate = false;
		isToReplicate = false;

		currentMidX = 0;
		currentMidY = 0;
		currentScalingFactor = 0;
		swarmDiameter = swarmRadius * 2;

		displayStates();
	}

	public void paint(Graphics g) {
		synchronized (this) {
			while (im == null)
				im = createImage(width, height);
			while (img == null)
				img = im.getGraphics();
		}
		mb.repaint();
		g.drawImage(im, ins.left, ins.top + menuHeight, width, height, this);
	}

	public void redraw() {
		mb.repaint();
		sfg.drawImage(im, ins.left, ins.top + menuHeight, width, height, this);
	}

	public void clearImage() {
		img.setColor(Color.white);
		img.fillRect(0, 0, width, height);
		redraw();
	}
	
	public synchronized void replacePopulationWith(Population newpop) {
		setTitle("Swarm #" + frameNumber + ": " + newpop.getTitle());

		for (int i = 0; i < originalPopulationList.size(); i++) {
			if (originalPopulationList.get(i) == population)
				population = newpop;
			originalPopulationList.set(i, population);
		}

		currentMidX = 0;
		currentMidY = 0;
		currentScalingFactor = 0;
		swarmDiameter = swarmRadius * 2;

		simulator = new PopulationSimulator(population);

		displayStates();
	}

	public synchronized void displayStates() {
		Individual ag, ag2;
		int max, x, y;
		double minX, maxX, minY, maxY, tempX, tempY, midX, midY, scalingFactor;
		double averageInterval;
		double intervalCoefficient = 10.0;
		int tempRadius, tempDiameter;
		int margin = 30;
		double gridInterval = 300;

		while (img == null)
			;

		if (isSelected)
			img.setColor(Color.cyan);
		else
			img.setColor(Color.white);
		img.fillRect(0, 0, width, height);

		Population population = simulator.getPopulation();
		
		if ((max = population.size()) == 0) {
			redraw();
			return;
		}

		List<Individual> swarmInXOrder = simulator.getSwarmInXOrder();
		List<Individual> swarmInYOrder = simulator.getSwarmInYOrder();
		minX = swarmInXOrder.get(0).getX();
		maxX = swarmInXOrder.get(max - 1).getX();
		minY = swarmInYOrder.get(0).getY();
		maxY = swarmInYOrder.get(max - 1).getY();

		if (tracking.isSelected() && max > 10) {

			averageInterval = 0;
			for (int i = 0; i < max - 1; i++) {
				ag = swarmInXOrder.get(i);
				ag2 = swarmInXOrder.get(i + 1);
				averageInterval += ag2.getX() - ag.getX();
			}
			averageInterval /= max - 1;
			for (int i = 0; i < max - 10; i++) {
				ag = swarmInXOrder.get(i);
				ag2 = swarmInXOrder.get(i + 10);
				if (ag2.getX() - ag.getX() < averageInterval * intervalCoefficient) {
					minX = ag.getX();
					break;
				}
			}
			for (int i = max - 1; i >= 10; i--) {
				ag = swarmInXOrder.get(i - 10);
				ag2 = swarmInXOrder.get(i);
				if (ag2.getX() - ag.getX() < averageInterval * intervalCoefficient) {
					maxX = ag2.getX();
					break;
				}
			}

			tempX = (maxX - minX) * 0.1;
			minX -= tempX;
			maxX += tempX;

			averageInterval = 0;
			for (int i = 0; i < max - 1; i++) {
				ag = swarmInYOrder.get(i);
				ag2 = swarmInYOrder.get(i + 1);
				averageInterval += ag2.getY() - ag.getY();
			}
			averageInterval /= max - 1;
			for (int i = 0; i < max - 10; i++) {
				ag = swarmInYOrder.get(i);
				ag2 = swarmInYOrder.get(i + 10);
				if (ag2.getY() - ag.getY() < averageInterval * intervalCoefficient) {
					minY = ag.getY();
					break;
				}
			}
			for (int i = max - 1; i >= 10; i--) {
				ag = swarmInYOrder.get(i - 10);
				ag2 = swarmInYOrder.get(i);
				if (ag2.getY() - ag.getY() < averageInterval * intervalCoefficient) {
					maxY = ag2.getY();
					break;
				}
			}

			tempY = (maxY - minY) * 0.1;
			minY -= tempY;
			maxY += tempY;
		}

		if (maxX - minX < (double) originalWidth)
			maxX = (minX = (minX + maxX - (double) originalWidth) / 2)
					+ (double) originalWidth;
		if (maxY - minY < (double) originalHeight)
			maxY = (minY = (minY + maxY - (double) originalHeight) / 2)
					+ (double) originalHeight;

		midX = (minX + maxX) / 2;
		midY = (minY + maxY) / 2;

		if ((maxX - minX) * height > (maxY - minY) * width)
			scalingFactor = ((double) (width - 2 * margin)) / (maxX - minX);
		else
			scalingFactor = ((double) (height - 2 * margin)) / (maxY - minY);

		if (currentScalingFactor == 0) {
			currentMidX += midX;
			currentMidY += midY;
			currentScalingFactor = scalingFactor;
		} else {
			currentMidX += (midX - currentMidX) * 0.1;
			currentMidY += (midY - currentMidY) * 0.1;
			currentScalingFactor += (scalingFactor - currentScalingFactor) * 0.5;
		}

		// Drawing grids

		img.setColor(Color.lightGray);
		for (tempX = Math
				.floor((-((double) width) / 2 / currentScalingFactor + currentMidX)
						/ gridInterval)
				* gridInterval; tempX < ((double) width) / 2
				/ currentScalingFactor + currentMidX; tempX += gridInterval)
			img.drawLine((int) ((tempX - currentMidX) * currentScalingFactor)
					+ width / 2, 0,
					(int) ((tempX - currentMidX) * currentScalingFactor)
							+ width / 2, height);
		for (tempY = Math
				.floor((-((double) height) / 2 / currentScalingFactor + currentMidY)
						/ gridInterval)
				* gridInterval; tempY < ((double) height) / 2
				/ currentScalingFactor + currentMidY; tempY += gridInterval)
			img.drawLine(0,
					(int) ((tempY - currentMidY) * currentScalingFactor)
							+ height / 2, width,
					(int) ((tempY - currentMidY) * currentScalingFactor)
							+ height / 2);

		// Drawing swarm

		tempRadius = (int) (swarmRadius * currentScalingFactor);
		tempDiameter = (int) (swarmDiameter * currentScalingFactor);
		if (tempDiameter < 3)
			tempDiameter = 3;

		for (int i = 0; i < max; i++) {
			ag = population.get(i);
			x = (int) ((ag.getX() - currentMidX) * currentScalingFactor) + width / 2;
			y = (int) ((ag.getY() - currentMidY) * currentScalingFactor) + height
					/ 2;
			img.setColor(ag.getDisplayColor());
			img.fillOval(x - tempRadius, y - tempRadius, tempDiameter,
					tempDiameter);
		}

		redraw();

		/*
		// Relocating swarm if they went too far

		if (midX < -3 * gridInterval) {
			currentMidX += gridInterval;
			for (int i = 0; i < max; i++) {
				Individual ind = population.get(i);
				ind.getX() += gridInterval;
			}
		} else if (midX > 3 * gridInterval) {
			currentMidX -= gridInterval;
			for (int i = 0; i < max; i++)
				swarmInBirthOrder.get(i).getX() -= gridInterval;
		}

		if (midY < -3 * gridInterval) {
			currentMidY += gridInterval;
			for (int i = 0; i < max; i++)
				swarmInBirthOrder.get(i).getY() += gridInterval;
		} else if (midY > 3 * gridInterval) {
			currentMidY -= gridInterval;
			for (int i = 0; i < max; i++)
				swarmInBirthOrder.get(i).getY() -= gridInterval;
		}*/
	}

	public synchronized void outputRecipe() {
		if (displayedRecipe == null) {
			displayedRecipe = new RecipeFrame(this, simulator.getPopulation(),
					originalWidth, originalHeight, recipeFrames);
			synchronized (recipeFrames) {
				recipeFrames.add(displayedRecipe);
			}
			displayedRecipe.setVisible(true);
		} else {
			displayedRecipe.putImage(im);
			displayedRecipe.setState(JFrame.NORMAL);
			displayedRecipe.toFront();
		}
	}

	public void actionPerformed(ActionEvent e) {
		Object src = e.getSource();

		if (src == menuMutate) {
			isToMutate = true;
		}

		else if (src == menuMix) {
			if (isSelected == false)
				isSelected = true;
			else
				isSelected = false;
		}

		else if (src == menuReplicate) {
			isToReplicate = true;
		}

		else if (src == menuEdit) {
			outputRecipe();
		}

		else if (src == menuKill) {
			if (displayedRecipe != null) {
				displayedRecipe.orphanize();
			}
			dispose();
		}
	}

	public void rescale(int targetFrameSize) {
		int prevWidth = width;
		int prevHeight = height;

		int w = prevWidth + (targetFrameSize - prevWidth) / 2;
		if (w < 30)
			w = 30;
		int h = prevHeight + (targetFrameSize - prevHeight) / 2;
		if (h < 30)
			h = 30;

		setSize(w + ins.left + ins.right, h + ins.top + ins.bottom + menuHeight);
		ins = getInsets();
		menuHeight = mb.getHeight();
		width = getWidth() - ins.left - ins.right;
		height = getHeight() - ins.top - ins.bottom - menuHeight;

		int offsetx = (prevWidth - width) / 2;
		int offsety = (prevHeight - height) / 2;
		setLocation(getLocation().x + offsetx, getLocation().y + offsety);

		synchronized (this) {
			im = null;
			img = null;
			while (im == null)
				im = createImage(width, height);
			while (img == null)
				img = im.getGraphics();
		}
		redraw();
	}

	public void simulateSwarmBehavior() {
		Collection<Individual> mouseCursor = Collections.emptyList();

		if (mouseEffect.isSelected() && isMouseIn) {
			Individual ind = new Individual(((double) (mouseX - width / 2))
					/ currentScalingFactor + currentMidX, ((double) (mouseY - height / 2))
					/ currentScalingFactor + currentMidY, 0.0, 0.0, new Parameters());
			mouseCursor = Collections.singletonList(ind);
		}
		
		simulator.stepSimulation(mouseCursor, weightOfMouseCursor);
	}
}
