using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

/*
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
*/
namespace SwarmEngine
{
    public class Parameters : IComparable
    {     
        protected double neighborhoodRadius;
        protected double normalSpeed;
        protected double maxSpeed;
        protected double c1;
        protected double c2;
        protected double c3;
        protected double c4;
        protected double c5;

        private Random rand = new Random();

        public Parameters()
        {
            neighborhoodRadius = rand.NextDouble() * WorldParameters.neighborhoodRadiusMax;
            normalSpeed = rand.NextDouble() * WorldParameters.normalSpeedMax;
            maxSpeed = rand.NextDouble() * WorldParameters.maxSpeedMax;
            c1 = rand.NextDouble() * WorldParameters.CohesiveForceMax;
            c2 = rand.NextDouble() * WorldParameters.AligningForceMax;
            c3 = rand.NextDouble() * WorldParameters.SeperatingForceMax;
            c4 = rand.NextDouble() * WorldParameters.ChanceOfRandomSteeringMax;
            c5 = rand.NextDouble() * WorldParameters.TendencyOfPaceKeepingMax;
        }

        public string getRecipe()
        {
            StringBuilder sb = new StringBuilder("");
            sb.Append((int)WorldParameters.numberOfIndividualsMax);
            sb.Append(", ");
            sb.Append((double)getNeighborhoodRadius());
            sb.Append(", ");
            sb.Append((double)getNormalSpeed());
            sb.Append(", ");
            sb.Append((double)getMaxSpeed());
            sb.Append(", ");
            sb.Append((double)c1);
            sb.Append(", ");
            sb.Append((double)c2);
            sb.Append(", ");
            sb.Append((double)c3);
            sb.Append(", ");
            sb.Append((double)c4);
            sb.Append(", ");
            sb.Append((double)c5);
            return sb.ToString();
        }

