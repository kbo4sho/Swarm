using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SwarmEngine;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;

namespace XNASwarms
{
#if WINDOWS
    [Serializable]
    [XmlInclude(typeof(SaveGenome))]
#endif
    public class SaveSpecies
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

        public List<List<SaveGenome>> SavedSpecies = new List<List<SaveGenome>>();
        public SaveWorldParameters SaveWorldParameters = new SaveWorldParameters();
        public SaveSpecies()
        {
        }

        public List<Color> GetMostUsedColors()
        {
            List<Color> Colors = new List<Color>();
            
            var item = SavedSpecies.OrderByDescending(s => s.Count).ToList();
            if (item != null)
            {
                Colors.Add(item[0][0].getDisplayColor());
                if (item.Count > 1)
                {
                    Colors.Add(item[1][0].getDisplayColor());
                }
                if (item.Count > 2)
                {
                    Colors.Add(item[2][0].getDisplayColor());
                }
            }
            return Colors;
        }
    }
}
