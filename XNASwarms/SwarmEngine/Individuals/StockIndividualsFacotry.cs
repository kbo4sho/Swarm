using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SwarmEngine
{
    public static class StockIndividualsFacotry
    {
        

        public static List<Individual> GenerateSet(int howmany)
        {
            Random rand = new Random(); ;
            List<Individual> indvdls = new List<Individual>();

            for (int i = 0; i < howmany; i++)
            {
                indvdls.Add(new Individual(rand.NextDouble() * 0,
                    rand.NextDouble() * 0, rand.NextDouble() * 10 - 5,
                    rand.NextDouble() * 10 - 5, new Parameters(226.96, 10.74, 38.96, 0.82, 0.8, 51.09, 0.28, 0.46)));

            }
            return indvdls;
        }

        public static List<Individual> SetA()
        {
            Random rand = new Random(); ;
            List<Individual> indvdls = new List<Individual>();

            for (int i = 0; i < 20; i++)
            {
                indvdls.Add(new Individual(rand.NextDouble() * 0,
                    rand.NextDouble() * 0, rand.NextDouble() * 10 - 5,
                    rand.NextDouble() * 10 - 5, new Parameters(226.96, 10.74, 38.96, 0.82, 0.8, 51.09, 0.28, 0.46)));

            }
            return indvdls;
        }

        public static List<Individual> SetB()
        {
            Random rand = new Random(); ;
            List<Individual> indvdls = new List<Individual>();

            for (int i = 0; i < 10; i++)
            {
                indvdls.Add(new Individual(rand.NextDouble() * 0,
                    rand.NextDouble() * 0, rand.NextDouble() * 10 - 5,
                    rand.NextDouble() * 10 - 5, new Parameters(300, 10.74, 38.96, 0.82, 0.2, 51.09, 0.28, 0.46)));

            }
            return indvdls;
        }

        public static List<Individual> SetC()
        {
            Random rand = new Random(); ;
            List<Individual> indvdls = new List<Individual>();

            for (int i = 0; i < 50; i++)
            {
                indvdls.Add(new Individual(rand.NextDouble() * 0,
                    rand.NextDouble() * 0, rand.NextDouble() * 10 - 5,
                    rand.NextDouble() * 10 - 5, new Parameters(300, 10.74, 38.96, 0.55, 0.1, 51.09, 0.28, 0.46)));

            }
            return indvdls;
        }

        public static List<Individual> SetD()
        {
            Random rand = new Random(); ;
            List<Individual> indvdls = new List<Individual>();

            for (int i = 0; i < 140; i++)
            {
                indvdls.Add(new Individual(rand.NextDouble() * 0,
                    rand.NextDouble() * 0, rand.NextDouble() * 10 - 5,
                    rand.NextDouble() * 10 - 5, new Parameters(300, 10.74, 38.96, 0.82, 0.2, 21.09, 0.28, 0.46)));

            }
            return indvdls;
        }
    }
}
