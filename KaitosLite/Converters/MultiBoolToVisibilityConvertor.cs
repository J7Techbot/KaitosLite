using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ViewLayer.Converters
{
    public class MultiBoolToVisibilityConvertor : IMultiValueConverter
    {
        ANDGateMultiBoolConverter andGateConverter;

        public MultiBoolToVisibilityConvertor()
        {
            this.andGateConverter = new ANDGateMultiBoolConverter(); 
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var b = (bool)andGateConverter.Convert(values, targetType, parameter, culture);

            return b ? Visibility.Visible : Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
