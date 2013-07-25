using SwarmEngine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
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
                //TODO: Need a more solid dvisor than staticworld params
                //TODO: Normalize the values here so they dont go crazy
                return Windows.UI.Color.FromArgb(255,(byte)(StaticBrushParameters.CohesiveForceMax / StaticWorldParameters.CohesiveForceMax * 0.8),
                                                     (byte)(StaticBrushParameters.AligningForceMax / StaticWorldParameters.AligningForceMax * 0.8), 
                                                     (byte)(StaticBrushParameters.SeperatingForceMax / StaticWorldParameters.SeperatingForceMax * 0.8));
            }
            set
            {
                if (value != brushColorProperty)
                {
                    brushColorProperty = value;
                    CohesiveForce = value.R * StaticWorldParameters.CohesiveForceMax / 0.8;
                    AlligningForce = value.G * StaticWorldParameters.AligningForceMax/ 0.8;
                    SperatingForce = value.B * StaticWorldParameters.SeperatingForceMax / 0.8; 
                    StaticBrushParameters.Color = value;
                    NotifyPropertyChanged("BrushColor");
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
