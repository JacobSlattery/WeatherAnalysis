using System;
using System.IO;
using System.Xml.Serialization;
using Windows.Storage;
using WeatherDataAnalysis.Model;

namespace WeatherDataAnalysis.DataTier
{
    public class WeatherCollectionXmlSerializer
    {
        public static void WriteToXmlAsync(WeatherDataCollection weatherData, StorageFile file)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file), "Cannot accept null for a file");
            }

            if (weatherData == null)
            {
                throw new ArgumentNullException(nameof(weatherData),
                    "Weather data collection cannot be null");
            }
            var serializer = new XmlSerializer(typeof(WeatherDataCollection));
            var writer = new StreamWriter(file.Path);
            serializer.Serialize(writer, weatherData);

        }
    }
}
