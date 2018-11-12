using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.Storage;
using WeatherDataAnalysis.Model;

namespace WeatherDataAnalysis.DataTier
{
    public class WeatherCollectionXmlDeserializer
    {
        #region Methods

        /// <summary>
        ///     XMLs to weather collection.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>The WeatherDataCollection from the xml file</returns>
        /// <exception cref="ArgumentNullException">file - Cannot accept null for a file</exception>
        public static async Task<ICollection<WeatherData>> XmlToWeatherCollection(StorageFile file)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file), "Cannot accept null for a file");
            }

            var reader = new XmlSerializer(typeof(Collection<WeatherData>));
            var xml = await FileIO.ReadTextAsync(file);
            var fileReader = new StringReader(xml);
            var weatherDataCollection = (Collection<WeatherData>) reader.Deserialize(fileReader);
            fileReader.Close();

            return weatherDataCollection;
        }

        #endregion
    }
}