        public Parameters(double p1, double p2, double p3, double p4,
                double p5, double p6, double p7, double p8)
        {
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

        public Parameters(Parameters parent)
        {
            neighborhoodRadius = parent.neighborhoodRadius;
            normalSpeed = parent.normalSpeed;
            maxSpeed = parent.maxSpeed;
            c1 = parent.c1;
            c2 = parent.c2;
            c3 = parent.c3;
            c4 = parent.c4;
            c5 = parent.c5;
        }


        public int hashCode()
        {
            int prime = 31;
            int result = 1;
            long temp;
            //all BitConverter.DoubleToInt64Bits were Double.DoubleToInt64Bits
            temp = BitConverter.DoubleToInt64Bits(c1);
            result = prime * result + (int)(temp ^ (temp >> 32)); //all >> were >>
            temp = BitConverter.DoubleToInt64Bits(c2);
            result = prime * result + (int)(temp ^ (temp >> 32));
            temp = BitConverter.DoubleToInt64Bits(c3);
            result = prime * result + (int)(temp ^ (temp >> 32));
            temp = BitConverter.DoubleToInt64Bits(c4);
            result = prime * result + (int)(temp ^ (temp >> 32));
            temp = BitConverter.DoubleToInt64Bits(c5);
            result = prime * result + (int)(temp ^ (temp >> 32));
            temp = BitConverter.DoubleToInt64Bits(maxSpeed);
            result = prime * result + (int)(temp ^ (temp >> 32));
            temp = BitConverter.DoubleToInt64Bits(neighborhoodRadius);
            result = prime * result + (int)(temp ^ (temp >> 32));
            temp = BitConverter.DoubleToInt64Bits(normalSpeed);
            result = prime * result + (int)(temp ^ (temp >> 32));
            return result;
        }


        public bool equals(Object obj)
        {
            if (this == obj)
                return true;
            if (obj == null)
                return false;
            //GetType was getClass
            if (GetType() != obj.GetType())
                return false;
            Parameters other = (Parameters)obj;
            if (BitConverter.DoubleToInt64Bits(c1) != BitConverter.DoubleToInt64Bits(other.c1))
                return false;
            if (BitConverter.DoubleToInt64Bits(c2) != BitConverter.DoubleToInt64Bits(other.c2))
                return false;
            if (BitConverter.DoubleToInt64Bits(c3) != BitConverter.DoubleToInt64Bits(other.c3))
                return false;
            if (BitConverter.DoubleToInt64Bits(c4) != BitConverter.DoubleToInt64Bits(other.c4))
                return false;
            if (BitConverter.DoubleToInt64Bits(c5) != BitConverter.DoubleToInt64Bits(other.c5))
                return false;
            if (BitConverter.DoubleToInt64Bits(maxSpeed) != BitConverter
                    .DoubleToInt64Bits(other.maxSpeed))
                return false;
            if (BitConverter.DoubleToInt64Bits(neighborhoodRadius) != BitConverter
                    .DoubleToInt64Bits(other.neighborhoodRadius))
                return false;
            if (BitConverter.DoubleToInt64Bits(normalSpeed) != BitConverter
                    .DoubleToInt64Bits(other.normalSpeed))
                return false;
            return true;
        }

        public void inducePointMutations(double rate, double magnitude)
        {
            if (rand.NextDouble() < rate)
                neighborhoodRadius += (rand.NextDouble() - 0.5) * WorldParameters.neighborhoodRadiusMax
                        * magnitude;

            if (rand.NextDouble() < rate)
                normalSpeed += (rand.NextDouble() - 0.5) * WorldParameters.normalSpeedMax * magnitude;

            if (rand.NextDouble() < rate)
                maxSpeed += (rand.NextDouble() - 0.5) * WorldParameters.maxSpeedMax * magnitude;

            if (rand.NextDouble() < rate)
                c1 += (rand.NextDouble() - 0.5) * WorldParameters.CohesiveForceMax * magnitude;

            if (rand.NextDouble() < rate)
                c2 += (rand.NextDouble() - 0.5) * WorldParameters.AligningForceMax * magnitude;

            if (rand.NextDouble() < rate)
                c3 += (rand.NextDouble() - 0.5) * WorldParameters.SeperatingForceMax * magnitude;

            if (rand.NextDouble() < rate)
                c4 += (rand.NextDouble() - 0.5) * WorldParameters.ChanceOfRandomSteeringMax * magnitude;

            if (rand.NextDouble() < rate)
                c5 += (rand.NextDouble() - 0.5) * WorldParameters.TendencyOfPaceKeepingMax * magnitude;

            boundParameterValues();
        }

        private void boundParameterValues()
        {
            if (neighborhoodRadius < 0)
                neighborhoodRadius = 0;
            else if (neighborhoodRadius > WorldParameters.neighborhoodRadiusMax)
                neighborhoodRadius = WorldParameters.neighborhoodRadiusMax;

            if (normalSpeed < 0)
                normalSpeed = 0;
            else if (normalSpeed > WorldParameters.normalSpeedMax)
                normalSpeed = WorldParameters.normalSpeedMax;

            if (maxSpeed < 0)
                maxSpeed = 0;
            else if (maxSpeed > WorldParameters.maxSpeedMax)
                maxSpeed = WorldParameters.maxSpeedMax;

            if (c1 < 0)
                c1 = 0;
            else if (c1 > WorldParameters.CohesiveForceMax)
                c1 = WorldParameters.CohesiveForceMax;

            if (c2 < 0)
                c2 = 0;
            else if (c2 > WorldParameters.AligningForceMax)
                c2 = WorldParameters.AligningForceMax;

            if (c3 < 0)
                c3 = 0;
            else if (c3 > WorldParameters.SeperatingForceMax)
                c3 = WorldParameters.SeperatingForceMax;

            if (c4 < 0)
                c4 = 0;
            else if (c4 > WorldParameters.ChanceOfRandomSteeringMax)
                c4 = WorldParameters.ChanceOfRandomSteeringMax;

            if (c5 < 0)
                c5 = 0;
            else if (c5 > WorldParameters.TendencyOfPaceKeepingMax)
                c5 = WorldParameters.TendencyOfPaceKeepingMax;
        }

        public Color getDisplayColor()
        {
            return new Color((float)(c1 / WorldParameters.CohesiveForceMax * 0.8),
                    (float)(c2 / WorldParameters.AligningForceMax * 0.8), (float)(c3 / WorldParameters.SeperatingForceMax* 0.8));
        }

        public double getNeighborhoodRadius()
        {
            return neighborhoodRadius;
        }

        public double getNormalSpeed()
        {
            return normalSpeed;
        }

        public double getMaxSpeed()
        {
            return maxSpeed;
        }

        public double getC1()
        {
            return c1;
        }

        public double getC2()
        {
            return c2;
        }

        public double getC3()
        {
            return c3;
        }

        public double getC4()
        {
            return c4;
        }

        public double getC5()
        {
            return c5;
        }

        #region IComparable
        public int CompareTo(object obj)
        {
            if (obj is Parameters)
            {
                Parameters p2 = (Parameters)obj;
                return getRecipe().CompareTo(p2.getRecipe());
            }
            else
            {
                throw new ArgumentException("Object is not a Person.");
            }
        }
        #endregion
    }
}

