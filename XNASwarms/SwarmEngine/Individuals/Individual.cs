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
        private int rankInXOrder, rankInYOrder;

        private Color color;

        public bool IsMobile
        {
            get;
            private set;
        }

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
            get;
            private set;
        }

        public double Y
        {
            get;
            private set;
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
            get;
            private set;
        }

        public double Dy
        {
            get;
            private set;
        }

        public double Dx2
        {
            get;
            private set;
        }

        public double Dy2
        {
            get;
            private set;
        }

        #region Constructor
        public Individual()
            : this(0, 0.0, 0.0, 0.0, 0.0, new Parameters())
        {

        }

        public Individual(int id, double x, double y, double dx, double dy, Parameters g) 
            : this (id,x,y,dx,dy,g, EmitterType.Still, true)
        {

        }

        public Individual(int id, double x, double y, double dx, double dy, Parameters g, EmitterType emitterType, bool isMobile)
            : this(id, x, y, dx, dy, dx, dy, g, emitterType, isMobile)
        {

        }

        public Individual(int id, double x, double y, double dx, double dy, double dx2, double dy2, Parameters g, EmitterType emitterType, bool isMobile)
        {
            ID = id;
            X = x;
            Y = y;
            Dx = Dx2 = dx;
            Dy = Dy2 = dy;
            Genome = g;
            EmitterType = emitterType;
            IsMobile = isMobile;

            color = Genome.getDisplayColor(emitterType);
            rankInXOrder = 0;
            rankInYOrder = 0;
        }

        #endregion

        public void accelerate(double ax, double ay, double maxMove)
        {
            Dx2 += ax;
            Dy2 += ay;

            double d = Dx2 * Dx2 + Dy2 * Dy2;
            if (d > maxMove * maxMove)
            {
                double normalizationFactor = maxMove / Math.Sqrt(d); //was Math.sqrt(d)
                Dx2 *= normalizationFactor;
                Dy2 *= normalizationFactor;
            }
        }

        public void stepSimulation()
        {
            Dx = Dx2;
            Dy = Dy2;
            X += Dx;
            Y += Dy;
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

        #region IContainable
        public void TravelThroughXWall()
        {
            X = -X;
            Dx = -Dx;
            Dx2 = Dx2 * 100;
        }
        public void TravelThroughYWall()
        {
            Y = -Y;
            Dy = -Dy;
            Dy2 = Dy2 * 100;
        }

        public void BounceXWall()
        {
            Dx = -Dx * 1000;
            Dx2 = -Dx2 * 1000;
        }
        public void BounceYWall()
        {
            Dy = -Dy * 1000;
            Dy2 = -Dy2 * 1000;
        }
        #endregion
    }
}



