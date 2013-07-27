using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Xml.Serialization;

namespace SwarmEngine
{
    public class Individual : IContainable
    {
        private double x, y, dx, dy, dx2, dy2;
        private int rankInXOrder, rankInYOrder;
        private Color color;
        public EmitterType EmitterType
        {
            get;
            private set;
        }

        public Parameters Genome
        {
            get;
            private set;
        }

        public int ID
        {
            get;
            private set;
        }

        public double X
        {
            get { return x; }
        }

        public double Y
        {
            get { return y; }
        }

        public int RankInXOrder
        {
            get { return rankInXOrder; }
        }

        public int RankInYOrder
        {
            get { return rankInYOrder; }
        }

        public double Dx
        {
            get { return dx; }
        }

        public double Dy
        {
            get { return dy; }
        }

        public Individual()
            : this(0, 0.0, 0.0, 0.0, 0.0, new Parameters())
        {

        }

        public Individual(int id, double xx, double yy, double dxx, double dyy, Parameters g) 
            : this (id,xx,yy,dxx,dyy,g, EmitterType.Still)
        {

        }

        public Individual(int id, double xx, double yy, double dxx, double dyy, Parameters g, EmitterType type)
        {
            ID = id;
            x = xx;
            y = yy;
            dx = dx2 = dxx;
            dy = dy2 = dyy;
            Genome = g;
            EmitterType = type;
            color = Genome.getDisplayColor(type);
            rankInXOrder = 0;
            rankInYOrder = 0;
        }

        public void accelerate(double ax, double ay, double maxMove)
        {
            dx2 += ax;
            dy2 += ay;

            double d = dx2 * dx2 + dy2 * dy2;
            if (d > maxMove * maxMove)
            {
                double normalizationFactor = maxMove / Math.Sqrt(d); //was Math.sqrt(d)
                dx2 *= normalizationFactor;
                dy2 *= normalizationFactor;
            }
        }

        public void stepSimulation()
        {
            dx = dx2;
            dy = dy2;
            x += dx;
            y += dy;
        }

        public void setDisplayColor(Color clr)
        {
            this.color = clr;
        }

        public Color getDisplayColor()
        {
            return this.color;
        }

        public Color getGenomeColor()
        {
            return Genome.getDisplayColor(this.EmitterType);
        }

        public void setRankInXOrder(int rankInXOrder)
        {
            this.rankInXOrder = rankInXOrder;
        }

        public void setRankInYOrder(int rankInYOrder)
        {
            this.rankInYOrder = rankInYOrder;
        }

        public double getDx()
        {
            return dx;
        }

        public double getDy()
        {
            return dy;
        }

        public double getDx2()
        {
            return dx2;
        }

        public double getDy2()
        {
            return dy2;
        }

        #region IContainable
        public void TravelThroughXWall()
        {
            x = -x;
            dx = -dx;
            dx2 = dx2 * 100;
        }
        public void TravelThroughYWall()
        {
            y = -y;
            dy = -dy;
            dy2 = dy2 * 100;
        }

        public void BounceXWall()
        {
            dx = -dx * 1000;
            dx2 = -dx2 * 1000;
        }
        public void BounceYWall()
        {
            dy = -dy * 1000;
            dy2 = -dy2 * 1000;
        }
        #endregion
    }
}



