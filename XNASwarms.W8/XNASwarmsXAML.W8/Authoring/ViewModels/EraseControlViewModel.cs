using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNASwarms;

namespace XNASwarmsXAML.W8.Authoring.ViewModels
{
    public class EraseControlViewModel : ControlViewModel, INotifyPropertyChanged
    {
        public EraseControlViewModel(IControlClient controlClient)
            : base("Erase", "X.png", controlClient)
        {
            EraseDiameter = controlClient.GetEraseDiameter();
        }

        private double eraseDiameterProperty;
        public double EraseDiameter
        {
            get
            {
                return eraseDiameterProperty;
            }
            set
            {
                if (value != eraseDiameterProperty)
                {
                    eraseDiameterProperty = value;
                    controlClient.ChangeEraseDiameter(value);
                    NotifyPropertyChanged("EraseDiameter");
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
