package swarm.engine;

// SwarmParameters.java
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

import java.awt.Color;

public class Parameters {
	public final static int numberOfIndividualsMax = 300;

	private double neighborhoodRadius;
	public final static double neighborhoodRadiusMax = 300;

	private double normalSpeed;
	public final static double normalSpeedMax = 20;

	private double maxSpeed;
	public final static double maxSpeedMax = 40;

	private double c1;
	public final static double c1Max = 1;

	private double c2;
	public final static double c2Max = 1;

	private double c3;
	public final static double c3Max = 100;

	private double c4;
	public final static double c4Max = 0.5;

	private double c5;
	public final static double c5Max = 1;

	public Parameters() {
		neighborhoodRadius = Math.random() * neighborhoodRadiusMax;
		normalSpeed = Math.random() * normalSpeedMax;
		maxSpeed = Math.random() * maxSpeedMax;
		c1 = Math.random() * c1Max;
		c2 = Math.random() * c2Max;
		c3 = Math.random() * c3Max;
		c4 = Math.random() * c4Max;
		c5 = Math.random() * c5Max;
	}

	public Parameters(double p1, double p2, double p3, double p4,
			double p5, double p6, double p7, double p8) {
		neighborhoodRadius = p1;
		normalSpeed = p2;
		maxSpeed = p3;
		c1 = p4;
		c2 = p5;
		c3 = p6;
		c4 = p7;
		c5 = p8;

		boundParameterValues();
	}

	public Parameters(Parameters parent) {
		neighborhoodRadius = parent.neighborhoodRadius;
		normalSpeed = parent.normalSpeed;
		maxSpeed = parent.maxSpeed;
		c1 = parent.c1;
		c2 = parent.c2;
		c3 = parent.c3;
		c4 = parent.c4;
		c5 = parent.c5;
	}

	@Override
	public int hashCode() {
		final int prime = 31;
		int result = 1;
		long temp;
		temp = Double.doubleToLongBits(c1);
		result = prime * result + (int) (temp ^ (temp >>> 32));
		temp = Double.doubleToLongBits(c2);
		result = prime * result + (int) (temp ^ (temp >>> 32));
		temp = Double.doubleToLongBits(c3);
		result = prime * result + (int) (temp ^ (temp >>> 32));
		temp = Double.doubleToLongBits(c4);
		result = prime * result + (int) (temp ^ (temp >>> 32));
		temp = Double.doubleToLongBits(c5);
		result = prime * result + (int) (temp ^ (temp >>> 32));
		temp = Double.doubleToLongBits(maxSpeed);
		result = prime * result + (int) (temp ^ (temp >>> 32));
		temp = Double.doubleToLongBits(neighborhoodRadius);
		result = prime * result + (int) (temp ^ (temp >>> 32));
		temp = Double.doubleToLongBits(normalSpeed);
		result = prime * result + (int) (temp ^ (temp >>> 32));
		return result;
	}

	@Override
	public boolean equals(Object obj) {
		if (this == obj)
			return true;
		if (obj == null)
			return false;
		if (getClass() != obj.getClass())
			return false;
		Parameters other = (Parameters) obj;
		if (Double.doubleToLongBits(c1) != Double.doubleToLongBits(other.c1))
			return false;
		if (Double.doubleToLongBits(c2) != Double.doubleToLongBits(other.c2))
			return false;
		if (Double.doubleToLongBits(c3) != Double.doubleToLongBits(other.c3))
			return false;
		if (Double.doubleToLongBits(c4) != Double.doubleToLongBits(other.c4))
			return false;
		if (Double.doubleToLongBits(c5) != Double.doubleToLongBits(other.c5))
			return false;
		if (Double.doubleToLongBits(maxSpeed) != Double
				.doubleToLongBits(other.maxSpeed))
			return false;
		if (Double.doubleToLongBits(neighborhoodRadius) != Double
				.doubleToLongBits(other.neighborhoodRadius))
			return false;
		if (Double.doubleToLongBits(normalSpeed) != Double
				.doubleToLongBits(other.normalSpeed))
			return false;
		return true;
	}

	public void inducePointMutations(double rate, double magnitude) {
		if (Math.random() < rate)
			neighborhoodRadius += (Math.random() - 0.5) * neighborhoodRadiusMax
					* magnitude;

		if (Math.random() < rate)
			normalSpeed += (Math.random() - 0.5) * normalSpeedMax * magnitude;

		if (Math.random() < rate)
			maxSpeed += (Math.random() - 0.5) * maxSpeedMax * magnitude;

		if (Math.random() < rate)
			c1 += (Math.random() - 0.5) * c1Max * magnitude;

		if (Math.random() < rate)
			c2 += (Math.random() - 0.5) * c2Max * magnitude;

		if (Math.random() < rate)
			c3 += (Math.random() - 0.5) * c3Max * magnitude;

		if (Math.random() < rate)
			c4 += (Math.random() - 0.5) * c4Max * magnitude;

		if (Math.random() < rate)
			c5 += (Math.random() - 0.5) * c5Max * magnitude;

		boundParameterValues();
	}

	private void boundParameterValues() {
		if (neighborhoodRadius < 0)
			neighborhoodRadius = 0;
		else if (neighborhoodRadius > neighborhoodRadiusMax)
			neighborhoodRadius = neighborhoodRadiusMax;

		if (normalSpeed < 0)
			normalSpeed = 0;
		else if (normalSpeed > normalSpeedMax)
			normalSpeed = normalSpeedMax;

		if (maxSpeed < 0)
			maxSpeed = 0;
		else if (maxSpeed > maxSpeedMax)
			maxSpeed = maxSpeedMax;

		if (c1 < 0)
			c1 = 0;
		else if (c1 > c1Max)
			c1 = c1Max;

		if (c2 < 0)
			c2 = 0;
		else if (c2 > c2Max)
			c2 = c2Max;

		if (c3 < 0)
			c3 = 0;
		else if (c3 > c3Max)
			c3 = c3Max;

		if (c4 < 0)
			c4 = 0;
		else if (c4 > c4Max)
			c4 = c4Max;

		if (c5 < 0)
			c5 = 0;
		else if (c5 > c5Max)
			c5 = c5Max;
	}

	public Color getDisplayColor() {
		return new Color((float) (c1 / c1Max * 0.8),
				(float) (c2 / c2Max * 0.8), (float) (c3 / c3Max * 0.8));
	}

	public double getNeighborhoodRadius() {
		return neighborhoodRadius;
	}

	public double getNormalSpeed() {
		return normalSpeed;
	}

	public double getMaxSpeed() {
		return maxSpeed;
	}

	public double getC1() {
		return c1;
	}

	public double getC2() {
		return c2;
	}

	public double getC3() {
		return c3;
	}

	public double getC4() {
		return c4;
	}

	public double getC5() {
		return c5;
	}
}
