using System;
using Windows.UI.Xaml.Data;

namespace WeatherDataAnalysis.Converter
{
    public class PrecipitationConverter : IValueConverter
    {
        #region Methods

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var precipitationValue = (double?) value;

            string output;

            if (precipitationValue == null || precipitationValue < 0)
            {
                output = string.Empty;
            }
            else
            {
                output = precipitationValue.ToString();
            }

            return output;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var stringValue = (string) value;
            double? output;
            var wasParsed = double.TryParse(stringValue, out var result);

            if (stringValue.StartsWith("-") || stringValue == string.Empty || !wasParsed)
            {
                output = null;
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