using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XNASwarms
{
    [Serializable]
    public class SaveGenome
    {
        public double neighborhoodRadius;
        public double normalSpeed;
        public double maxSpeed;
        public double c1;
        public double c2;
        public double c3;
        public double c4;
        public double c5;

        public SaveGenome()
        {

        }
    }
}
