using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using XNASwarmsXAML.W8.Authoring.ViewModels;

namespace XNASwarmsXAML.W8.Authoring.Commands
{
    public class GeneralCommand : ICommand
    {
        private WorldControlViewModel worldControlViewModel;
        public GeneralCommand(WorldControlViewModel worldControlViewModel)
        {
            this.worldControlViewModel = worldControlViewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            if (parameter.Equals(App.Current.Resources["StableValue"]))
            {
                this.worldControlViewModel.CreateStable();
            }

            if (parameter.Equals(App.Current.Resources["MutationValue"]))
            {
                this.worldControlViewModel.CreateMutation();
            }
        }
    }

    public class MutateCommand : ICommand
    {
        private WorldControlViewModel worldControlViewModel;
        public MutateCommand(WorldControlViewModel worldControlViewModel)
        {
            this.worldControlViewModel = worldControlViewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            this.worldControlViewModel.CreateStable();
        }
    }
}
