using System;
using System.Globalization;
using Xamarin.Forms;

namespace Thunder.Helpers.Converters
{
    public class RewardsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return "Order Rewards";
            }
            var orderBy = (bool)value;
            return orderBy == true ? "Order rewards A-Z" : "Order rewards Z-A";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
