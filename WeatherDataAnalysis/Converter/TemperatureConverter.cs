using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace WeatherDataAnalysis.Converter
{
    class TemperatureConverter : IValueConverter
    {
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
            int? output;

            if (stringValue.Equals("-") || stringValue.Length == 0)
            {
                output = null;
            }
            else
            {
                var wasParsed = int.TryParse(stringValue, out var result);
                if (wasParsed)
                {
                    output = result;
                }
                else
                {
                    output = null;
                }
            }


            return output;
        }
    }
}
