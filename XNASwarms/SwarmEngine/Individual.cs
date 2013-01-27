using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#if Surface
using Microsoft.Surface.Core;
#endif
using Microsoft.Xna.Framework;

namespace SwarmEngine
{
     public class Individual: IContainable
     {

        private double x, y, dx, dy, dx2, dy2;
	    private Parameters genome;
	    private int rankInXOrder, rankInYOrder;

        public Individual() :  this(0.0, 0.0, 0.0, 0.0, new Parameters())
        {
            
        }

        public Individual(double xx, double yy, double dxx, double dyy, Parameters g)
        {
            x = xx;
            y = yy;
            dx = dx2 = dxx;
            dy = dy2 = dyy;
            genome = g;
            rankInXOrder = 0;
            rankInYOrder = 0;
        }

	    public void accelerate(double ax, double ay, double maxMove) 
        {

            dx2 += ax;
            dy2 += ay;

            //if ( y > 0 && y < 10 && dy2 > 0 && dy2 < 10)
            //{
            //    dy2 += ay;
            //}
            //else
            //{
            //    dy2 = -ay;
            //}

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

	    public Color getDisplayColor() {
		    return genome.getDisplayColor();
	    }

	    public int getRankInXOrder() {
		    return rankInXOrder;
	    }

	    public void setRankInXOrder(int rankInXOrder) {
		    this.rankInXOrder = rankInXOrder;
	    }

	    public int getRankInYOrder() {
		    return rankInYOrder;
	    }

	    public void setRankInYOrder(int rankInYOrder) {
		    this.rankInYOrder = rankInYOrder;
	    }

	    public double getX() {
		    return x;
	    }

	    public double getY() {
		    return y;
	    }

	    public double getDx() {
		    return dx;
	    }

	    public double getDy() {
		    return dy;
	    }

	    public double getDx2() {
		    return dx2;
	    }

	    public double getDy2() {
		    return dy2;
	    }

	    public Parameters getGenome() {
		    return genome;
	    }

        #region IContainable
        public void TravelThroughXWall()
        {
            //x = x ;
            x = -x;
            dx = -dx * 400;
            dx2 = dx2 * 400;
        }
        public void TravelThroughYWall()
        {
            y = -y;
            dy = -dy * 400;
            dy2 = dy2 * 400;
        }

        public void BounceXWall()
        {
            x = x ;
            dx = -dx * 1000;
            dx2 = -dx2 * 1000;
        }
        public void BounceYWall()
        {
            x = x;
            dy = -dy * 1000;
            dy2 = -dy2 * 1000;
        }
        #endregion


     }
}



