using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SwarmEngine
{
    [Serializable]
    [XmlInclude(typeof(Individual))]
    public class Species : List<Individual>
    {
        private Parameters parameters;

        public Species()
            : this(new List<Individual>())
        {
        }

        public Species(List<Individual> indvds)
        {
            this.AddRange(indvds);
        }


        public Individual get(int index)
        {
            return this[index];
        }



        public object getDisplayColor()
        {
            throw new NotImplementedException();
        }
    }
}
