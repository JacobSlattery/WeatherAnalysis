using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;
using Windows.Storage;
using WeatherDataAnalysis.Model;

namespace WeatherDataAnalysis.DataTier
{
    public class WeatherCollectionXmlSerializer
    {
        #region Methods

        /// <summary>
        ///     Writes to XML.
        /// </summary>
        /// <param name="weatherData">The weather data.</param>
        /// <param name="file">The file.</param>
        /// <exception cref="ArgumentNullException">
        ///     file - Cannot accept null for a file
        ///     or
        ///     weatherData - Weather data collection cannot be null
        /// </exception>
        public static async void WriteToXml(ICollection<WeatherData> weatherData, StorageFile file)
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
            var serializer = new XmlSerializer(typeof(Collection<WeatherData>), knownTypes);
            var writer = new StringWriter();
            serializer.Serialize(writer, weatherData);

            await FileIO.WriteTextAsync(file, writer.ToString());
        }

        #endregion
    }
}