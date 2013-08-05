using SwarmEngine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNASwarms;
using XNASwarmsXAML.W8.Authoring.Commands;

namespace XNASwarmsXAML.W8.Authoring.ViewModels
{
    public class WorldControlViewModel : ControlViewModel, INotifyPropertyChanged
    {

        public WorldControlViewModel(IControlClient controlClient)
            : base("World", "Earth.png", controlClient)
        {
        }

        private double chanceOfRandomSteeringProperty = StaticWorldParameters.ChanceOfRandomSteeringMax;
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
                    StaticWorldParameters.ChanceOfRandomSteeringMax = value;
                    NotifyPropertyChanged("ChanceOfRandomSteering");
                }
            }
        }

        private double seperatingForceProperty = StaticWorldParameters.SeperatingForceMax;
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
                    StaticWorldParameters.SeperatingForceMax = value;
                    NotifyPropertyChanged("SperateingForce");
                }
            }
        }

        private double alligningForceProperty = StaticWorldParameters.AligningForceMax;
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
                    StaticWorldParameters.AligningForceMax = value;
                    NotifyPropertyChanged("AlligningForce");
                }
            }
        }

        private double cohesiveForceProperty = StaticWorldParameters.CohesiveForceMax;
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
                    StaticWorldParameters.CohesiveForceMax = value;
                    NotifyPropertyChanged("CohesiveForce");
                }
            }
        }

        private int normalSpeedProperty = StaticWorldParameters.normalSpeedMax;
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
                    StaticWorldParameters.normalSpeedMax = value;
                    NotifyPropertyChanged("NormalSpeed");
                }
            }
        }

        private int maxSpeedProperty = StaticWorldParameters.maxSpeedMax;
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
                    StaticWorldParameters.maxSpeedMax = value;
                    NotifyPropertyChanged("MaxSpeed");
                }
            }
        }

        private int numberOfIndividualsMaxProperty = StaticWorldParameters.numberOfIndividualsMax;
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
                    StaticWorldParameters.numberOfIndividualsMax = value;
                    NotifyPropertyChanged("NumberOfIndividualsMax");
                }
            }
        }

        private int neighborhoodMaxRadiusProperty = StaticWorldParameters.neighborhoodRadiusMax;
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
                    StaticWorldParameters.neighborhoodRadiusMax = value;
                    NotifyPropertyChanged("NeighborhoodRadiusMax");
                }
            }
        }

        #region Commands
        private GeneralCommand generalCmnd;
        public GeneralCommand GeneralCmnd
        {
            get
            {
                if (generalCmnd == null)
                    generalCmnd = new GeneralCommand(this);
                return generalCmnd;
            }
        }

        public void CreateMutation()
        {
            controlClient.CreateMutationSwarm();
        }

        public void CreateStable()
        {
            controlClient.CreateStableSwarm();
        }

        #endregion

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
