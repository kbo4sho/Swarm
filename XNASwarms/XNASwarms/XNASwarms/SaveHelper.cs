using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SwarmEngine;
using System.IO;
using System.Xml.Serialization;

namespace XNASwarms
{
    public static class SaveHelper
    {

        public static void Save(string filename, SaveAllSpecies savespecies)
        {
#if WINDOWS
            try
            {
                Stream stream = File.Create(filename + ".xml");
                XmlSerializer serializer = new XmlSerializer(typeof(SaveAllSpecies));
                serializer.Serialize(stream, savespecies);
                stream.Close();
            }
            catch
            {
                //
            }
#endif
        }

        public static SaveAllSpecies Load(string filename)
        {

#if WINDOWS
            try
            {
                Stream stream = File.OpenRead(filename + ".xml");
                XmlSerializer serializer = new XmlSerializer(typeof(SaveAllSpecies));
                var item = (SaveAllSpecies)serializer.Deserialize(stream);
                stream.Close();
                return item;
            }
            catch
            {
                return null;
            }
#else
            return null;
#endif
        }
    }
}
