using System;
using Windows.UI.Xaml.Data;

namespace WeatherDataAnalysis.Converter
{
    internal class TemperatureConverter : IValueConverter
    {
        #region Methods

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var temperatureValue = (int?) value;

            string output;

            if (temperatureValue == null)
            {
                output = string.Empty;
            }
            else
            {
                output = temperatureValue.ToString();
            }

            return output;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var stringValue = (string) value;
            int output;
            var wasParsed = int.TryParse(stringValue, out var result);

            if (stringValue.Equals("-") || stringValue == string.Empty || !wasParsed)
            {
                output = 0;
            }
            else
            {
                output = result;
            }

            return output;
        }

        #endregion
    }
}