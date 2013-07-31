using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNASwarmsXAML.W8.Authoring.ViewModels;

namespace XNASwarmsXAML.W8.Authoring.ViewModels
{
    public class HandControlViewModel : ControlViewModel, INotifyPropertyChanged
    {
        public HandControlViewModel()
            : base("Hand", "Assets/Icons/Games.png")
        {

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
