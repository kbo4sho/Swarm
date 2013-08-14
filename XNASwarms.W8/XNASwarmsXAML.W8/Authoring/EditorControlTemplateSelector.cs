using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using XNASwarmsXAML.W8.Authoring.ViewModels;

namespace XNASwarmsXAML.W8.Authoring
{
    public class EditorControlTemplateSelector : DataTemplateSelector
    {
        public DataTemplate WorldEditorTemplate { get; set; }
        public DataTemplate BrushEditorTemplate { get; set; }
        public DataTemplate EraseEditorTemplate { get; set; }
        public DataTemplate GameEditorTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (item != null)
            {
                if(item is WorldControlViewModel)
                {
                    return WorldEditorTemplate;
                }
                else if(item is BrushControlViewModel)
                {
                    return BrushEditorTemplate;
                }
                else if (item is EraseControlViewModel)
                {
                    return EraseEditorTemplate;
                }
                else if (item is GameControlViewModel)
                {
                    return GameEditorTemplate;
                }
            }

 	        return base.SelectTemplateCore(item, container);
        }

    }
}
