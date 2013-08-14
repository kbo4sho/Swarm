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
using XNASwarmsXAML.W8.Authoring.Controls.Util;


namespace XNASwarmsXAML.W8.Authoring.Controls
{
    public sealed partial class JoyStick : UserControl, INotifyPropertyChanged
    {
        public JoyStick()
        {
            this.InitializeComponent();
            transform.DataContext = this;
        }

        #region Properties

        int m_Amount = default(int);
        public int Amount { get { return m_Amount; } set { SetProperty(ref m_Amount, value); } }

        int m_StickCenter = default(int);
        public int StickCenter { get { return m_StickCenter; } set { SetProperty(ref m_StickCenter, value); } }

        int m_CenterX = default(int);
        public int CenterX { get { return m_CenterX; } set { SetProperty(ref m_CenterX, value); } }

        int m_CenterY = default(int);
        public int CenterY { get { return m_CenterY; } set { SetProperty(ref m_CenterY, value); } }

        int m_IndicatorWidth = default(int);
        public int IndicatorWidth { get { return m_IndicatorWidth; } set { SetProperty(ref m_IndicatorWidth, value); } }

        int m_IndicatorHeight = default(int);
        public int IndicatorHeight { get { return m_IndicatorHeight; } set { SetProperty(ref m_IndicatorHeight, value); } }

        public static DependencyProperty AngleProperty = DependencyProperty.Register("Angle", typeof(double), typeof(JoyStick), null);
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
        #endregion

        #region Methods
        private int GetStickCenteredValue()
        {
            int value = (int)((this.wrapper.ActualHeight * .5f) - IndicatorHeight * .5f);
            if (value > 0)
            {
                return value;
            }
            else
            {
                return 0;
            }
        }

        private int GetStickAtEdgeValue()
        {
            int value = ((int)((this.wrapper.ActualHeight * .5f) - IndicatorHeight * .5f) - 
                         (int)this.wrapper.ActualHeight / 3);
            if (value > 0)
            {
                return value;
            }
            else
            {
                return 0;
            }
        }
        #endregion

        #region Events
        private void Grid_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            this.StickCenter = GetStickCenteredValue();
        }

        private void Grid_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            this.Angle = MathHelper.GetAngle(e.Position, this.RenderSize);
            this.Amount = (int)(this.Angle / 360 * 100);

            if (!e.IsInertial)
            {
                this.StickCenter = GetStickAtEdgeValue();
            }
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        void SetProperty<T>(ref T storage, T value, [System.Runtime.CompilerServices.CallerMemberName]  String propertyName = null)
        {
            if (!object.Equals(storage, value))
            {
                storage = value;
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void wrapper_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            this.StickCenter = GetStickCenteredValue();
        }

        private void joyStick_Loaded(object sender, RoutedEventArgs e)
        {
            this.CenterX = (int)this.Width/2;
            this.CenterY = (int)this.Height/2;
            IndicatorWidth = (int)this.Width / 8;
            IndicatorHeight = (int)this.Height / 8;
            this.StickCenter = GetStickCenteredValue();
        }

    }
}
