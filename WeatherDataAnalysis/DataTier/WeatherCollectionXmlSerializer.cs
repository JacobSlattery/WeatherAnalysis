using System;
using System.IO;
using System.Xml.Serialization;
using Windows.Storage;
using WeatherDataAnalysis.Model;

namespace WeatherDataAnalysis.DataTier
{
    public class WeatherCollectionXmlSerializer
    {
        #region Methods

        public static async void WriteToXml(WeatherDataCollection weatherData, StorageFile file)
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

            var knownTypes = new[] {typeof(WeatherData)};
            var serializer = new XmlSerializer(typeof(WeatherDataCollection), knownTypes);
            var writer = new StringWriter();
            serializer.Serialize(writer, weatherData);

            await FileIO.WriteTextAsync(file, writer.ToString());
        }

        #endregion
    }
}