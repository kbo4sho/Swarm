

using System.Collections.Generic;
namespace SwarmEngine
{
    public static class StockSpecies
    {
        public static Population Species_A
        {
            get
            {
                List<Species> spcs = new List<Species>();
                Species spec1 = new Species(StockIndividualsFacotry.SetA());
                Species spec2 = new Species(StockIndividualsFacotry.SetB());
                Species spec3 = new Species(StockIndividualsFacotry.SetC());
                Species spec4 = new Species(StockIndividualsFacotry.SetD());
                spcs.Add(spec1);
                spcs.Add(spec2);
                spcs.Add(spec3);
                spcs.Add(spec4);

                return new Population(spcs, "KboPop");

            }
        }
    }
}