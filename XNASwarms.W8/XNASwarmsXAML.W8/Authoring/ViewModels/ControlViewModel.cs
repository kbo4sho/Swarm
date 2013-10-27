using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNASwarms;
using XNASwarms.Util;

namespace XNASwarmsXAML.W8.Authoring.ViewModels
{
    public abstract class ControlViewModel
    {
        private static readonly string ImageBasePath = "Assets/Icons/";
        public string Name { get; private set; }
        public string IconPath { get; private set; }
        protected IControlClient controlClient;

        public ControlViewModel(string name, string iconPath, IControlClient controlclient)
        {
            Name = name;
            IconPath = ImageBasePath + iconPath;
            controlClient = controlclient;
        }
    }
}
