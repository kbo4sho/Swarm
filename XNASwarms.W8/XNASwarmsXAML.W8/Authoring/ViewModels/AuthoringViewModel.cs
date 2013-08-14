using SwarmEngine;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNASwarms;

namespace XNASwarmsXAML.W8.Authoring.ViewModels
{
    public class AuthoringViewModel : INotifyPropertyChanged
    {
        public AuthoringViewModel(IControlClient controlClient)
        {
            controlViewModels = new List<ViewModels.ControlViewModel>() { 
                new WorldControlViewModel(controlClient), 
                new BrushControlViewModel(controlClient),
                new EraseControlViewModel(controlClient),
                new HandControlViewModel(controlClient) 
            };
            ControlViewModel = controlViewModels[0];
        }

        private List<ControlViewModel> controlViewModels;
        public List<ControlViewModel> ControlViewModels
        {
            get
            {
                return controlViewModels;
            }
            set
            {
                if (value != controlViewModels)
                {
                    controlViewModels = value;
                    NotifyPropertyChanged("ControlViewModels");
                }
            }
        }

        private ControlViewModel controlViewModel;
        public ControlViewModel ControlViewModel
        {
            get
            {
                return controlViewModel;
            }
            set
            {
                if (value != controlViewModel)
                {
                    controlViewModel = value;
                    StaticEditModeParameters.SetType(controlViewModel.Name);
                    NotifyPropertyChanged("ControlViewModel");
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
