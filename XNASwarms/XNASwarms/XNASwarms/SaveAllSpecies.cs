using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace XNASwarms
{
#if WINDOWS
    [Serializable]
#endif
    [XmlInclude(typeof(SaveSpecies))]
    public class SaveAllSpecies : List<SaveSpecies>
    {
        public SaveAllSpecies()
        {
        }
    }
}
