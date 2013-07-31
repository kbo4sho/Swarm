using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace XNASwarmsXAML.W8.Authoring.Controls
{
    public sealed partial class Knob : UserControl, INotifyPropertyChanged
    {
        public Knob()
        {
            this.InitializeComponent();
           // this.DataContext = this;
            transform.DataContext = this;
        }

        private void Grid_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            this.Angle = GetAngle(e.Position, this.RenderSize);
            this.Amount = (int)(this.Angle / 360 * 100);
        }

        int m_Amount = default(int);
        public int Amount { get { return m_Amount; } set { SetProperty(ref m_Amount, value); } }

        public double m_Angle = default(double);
        public static DependencyProperty AngleProperty = DependencyProperty.Register("Angle", typeof(double), typeof(Knob), null);
        public double Angle 
        { 
            get 
            { 
                return (double)GetValue(AngleProperty);
            } 
            set 
            {
                SetValue(AngleProperty, value);
                //SetProperty(ref m_Angle, value); 
                
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        void SetProperty<T>(ref T storage, T value, [System.Runtime.CompilerServices.CallerMemberName]  String propertyName = null)
        {
            if (!object.Equals(storage, value))
            {
                storage = value;
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public enum Quadrants : int { nw = 2, ne = 1, sw = 4, se = 3}
        private double GetAngle(Point touchPoint, Size circleSize)
        {
            var _X = touchPoint.X - (circleSize.Width /2d);
            var _Y = circleSize.Height - touchPoint.Y - (circleSize.Height / 2d);
            var _Hypot = Math.Sqrt(_X * _X + _Y * _Y);
            var _Value = Math.Asin(_Y / _Hypot) * 180 / Math.PI;
            var _Quadrant = (_X >= 0) ?
                (_Y >= 0) ? Quadrants.ne : Quadrants.se :
                (_Y >= 0) ? Quadrants.nw : Quadrants.sw;
            switch(_Quadrant)
            {
                case Quadrants.ne: _Value = 090 - _Value; break;
                case Quadrants.nw: _Value = 270 + _Value; break;
                case Quadrants.se: _Value = 090 - _Value; break;
                case Quadrants.sw: _Value = 270 + _Value; break;
            }
            return _Value;
        }

       
    }
}

   

   