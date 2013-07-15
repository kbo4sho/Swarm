using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNASwarms.Emitters;

namespace XNASwarmsXAML.W8.Authoring.ViewModels
{
    public class BrushControlViewModel : ControlViewModel, INotifyPropertyChanged
    {
        public BrushControlViewModel()
            : base("Brush")
        {

        }

        private double chanceOfRandomSteeringProperty = StaticBrushParameters.ChanceOfRandomSteeringMax;
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
                    StaticBrushParameters.ChanceOfRandomSteeringMax = value;
                    NotifyPropertyChanged("ChanceOfRandomSteering");
                }
            }
        }

        private double seperatingForceProperty = StaticBrushParameters.SeperatingForceMax;
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
                    StaticBrushParameters.SeperatingForceMax = value;
                    NotifyPropertyChanged("SperateingForce");
                }
            }
        }

        private double alligningForceProperty = StaticBrushParameters.AligningForceMax;
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
                    StaticBrushParameters.AligningForceMax = value;
                    NotifyPropertyChanged("AlligningForce");
                }
            }
        }

        private double cohesiveForceProperty = StaticBrushParameters.CohesiveForceMax;
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
                    StaticBrushParameters.CohesiveForceMax = value;
                    NotifyPropertyChanged("CohesiveForce");
                }
            }
        }

        private int normalSpeedProperty = StaticBrushParameters.normalSpeedMax;
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
                    StaticBrushParameters.normalSpeedMax = value;
                    NotifyPropertyChanged("NormalSpeed");
                }
            }
        }

        private int maxSpeedProperty = StaticBrushParameters.maxSpeedMax;
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
                    StaticBrushParameters.maxSpeedMax = value;
                    NotifyPropertyChanged("MaxSpeed");
                }
            }
        }

        private int numberOfIndividualsMaxProperty = StaticBrushParameters.numberOfIndividualsMax;
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
                    StaticBrushParameters.numberOfIndividualsMax = value;
                    NotifyPropertyChanged("NumberOfIndividualsMax");
                }
            }
        }

        private int neighborhoodMaxRadiusProperty = StaticBrushParameters.neighborhoodRadiusMax;
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
                    StaticBrushParameters.neighborhoodRadiusMax = value;
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
