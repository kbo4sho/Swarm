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
            for (int i = 0; i < saveGenomes.Count; i++ )
            {
                individuals.Add(new Individual(i,
                    saveGenomes[i].x,
                    saveGenomes[i].y,
                    saveGenomes[i].dx,
                    saveGenomes[i].dy,
                    saveGenomes[i].dx2,
                    saveGenomes[i].dy2,
                    new Parameters(saveGenomes[i].neighborhoodRadius,
                                   saveGenomes[i].normalSpeed,
                                   saveGenomes[i].maxSpeed,
                                   saveGenomes[i].c1,
                                   saveGenomes[i].c2,
                                   saveGenomes[i].c3,
                                   saveGenomes[i].c4,
                                   saveGenomes[i].c5),
                    saveGenomes[i].type,
                    saveGenomes[i].isMobile)
                 );
            }

            return individuals;
        }
    }
}
