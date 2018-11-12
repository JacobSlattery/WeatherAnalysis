using System;
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
        /// XMLs to weather collection.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>The WeatherDataCollection from the xml file</returns>
        /// <exception cref="ArgumentNullException">file - Cannot accept null for a file</exception>
        public static async Task<WeatherDataCollection> XmlToWeatherCollection(StorageFile file)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file), "Cannot accept null for a file");
            }

            var reader = new XmlSerializer(typeof(WeatherDataCollection));
            var xml = await FileIO.ReadTextAsync(file);
            var fileReader = new StringReader(xml);
            var weatherDataCollection = (WeatherDataCollection) reader.Deserialize(fileReader);
            fileReader.Close();

            return weatherDataCollection;
        }

        #endregion
    }
}