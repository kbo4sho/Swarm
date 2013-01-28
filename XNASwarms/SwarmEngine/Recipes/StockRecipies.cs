﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SwarmEngine
{
    public static class StockRecipies
    {
        /* RECIPE REFERENCE
         * Population Size          0/300
         * Neighborhood Radius      0/20
         * Normal Speed             0/40
         * Max Speed                0/1
         * CohesiveForce    = R     0/1
         * AligningForce    = G     0/1
         * SeperatingForce  = B     0/100
         * ChanceOfRandomSteering   0/.5
         * TendencyOfPaceKeeping    0/1
         */

        public static string Stable_A
        {
            get
            {
                string PopulationSize = "300";
                string NeighborhoodRadius = "128.08";
                string NormalSpeed = "2.62";
                string MaxSpeed = "36.46";
                string CohesiveForce = "0.92";
                string AligningForce = ".52";
                string SeperatingForce = "58.63";
                string ChanceOfRandomSteering = ".04";
                string TendencyOfPaceKeeping = ".52"; 

                return PopulationSize + "," + NeighborhoodRadius + "," + NormalSpeed + "," + MaxSpeed + "," + CohesiveForce + "," + AligningForce + "," + SeperatingForce + "," + ChanceOfRandomSteering + "," + TendencyOfPaceKeeping;
            }
        }

        public static string Recipe2()
        {
            return "15, 226.96, 10.74, 38.96, 0.82, 0.8, 51.09, 0.28, 0.46";
        }

        public static string Recipe3
        {
            get
            {
                return "300, 128, 0, 20, 1, 0, 0, 0, 0";
            }
        }
    }
}