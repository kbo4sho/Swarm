using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNASwarms
{
    public static class StockSaveSpecies
    {
        public static SaveSpecies GetLavaLamp
        {
            get
            {
                SaveSpecies lavalamp = new SaveSpecies();
                lavalamp.SavedSpecies.Add(GetSpecies(38, 400, 55.95, 10, 13.75, 1, 1, 88.61, 0.19, 0));
                lavalamp.SavedSpecies.Add(GetSpecies(14, 400, 55.95, 10, 13.75, 1, 0.52, 58.63, 0.18, 0.83));
                lavalamp.SavedSpecies.Add(GetSpecies(2, 400, 128.08, 2.62, 20, 0.92, .52, 58.63, 0.04, 0.52));
                lavalamp.SavedSpecies.Add(GetSpecies(49, 400, 55.95, 10, 20, 0.92, 0.99, 58.63, 0.04, 0.52));
                lavalamp.SavedSpecies.Add(GetSpecies(18, 400, 55.95, 10, 20, 0, 0.52, 58.63, 0.04, 0.81));
                lavalamp.SavedSpecies.Add(GetSpecies(29, 400, 55.95, 10, 13.75, 1, 0.52, 92.94, 0.10, 0.58));
                lavalamp.SavedSpecies.Add(GetSpecies(29, 400, 128.08, 2.62, 20, 0.92, 0.52, 58.63, 0.02, 0.52));
                lavalamp.SavedSpecies.Add(GetSpecies(105, 400, 55.95, 10, 20, 0, 0.52, 58.63, 0.04, 0.52));
                lavalamp.SavedSpecies.Add(GetSpecies(16, 400, 128.08, 2.62, 20, 0.92, 0.52, 100, 0.04, 0.52));
                return lavalamp;
            }
        }

        private static List<SaveGenome> GetSpecies(int numberof, int count, double p1, double p2, double p3, double p4, double p5, double p6, double p7, double p8)
        {
            List<SaveGenome> returnSpecies = new List<SaveGenome>();

            for (int i = 0; i < numberof; i++)
            {
                SaveGenome saveGenome = new SaveGenome();
                saveGenome.neighborhoodRadius = p1;
                saveGenome.normalSpeed = p2;
                saveGenome.maxSpeed = p3;
                saveGenome.c1 = p4;
                saveGenome.c2 = p5;
                saveGenome.c3 = p6;
                saveGenome.c4 = p7;
                saveGenome.c5 = p8;
                returnSpecies.Add(saveGenome);
            }

            return returnSpecies;
        }
    }
}
