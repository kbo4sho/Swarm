using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XNASwarms
{
    static class StockRecipies
    {
        /* Recipe Refernce
         * Population Size
         * Neighborhood Radius
         * Normal Speed
         * Max Speed
         * C1 = R   Colors are odd...
         * C2 = G
         * C3 = B
         * C4
         * C5
         * Must enter values in groups of nine.
         */

        public static string Recipe1()
        {
            string returnValue;
            returnValue = "10, 1, 3.23, 4.21, 6, .5, 12, 1, 1";
            returnValue += ", 25, 5, 5, 6, 0.82, 0.6, 51.09, 0.28, 0.46";
            return returnValue;
        }
        public static string Recipe2()
        {
            return "15, 226.96, 10.74, 38.96, 0.82, 0.6, 51.09, 0.28, 0.46";
        }
    }
}
