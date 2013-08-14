using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using XNASwarmsXAML.W8.Authoring.ViewModels;

namespace XNASwarmsXAML.W8.Authoring.Commands
{
    public class GameCommands : ICommand
    {
        private GameControlViewModel gameControlViewModel;
        public GameCommands(GameControlViewModel gameControlViewModel)
        {
            this.gameControlViewModel = gameControlViewModel;
        }

        public bool CanExecute(object parameters)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            if (parameter.Equals(App.Current.Resources["PlayMusicValue"]))
            {
                this.gameControlViewModel.PlayMusic();
            }

            if (parameter.Equals(App.Current.Resources["PauseMusicValue"]))
            {
                this.gameControlViewModel.PauseMusic();
            }
        }
    }
}
