using SwarmEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNASwarms
{
    public static class SwarmSaveHelper
    {
        public static SaveSpecies GetPopulationAsSaveSpecies(Population population)
        {
            SaveSpecies saveSpecies = new SaveSpecies();
            saveSpecies.CreadtedDt = DateTime.Now;
            foreach (Species species in population)
            {
                saveSpecies.SavedSpecies.Add(GetSavedGenomes(species));
            }
            return saveSpecies;
        }

        private static List<SaveGenome> GetSavedGenomes(Species species)
        {
            List<SaveGenome> savedGenomes = new List<SaveGenome>();
            foreach (Individual individual in species)
            {
                savedGenomes.Add(SwarmSaveHelper.GetSavedGenomeFromIndividual(individual));
            }
            return savedGenomes;
        }

        private static SaveGenome GetSavedGenomeFromIndividual(Individual individual)
        {
            SaveGenome savedGenome = new SaveGenome();
            savedGenome.neighborhoodRadius = individual.Genome.getNeighborhoodRadius();
            savedGenome.normalSpeed = individual.Genome.getNormalSpeed();
            savedGenome.maxSpeed = individual.Genome.getMaxSpeed();
            savedGenome.c1 = individual.Genome.getC1();
            savedGenome.c2 = individual.Genome.getC2();
            savedGenome.c3 = individual.Genome.getC3();
            savedGenome.c4 = individual.Genome.getC4();
            savedGenome.c5 = individual.Genome.getC5();
            savedGenome.x = individual.X;
            savedGenome.y = individual.Y;
            savedGenome.dx = individual.Dx;
            savedGenome.dy = individual.Dy;
            savedGenome.dx2 = individual.Dx2;
            savedGenome.dy2 = individual.Dy2;
            savedGenome.type = individual.EmitterType;
            savedGenome.isMobile = individual.IsMobile;
            return savedGenome;
        }
    }
}
