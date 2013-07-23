using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace XNASwarmsXAML.W8.Authoring
{
    public class EditorControlTemplateSelector : DataTemplateSelector
    {
        //Declare templates here
        //One of these methods should work
        //http://diptimayapatra.wordpress.com/2012/07/24/data-template-selector-in-windows-8-metro-xaml-app/

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            return null;
        }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
 	         return base.SelectTemplateCore(item, container);
        }

    }
}
