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

        }

        public int thing = 1;

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
                    NotifyPropertyChanged("SperatingForce");
                    NotifyPropertyChanged("BrushColor");
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
                    NotifyPropertyChanged("BrushColor");
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
                    NotifyPropertyChanged("BrushColor");
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

        private Color brushColorProperty = StaticBrushParameters.Color;
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

                    CohesiveForce = value.R * StaticWorldParameters.CohesiveForceMax / 255;
                    AlligningForce = value.G * StaticWorldParameters.AligningForceMax / 255;
                    SperatingForce = value.B * StaticWorldParameters.SeperatingForceMax / 255;
                    NotifyPropertyChanged("BrushColor");
                    
                }
            }
        }

        private bool isMobileProperty = StaticBrushParameters.IsMobile;
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
                    StaticBrushParameters.IsMobile = value;
                    NotifyPropertyChanged("IsMobile");
                }
            }
        }

        private double startingDirectionProperty = StaticBrushParameters.StartingDirection;
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
                    StaticBrushParameters.StartingDirection = value;
                    NotifyPropertyChanged("StartingDirection");
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
