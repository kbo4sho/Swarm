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
            ChanceOfRandomSteering = controlClient.GetWorldRandomSteering();
            SperatingForce = controlClient.GetWorldSeperatingForce();
            AlligningForce = controlClient.GetWorldAligningForce();
            CohesiveForce = controlClient.GetWorldCohesiveForce();
            NormalSpeed = controlClient.GetWorldNormalSpeed();
            MaxSpeed = controlClient.GetWorldMaxSpeed();
            NumberOfIndividualsMax = controlClient.GetWorldNumberOfIndividuals();
            NeighborhoodRadiusMax = controlClient.GetWorldNeighborhoodRadius();
        }

        private double chanceOfRandomSteeringProperty;
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
                    controlClient.ChangeWorldRandomSteering(value);
                    NotifyPropertyChanged("ChanceOfRandomSteering");
                }
            }
        }

        private double seperatingForceProperty;
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
                    controlClient.ChangeWorldSeperatingForce(value);
                    NotifyPropertyChanged("SperateingForce");
                }
            }
        }

        private double alligningForceProperty;
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
                    controlClient.ChangeWorldAligningForce(value);
                    NotifyPropertyChanged("AlligningForce");
                }
            }
        }

        private double cohesiveForceProperty;
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
                    controlClient.ChangeWorldCohesiveForce(value);
                    NotifyPropertyChanged("CohesiveForce");
                }
            }
        }

        private int normalSpeedProperty;
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
                    controlClient.ChangeWorldNormalSpeed(value);
                    NotifyPropertyChanged("NormalSpeed");
                }
            }
        }

        private int maxSpeedProperty;
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
                    controlClient.ChangeWorldMaxSpeed(value);
                    NotifyPropertyChanged("MaxSpeed");
                }
            }
        }

        private int numberOfIndividualsMaxProperty;
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
                    controlClient.ChangeWorldNumberOfIndividuals(value);
                    NotifyPropertyChanged("NumberOfIndividualsMax");
                }
            }
        }

        private int neighborhoodMaxRadiusProperty;
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
                    controlClient.ChangeWorldNeighborhoodRadius(value);
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
