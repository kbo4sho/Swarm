﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNASwarms;
using XNASwarms.Util;
using XNASwarmsXAML.W8.Authoring.Commands;
using XNASwarmsXAML.W8.Authoring.ViewModels;

namespace XNASwarmsXAML.W8.Authoring.ViewModels
{
    public class GameControlViewModel : ControlViewModel, INotifyPropertyChanged
    {
        public GameControlViewModel(IControlClient controlClient)
            : base("Game", "Games.png", controlClient)
        {

        }

        private double directionProperty;
        public double Direction
        {
            get
            {
                return directionProperty;
            }
            set
            {
                if (value != directionProperty)
                {
                    directionProperty = value;
                    NotifyPropertyChanged("Direction");
                }
            }
        }

        private double aimProperty;
        public double Aim
        {
            get
            {
                return aimProperty;
            }
            set
            {
                if (value != aimProperty)
                {
                    aimProperty = value;
                    NotifyPropertyChanged("Aim");
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

        #region Commands
        private GameCommands gameCmnd;
        public GameCommands GameCmnd
        {
            get
            {
                if (gameCmnd == null)
                    gameCmnd = new GameCommands(this);
                return gameCmnd;
            }
        }

        public void PlayMusic()
        {
            controlClient.PlayMusic();
        }

        public void PauseMusic()
        {
            controlClient.PauseMusic();
        }
        #endregion

    }
}
