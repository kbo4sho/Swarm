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
using XNASwarmsXAML.W8.Authoring.Controls.Util;


namespace XNASwarmsXAML.W8.Authoring.Controls
{
    public sealed partial class Knob : UserControl, INotifyPropertyChanged
    {
        public Knob()
        {
            this.InitializeComponent();
            transform.DataContext = this;
        }

        private void Grid_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            this.Angle = MathHelper.GetAngle(e.Position, this.RenderSize);
            this.Amount = (int)(this.Angle / 360 * 100);
        }

        int m_Amount = default(int);
        public int Amount { get { return m_Amount; } set { SetProperty(ref m_Amount, value); } }

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
       
    }
}

   

   