using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Study_Step.Data.UIConverters
{
    public class MarginOffsetMesConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length >= 2 && values[0] is double width
                                   && values[1] is bool isOutside)
            {
                double offset = width - 10;

                return isOutside
                    ? new Thickness(offset, 0, 0, 0)
                    : new Thickness(0, 0, offset, 0);
            }
            return new Thickness(0);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
