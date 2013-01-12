package swarm.gui;

// RecipeFrame.java
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

import java.awt.BorderLayout;
import java.awt.Canvas;
import java.awt.Color;
import java.awt.Graphics;
import java.awt.Image;
import java.awt.Insets;
import java.awt.Label;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.WindowAdapter;
import java.awt.event.WindowEvent;
import java.util.List;

import javax.swing.JButton;
import javax.swing.JFrame;
import javax.swing.JLabel;
import javax.swing.JPanel;
import javax.swing.JScrollPane;
import javax.swing.JTextArea;
import javax.swing.WindowConstants;

import swarm.engine.Individual;
import swarm.engine.Population;
import swarm.engine.Recipe;

public class RecipeFrame extends JFrame implements ActionListener {
	private Recipe recipe;
	private Image im;
	private Insets ins;
	private Canvas swarmCanvas;
	private JPanel leftPanel, rightPanel;
	private JTextArea recipeBox;
	private JButton applyEdits;
	private int width, height;
	private SwarmPopulationSimulatorWindow parentFrame;
	private java.util.List<RecipeFrame> recipeFrames;

	public RecipeFrame(SwarmPopulationSimulatorWindow par,
			List<Individual> swarmInAnyOrder, int w, int h,
			java.util.List<RecipeFrame> rcfs) {

		super("Recipe of Swarm #" + par.frameNumber);

		parentFrame = par;
		width = w;
		height = h;
		recipeFrames = rcfs;
		setDefaultCloseOperation(WindowConstants.DO_NOTHING_ON_CLOSE);
		setResizable(false);
		setVisible(true);
		setLocation(parentFrame.getLocation());
		ins = getInsets();
		setSize(600 + ins.left + ins.right, 240 + ins.top + ins.bottom);

		addWindowListener(new WindowAdapter() {
			public void windowClosing(WindowEvent e) {
				synchronized (recipeFrames) {
					recipeFrames.remove(parentFrame.displayedRecipe);
					parentFrame.displayedRecipe = null;
				}
				dispose();
			}
		});

		putImage(parentFrame.im);

		rightPanel = new JPanel();
		rightPanel.setLayout(new BorderLayout());
		rightPanel.add(new JLabel("Screen shot"), BorderLayout.NORTH);
		rightPanel.add(swarmCanvas = new Canvas() {
			public void paint(Graphics g) {
				g.drawImage(im, 0, 0, 200, 200, this);
			}
		}, BorderLayout.CENTER);
		swarmCanvas.setSize(200, 200);
		add(rightPanel, BorderLayout.EAST);

		leftPanel = new JPanel();
		leftPanel.setLayout(new BorderLayout());
		leftPanel.add(new Label(
				"Format: # of agents * (R, Vn, Vm, c1, c2, c3, c4, c5)"),
				BorderLayout.NORTH);
		recipeBox = new JTextArea();
		recipeBox.setBackground(Color.white);
		leftPanel.add(new JScrollPane(recipeBox), BorderLayout.CENTER);
		leftPanel.add(applyEdits = new JButton("Apply edits"),
				BorderLayout.SOUTH);
		add(leftPanel, BorderLayout.CENTER);

		applyEdits.addActionListener(this);

		recipe = new Recipe(swarmInAnyOrder);
		recipeBox.setText(recipe.getRecipeText());
	}

	public void putImage(Image im2) {
		while (im == null)
			im = createImage(200, 200);
		im.getGraphics().drawImage(im2, 0, 0, 200, 200, this);
		if (swarmCanvas instanceof Canvas)
			swarmCanvas.repaint();
	}

	public void actionPerformed(ActionEvent e) {
		if (e.getSource() == applyEdits) {

			recipe.setFromText(recipeBox.getText());
			recipeBox.setText(recipe.getRecipeText());

			if (recipe.getRecipeText().charAt(0) == '*')
				recipeBox.setBackground(Color.yellow);
			else {
				recipeBox.setBackground(Color.white);
				Population newSwarmPop = new Population(recipe
						.createPopulation(width, height),
						"Created from a given recipe");
				parentFrame.replacePopulationWith(newSwarmPop);
				im.getGraphics().clearRect(0, 0, 200, 200);
				swarmCanvas.repaint();
			}
		}
	}

	public void orphanize() {
		setTitle("Orphaned recipe");
		putImage(parentFrame.im);
		recipeBox.setEditable(false);
		applyEdits.setEnabled(false);
	}
}
