using SwarmEngine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNASwarmsXAML.W8.Authoring.ViewModels
{
    //TODO: Set up a system for dynamically adding models to the collection,
    //switching the control panel to support each selection,
    //handle removing of models 
    public class AuthoringViewModel : INotifyPropertyChanged
    {
        public AuthoringViewModel()
        {
            controlViewModels = new List<ViewModels.ControlViewModel>() { new BrushControlViewModel(), new WorldControlViewModel() };
            ControlViewModel = controlViewModels[0];
        }

        //TODO: Add a collection of control view models

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
