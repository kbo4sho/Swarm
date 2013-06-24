using SwarmEngine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNASwarmsXAML.W8.Authoring.ViewModels
{
    public class WorldControlsViewModel : INotifyPropertyChanged
    {
        public WorldControlsViewModel()
        {
        }

        private double chanceOfRandomSteeringProperty = WorldParameters.c4Max;

        public double ChanceOfRandomSteering
        {
            get
            {
                return chanceOfRandomSteeringProperty;
            }
            set
            {
                if (value != chanceOfRandomSteeringProperty)
                {
                    chanceOfRandomSteeringProperty = value;
                    WorldParameters.c4Max = value;
                    NotifyPropertyChanged("ChanceOfRandomSteering");
                }
            }
        }

        private double seperatingForceProperty = WorldParameters.c3Max;

        public double SperatingForce
        {
            get
            {
                return seperatingForceProperty;
            }
            set
            {
                if (value != seperatingForceProperty)
                {
                    seperatingForceProperty = value;
                    WorldParameters.c3Max = value;
                    NotifyPropertyChanged("SperateingForce");
                }
            }
        }

        private double alligningForceProperty = WorldParameters.c2Max;

        public double AlligningForce
        {
            get
            {
                return alligningForceProperty;
            }
            set
            {
                if (value != alligningForceProperty)
                {
                    alligningForceProperty = value;
                    WorldParameters.c2Max = value;
                    NotifyPropertyChanged("AlligningForce");
                }
            }
        }


        private double cohesiveForceProperty = WorldParameters.c1Max;

        public double CohesiveForce
        {
            get
            {
                return cohesiveForceProperty;
            }
            set
            {
                if (value != cohesiveForceProperty)
                {
                    cohesiveForceProperty = value;
                    WorldParameters.c1Max = value;
                    NotifyPropertyChanged("CohesiveForce");
                }
            }
        }

        private int normalSpeedProperty = WorldParameters.normalSpeedMax;

        public int NormalSpeed
        {
            get
            {
                return normalSpeedProperty;
            }
            set
            {
                if (value != normalSpeedProperty)
                {
                    normalSpeedProperty = value;
                    WorldParameters.normalSpeedMax = value;
                    NotifyPropertyChanged("NormalSpeed");
                }
            }
        }

        private int maxSpeedProperty = WorldParameters.maxSpeedMax;

        public int MaxSpeed
        {
            get
            {
                return maxSpeedProperty;
            }
            set
            {
                if (value != maxSpeedProperty)
                {
                    maxSpeedProperty = value;
                    WorldParameters.maxSpeedMax = value;
                    NotifyPropertyChanged("MaxSpeed");
                }
            }
        }

        private int numberOfIndividualsMaxProperty = WorldParameters.numberOfIndividualsMax;

        public int NumberOfIndividualsMax
        {
            get
            {
                return numberOfIndividualsMaxProperty;
            }
            set
            {
                if (value != numberOfIndividualsMaxProperty)
                {
                    numberOfIndividualsMaxProperty = value;
                    WorldParameters.numberOfIndividualsMax = value;
                    NotifyPropertyChanged("NumberOfIndividualsMax");
                }
            }
        }

        private int neighborhoodMaxRadiusProperty = WorldParameters.neighborhoodRadiusMax;

        public int NeighborhoodRadiusMax
        {
            get
            {
                return neighborhoodMaxRadiusProperty;
            }
            set
            {
                if (value != neighborhoodMaxRadiusProperty)
                {
                    neighborhoodMaxRadiusProperty = value;
                    WorldParameters.neighborhoodRadiusMax = value;
                    NotifyPropertyChanged("NeighborhoodRadiusMax");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
