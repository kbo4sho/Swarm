using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNASwarmsXAML.W8.Authoring.ViewModels
{
    public abstract class ControlViewModel
    {

        public string Name { get; private set; }
        public string IconPath { get; private set; }

        public ControlViewModel(string name, string iconPath)
        {
            Name = name;
            IconPath = iconPath;
        }
    }
}
