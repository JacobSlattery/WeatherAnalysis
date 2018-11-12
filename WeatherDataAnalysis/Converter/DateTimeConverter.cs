using System;
using Windows.UI.Xaml.Data;

namespace WeatherDataAnalysis.Converter
{
    public class DateTimeConverter : IValueConverter
    {
        #region Methods

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var dateTimeValue = (DateTime) value;
            return dateTimeValue.ToShortDateString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var stringValue = (string) value;
            return DateTime.Parse(stringValue);
        }

        #endregion
    }
}