using SwarmEngine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using XNASwarms;
using XNASwarms.Emitters;

namespace XNASwarmsXAML.W8.Authoring.ViewModels
{
    public class BrushControlViewModel : ControlViewModel, INotifyPropertyChanged
    {
        public BrushControlViewModel(IControlClient controlClient)
            : base("Brush", "Paint.png", controlClient)
        {
            ChanceOfRandomSteering = controlClient.GetBrushRandomSteering();
            SperatingForce = controlClient.GetBrushSeperatingForce();
            AlligningForce = controlClient.GetBrushAligningForce();
            CohesiveForce = controlClient.GetBrushCohesiveForce();
            NormalSpeed = controlClient.GetBrushNormalSpeed();
            MaxSpeed = controlClient.GetBrushMaxSpeed();
            NeighborhoodRadiusMax = controlClient.GetBrushNeighborhoodRadius();
            BrushColor = controlClient.GetBrushColor();
            IsMobile = controlClient.GetBrushIsMobile();
            StartingDirection = controlClient.GetBrushStartingDirection();
            IsUndo = controlClient.GetBrushIsUndo();
        }

        public int thing = 1;

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
                    controlClient.ChangeBrushRandomSteering(value);
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
                    controlClient.ChangeBrushSeperatingForce(value);
                    NotifyPropertyChanged("SperatingForce");
                    NotifyPropertyChanged("BrushColor");
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
                    controlClient.ChangeBrushAligningForce(value);
                    NotifyPropertyChanged("AlligningForce");
                    NotifyPropertyChanged("BrushColor");
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
                    controlClient.ChangeBrushCohesiveForce(value);
                    NotifyPropertyChanged("CohesiveForce");
                    NotifyPropertyChanged("BrushColor");
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
                    controlClient.ChangeBrushNormalSpeed(value);
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
                    controlClient.ChangeBrushMaxSpeed(value);
                    NotifyPropertyChanged("MaxSpeed");
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
                    controlClient.ChangeBrushNeighborhoodRadius(value);
                    NotifyPropertyChanged("NeighborhoodRadiusMax");
                }
            }
        }

        private Color brushColorProperty;
        public Color BrushColor
        {
            get
            {
                return Windows.UI.Color.FromArgb(255,(byte)((CohesiveForce / StaticWorldParameters.CohesiveForceMax) * 255),
                                                     (byte)((AlligningForce / StaticWorldParameters.AligningForceMax) * 255),
                                                     (byte)((SperatingForce / StaticWorldParameters.SeperatingForceMax) * 255));
            }
            set
            {
                if (value != brushColorProperty)
                {
                    brushColorProperty = value;
                    StaticBrushParameters.Color = value;
                    
                    SperatingForce = value.B * StaticWorldParameters.SeperatingForceMax / 255;
                    AlligningForce = value.G * StaticWorldParameters.AligningForceMax / 255;
                    CohesiveForce = value.R * StaticWorldParameters.CohesiveForceMax / 255;
                    NotifyPropertyChanged("BrushColor");
                    
                }
            }
        }

        private bool isMobileProperty;
        public bool IsMobile
        {
            get
            {
                return isMobileProperty; 
            }
            set
            {
                if (value != isMobileProperty)
                {
                    isMobileProperty = value;
                    controlClient.ChangeBrushIsMobile(value);
                    NotifyPropertyChanged("IsMobile");
                }
            }
        }

        private double startingDirectionProperty;
        public double StartingDirection
        {
            get
            {
                return startingDirectionProperty;
            }
            set
            {
                if (value != startingDirectionProperty)
                {
                    startingDirectionProperty = value;
                    controlClient.ChangeBrushStartingDirection(value);
                    NotifyPropertyChanged("StartingDirection");
                }
            }
        }

        private bool isUndoProperty;
        public bool IsUndo
        {
            get
            {
                return isUndoProperty;
            }
            set
            {
                if (value != isUndoProperty)
                {
                    isUndoProperty = value;
                    controlClient.ChangeBrushIsUndo(value);
                    NotifyPropertyChanged("IsUndo");
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
