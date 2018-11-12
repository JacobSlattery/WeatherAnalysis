using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Storage;
using WeatherDataAnalysis.Model;

namespace WeatherDataAnalysis.DataTier
{
    /// <summary>
    ///     Can read a csv or txt file containing weather data and converts them into a <see cref="WeatherDataCollection" />
    ///     Will report the lines in the file that are unreadable
    /// </summary>
    internal class WeatherFileReader
    {
        #region Data members

        private readonly char[] delimiterChars = {',', '|', ';', '\t'};

        #endregion

        #region Properties

        /// <summary>
        ///     Gets each row with unreadable data.
        /// </summary>
        /// <value>
        ///     The rows with unreadable data.
        /// </value>
        public ICollection<string> UnreadableLines { get; private set; }

        #endregion

        public WeatherFileReader()
        {
            this.UnreadableLines = new List<string>();
        }

        #region Methods

        /// <summary>
        ///     Reads the file and converts the data into a <see cref="WeatherDataCollection" />.
        /// </summary>
        /// <param name="weatherDataFile">The weather data file.</param>
        /// <returns>
        ///     A <see cref="WeatherDataCollection" /> representing the file's weather data
        /// </returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<WeatherDataCollection> ReadFileToWeatherCollection(StorageFile weatherDataFile)
        {
            if (weatherDataFile == null)
            {
                throw new ArgumentNullException(nameof(weatherDataFile), "Cannot parse a null file");
            }

            this.UnreadableLines = new Collection<string>();
            var rawWeatherData = await FileIO.ReadTextAsync(weatherDataFile);
            return this.parseIntoWeatherCollection(rawWeatherData);
        }

        private WeatherDataCollection parseIntoWeatherCollection(string rawWeatherData)
        {
            var weatherCollection = new WeatherDataCollection();
            var rows = rawWeatherData.Split(Environment.NewLine);

            var lineCount = 0;
            foreach (var currentRow in rows)
            {
                lineCount++;
                var cells = currentRow.Split(this.delimiterChars);
                try
                {
                    weatherCollection.Add(parseRawCsvLineIntoAWeatherData(cells));
                }
                catch
                {
                    this.UnreadableLines.Add($"Line {lineCount}: " + currentRow);
                }
            }

            return weatherCollection;
        }

        private static WeatherData parseRawCsvLineIntoAWeatherData(IReadOnlyList<string> row)
        {
            var date = DateTime.Parse(row[(int) Columns.Date]);
            var high = int.Parse(row[(int) Columns.High]);
            var low = int.Parse(row[(int) Columns.Low]);
            var precipitation = double.Parse(row[(int) Columns.Precipitation]);
            return new WeatherData(date, high, low, precipitation);
        }

        private enum Columns
        {
            Date,
            High,
            Low,
            Precipitation
        }

        #endregion
    }
}