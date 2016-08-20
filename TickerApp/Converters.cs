using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;
using Ticker.Model;

namespace TickerApp
{
    public class CellColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var priceChange = (PriceChange)value;

            if (priceChange == PriceChange.Increasing)
            {
                return Brushes.Green;
            }
            else if(priceChange == PriceChange.Decreasing)
            {
                return Brushes.Red;
            }

            return Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
