using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ViewLayer.Converters
{
    //if any false, return false
    public class ANDGateMultiBoolConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] is bool)
                {
                    var value = (bool)values[i];
                    if (!value)
                        return false;
                }
                
            }

            return true;
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException("BooleanAndConverter is a OneWay converter.");
        }
    }
}
