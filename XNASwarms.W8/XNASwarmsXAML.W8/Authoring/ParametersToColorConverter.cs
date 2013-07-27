using SwarmEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Data;
using XNASwarms.Emitters;

namespace XNASwarmsXAML.W8.Authoring
{
    public class ParametersToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return Color.FromArgb((byte)255, (byte)(StaticBrushParameters.CohesiveForceMax * 0.8), 
                                             (byte)(StaticBrushParameters.AligningForceMax * 0.8), 
                                             (byte)(StaticBrushParameters.SeperatingForceMax * 0.8));  
 
            //return new Windows.UI.Color((float)(c1 / StaticWorldParameters.CohesiveForceMax * 0.8),
            //        (float)(c2 / StaticWorldParameters.AligningForceMax * 0.8), (float)(c3 / StaticWorldParameters.SeperatingForceMax * 0.8));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return Colors.Blue;
        }
    }
}
