using SwarmEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNASwarms
{
    public static class SaveSpeciesHelper
    {
        public static Population GetPopulationFromSaveSpecies(SaveSpecies savespecies)
        {
            List<Species> species = new List<Species>();
            foreach (List<SaveGenome> genome in savespecies.SavedSpecies)
            {
                species.Add(GetIndividualsFromSaveGenome(genome));
            }
            return new Population(species, "KboSaved");
        }

        private static Species GetIndividualsFromSaveGenome(List<SaveGenome> saveGenomes)
        {
            Random rand = new Random();
            Species individuals = new Species();
            foreach (SaveGenome savegenome in saveGenomes)
            {
                individuals.Add(new Individual(rand.NextDouble() * 0,
                    rand.NextDouble() * 0, rand.NextDouble() * 10 - 5,
                    rand.NextDouble() * 10 - 5, new Parameters(savegenome.neighborhoodRadius, savegenome.normalSpeed, savegenome.maxSpeed, savegenome.c1, savegenome.c2, savegenome.c3, savegenome.c4, savegenome.c5)));
            }
            return individuals;
        }
    }
}
