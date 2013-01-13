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
            //string returnValue;
            //returnValue = "1, .5, 1, 4.21, 6, .5, 12, 1, 1";
            //returnValue += ", 25, 5, 5, 6, 0.82, 0.6, 51.09, 0.28, 0.46";
            //return returnValue;
            string PopulationSize           = "283"; //0,1200

            //string NeighborhoodRadius       = "289.69";  //0,300
            //string NormalSpeed              = "19.52";   //0,20
            //string MaxSpeed                 = "1.83";   //0,40
            //string CohesiveForce            = ".54";  //0,1
            //string AligningForce            = ".94";  //0,1
            //string SeperatingForce          = "75"; //0,100
            //string ChanceOfRandomSteering   = ".32";  //0,.5
            //string TendencyOfPaceKeeping    = "1";   //0,1

            string NeighborhoodRadius = "128.08";  //0,300
            string NormalSpeed = "2.62";   //0,20
            string MaxSpeed = "36.46";   //0,40
            string CohesiveForce = "0.92";  //0,1
            string AligningForce = ".52";  //0,1
            string SeperatingForce = "58.63"; //0,100100
            string ChanceOfRandomSteering = ".04";  //0,.5
            string TendencyOfPaceKeeping = ".52";   //0,1white

            return  PopulationSize + "," + NeighborhoodRadius + "," + NormalSpeed + "," + MaxSpeed + "," + CohesiveForce + "," + AligningForce + "," + SeperatingForce + "," + ChanceOfRandomSteering + "," + TendencyOfPaceKeeping;
            //return "224, 71.22, 10, 10, 1, 0.02, 15, 0.36, 1";//returnstr;
        }
        public static string Recipe2()
        {
            return "15, 226.96, 10.74, 38.96, 0.82, 0.8, 51.09, 0.28, 0.46";
        }
    }
}
