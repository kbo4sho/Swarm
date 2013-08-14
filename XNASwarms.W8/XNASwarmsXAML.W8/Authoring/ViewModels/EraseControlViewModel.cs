using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNASwarms;

namespace XNASwarmsXAML.W8.Authoring.ViewModels
{
    public class RemoveControlViewModel : ControlViewModel, INotifyPropertyChanged
    {
        public RemoveControlViewModel(IControlClient controlClient)
            : base("Remove", "X.png", controlClient)
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
