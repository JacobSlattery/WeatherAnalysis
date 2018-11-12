using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using WeatherDataAnalysis.Model;

namespace WeatherDataAnalysis.DataTier
{
    /// <summary>
    ///     Takes the data contained in a <see cref="IEnumerable{WeatherData}" /> and writes it to a file.
    /// </summary>
    internal class WeatherFileWriter
    {
        #region Methods

        /// <summary>
        ///     Parses the <see cref="IEnumerable{WeatherData}" /> contents to the file.
        /// </summary>
        /// <param name="weatherDataCollection">The weather data collection.</param>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        ///     file - Cannot accept null for a file
        ///     or
        ///     weatherDataCollection - Weather data collection cannot be null
        /// </exception>
        public async Task ParseWeatherDataCollectionToFile(ICollection<WeatherData> weatherDataCollection,
            StorageFile file)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file), "Cannot accept null for a file");
            }

            if (weatherDataCollection == null)
            {
                throw new ArgumentNullException(nameof(weatherDataCollection),
                    "Weather data collection cannot be null");
            }

            if (file.FileType.Equals(".csv"))
            {
                await this.writeToCsvFile(weatherDataCollection, file);
            }
            else if (file.FileType.Equals(".xml"))
            {
                WeatherCollectionXmlSerializer.WriteToXml(weatherDataCollection, file);
            }
        }

        private async Task writeToCsvFile(IEnumerable<WeatherData> weatherDataCollection, StorageFile file)
        {
            var fileContents = string.Empty;
            foreach (var weatherData in weatherDataCollection.OrderBy(weather => weather.Date))
            {
                fileContents += this.parseWeatherDataToFileFormat(weatherData);
            }

            await FileIO.WriteTextAsync(file, fileContents);
        }

        private string parseWeatherDataToFileFormat(WeatherData weatherData)
        {
            return
                $"{weatherData.Date.ToShortDateString()}, {weatherData.High}, {weatherData.Low}, {weatherData.Precipitation}{Environment.NewLine}";
        }

        #endregion
    }
}