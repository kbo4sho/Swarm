using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SwarmEngine;
using System.Xml.Serialization;

namespace XNASwarms
{
    [Serializable]
    [XmlInclude(typeof(SaveGenome))]
    public class SaveSpecies : List<List<SaveGenome>>
    {
        //normal DateTime accessor
        [XmlIgnore]
        public DateTime CreadtedDt { get; set; }

        public string XmlDateTime
        {
            get
            {
                return this.CreadtedDt.ToString("o");
            }
            set
            {
                this.CreadtedDt = DateTime.Parse(value);
            }
        }

        public List<List<List<SaveGenome>>> SavedSpecies = new List<List<List<SaveGenome>>>();

        public SaveSpecies()
        {
        }
    }
}
