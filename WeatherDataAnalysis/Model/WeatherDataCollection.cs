using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;

namespace WeatherDataAnalysis.Model
{
    /// <inheritdoc />
    /// <summary>
    ///     A collection of <see cref="T:WeatherDataAnalysis.Model.WeatherData" /> that can be manipulated to find days of
    ///     significance.
    /// </summary>
    [DataContract]
    public class WeatherDataCollection : ICollection<WeatherData>
    {
        #region Data members

        [DataMember] public readonly ICollection<WeatherData> WeatherCollection;

        #endregion

        #region Properties

        /// <inheritdoc />
        /// <summary>
        ///     Gets the number of elements contained in the <see cref="T:System.Collections.IList"></see>.
        /// </summary>
        public int Count => this.WeatherCollection.Count;

        /// <inheritdoc />
        /// <summary>
        ///     Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.
        /// </summary>
        public bool IsReadOnly => this.WeatherCollection.IsReadOnly;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="WeatherDataCollection" /> class.
        /// </summary>
        public WeatherDataCollection()
        {
            this.WeatherCollection = new Collection<WeatherData>();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="WeatherDataCollection" /> class using a collection of
        ///     <see cref="WeatherData" />.
        /// </summary>
        /// <param name="weatherCollection">The weather collection.</param>
        /// <exception cref="ArgumentNullException">WeatherCollection - Cannot make a weather data collection from null</exception>
        public WeatherDataCollection(ICollection<WeatherData> weatherCollection)
        {
            this.WeatherCollection = weatherCollection ?? throw new ArgumentNullException(nameof(weatherCollection),
                                         "Cannot make a weather data collection from null");
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        /// <summary>
        ///     Adds the <see cref="T:WeatherDataAnalysis.Model.WeatherData" /> object to the collection.
        /// </summary>
        /// <param name="weatherData">The <see cref="T:WeatherDataAnalysis.Model.WeatherData" /> to add.</param>
        /// <exception cref="T:System.ArgumentNullException">weatherData - Cannot add a null object to the collection</exception>
        public void Add(WeatherData weatherData)
        {
            if (weatherData == null)
            {
                throw new ArgumentNullException(nameof(weatherData), "Cannot add a null object to the collection");
            }

            this.WeatherCollection.Add(weatherData);
        }

        /// <inheritdoc />
        /// <summary>
        ///     Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        public void Clear()
        {
            this.WeatherCollection.Clear();
        }

        /// <inheritdoc />
        /// <summary>
        ///     Determines whether [contains] [the specified weather data].
        /// </summary>
        /// <param name="weatherData">The weather data.</param>
        /// <returns>
        ///     <c>true</c> if [contains] [the specified weather data]; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">weatherData - Cannot add a null object to the collection</exception>
        public bool Contains(WeatherData weatherData)
        {
            if (weatherData == null)
            {
                throw new ArgumentNullException(nameof(weatherData), "Cannot add a null object to the collection");
            }

            var value = false;
            foreach (var currentWeather in this.WeatherCollection)
            {
                if (currentWeather.Date == weatherData.Date)
                {
                    value = true;
                }
            }

            return value;
        }

        /// <inheritdoc />
        /// <summary>
        ///     Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"></see> to an
        ///     <see cref="T:System.Array"></see>, starting at a particular <see cref="T:System.Array"></see> index.
        /// </summary>
        /// <param name="array">
        ///     The one-dimensional <see cref="T:System.Array"></see> that is the destination of the elements
        ///     copied from <see cref="T:System.Collections.Generic.ICollection`1"></see>. The <see cref="T:System.Array"></see>
        ///     must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(WeatherData[] array, int arrayIndex)
        {
            this.WeatherCollection.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc />
        /// <summary>
        ///     Removes the specified weather data.
        /// </summary>
        /// <param name="weatherData">The weather data.</param>
        /// <returns></returns>
        public bool Remove(WeatherData weatherData)
        {
            return this.WeatherCollection.Remove(weatherData);
        }

        /// <inheritdoc />
        /// <summary>
        ///     Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        ///     An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<WeatherData> GetEnumerator()
        {
            return this.WeatherCollection.GetEnumerator();
        }

        /// <inheritdoc />
        /// <summary>
        ///     Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        ///     An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) this.WeatherCollection).GetEnumerator();
        }

        /// <summary>
        ///     Gets the highs in the collection.
        /// </summary>
        /// <returns>
        ///     The highs
        /// </returns>
        public IEnumerable<int> GetHighs()
        {
            return this.WeatherCollection.Select(weather => weather.High);
        }

        /// <summary>
        ///     Gets the lows in the collection.
        /// </summary>
        /// <returns>
        ///     The lows
        /// </returns>
        public IEnumerable<int> GetLows()
        {
            return this.WeatherCollection.Select(weather => weather.Low);
        }

        /// <summary>
        ///     Gets if there is a weather in the collection with the given date.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public bool ContainsWeatherDataWith(DateTime date)
        {
            return this.WeatherCollection.Select(weather => weather.Date).Contains(date);
        }

        /// <summary>
        ///     Gets the weather at a given date.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>The weather at that date</returns>
        public WeatherData GetWeatherAtDate(DateTime date)
        {
            return this.WeatherCollection.Single(weather => weather.Date == date);
        }

        /// <summary>
        ///     Gets each distinct year in collection.
        /// </summary>
        /// <returns>
        ///     The years
        /// </returns>
        public IEnumerable<int> GetEachDistinctYear()
        {
            return this.WeatherCollection.Select(weather => weather.Date.Year).Distinct();
        }

        /// <summary>
        ///     Gets every <see cref="WeatherData" /> with lowest high temperature in year.
        /// </summary>
        /// <param name="year">The year searched.</param>
        /// <returns>
        ///     A <see cref="ICollection{WeatherData}" /> with each having the lowest high temperature in year.
        /// </returns>
        public ICollection<WeatherData> GetEveryWeatherDataWithLowestHighTemperatureInYear(int year)
        {
            var yearData = this.GetWeatherDataForYear(year);
            var lowestHigh = getMinHighTemperatureIn(yearData);
            var lowestHighWeathers = yearData.Where(weather => weather.High == lowestHigh).ToList();

            return lowestHighWeathers;
        }

        /// <summary>
        ///     Gets every <see cref="WeatherData" /> with highest low temperature in year.
        /// </summary>
        /// <param name="year">The year searched.</param>
        /// <returns>
        ///     A <see cref="ICollection{WeatherData}" /> with each having the highest low temperature in year.
        /// </returns>
        public ICollection<WeatherData> GetEveryWeatherDataWithMaxLowTemperatureInYear(int year)
        {
            var yearWeathers = this.GetWeatherDataForYear(year);
            var highestLow = getMaxLowTemperatureIn(yearWeathers);
            var highestLowWeathers = yearWeathers.Where(weather => weather.Low == highestLow).ToList();

            return highestLowWeathers;
        }

        /// <summary>
        ///     Counts the number of days with temperature equal to or over a given degree in the year.
        /// </summary>
        /// <param name="degree">The degree threshold.</param>
        /// <param name="year">The year searched.</param>
        /// <returns>
        ///     A <see cref="int" /> representing number of days with temperature equal to or over the given degree.
        /// </returns>
        public int CountDaysWithTemperatureOverDegreeInYearInclusive(int degree, int year)
        {
            var yearWeathers = this.GetWeatherDataForYear(year);
            return yearWeathers.Count(weather => weather.High >= degree);
        }

        /// <summary>
        ///     Gets the number of days with temperature equal to or under a given degree in the year.
        /// </summary>
        /// <param name="degree">The degree threshold.</param>
        /// <param name="year">The year searched.</param>
        /// <returns>
        ///     A <see cref="int" /> representing number of days with temperature equal to or under the given degree.
        /// </returns>
        public int CountDaysWithTemperatureUnderDegreeInYearInclusive(int degree, int year)
        {
            var yearWeathers = this.GetWeatherDataForYear(year);
            return yearWeathers.Count(weather => weather.Low <= degree);
        }

        /// <summary>
        ///     Gets every <see cref="WeatherData" /> with highest temperature for year.
        /// </summary>
        /// <param name="year">The year searched.</param>
        /// <returns>
        ///     A <see cref="ICollection{WeatherData}" /> with the each having the highest temperature for the year.
        /// </returns>
        public ICollection<WeatherData> GetEveryWeatherDataWithHighestTemperatureInYear(int year)
        {
            var yearData = this.GetWeatherDataForYear(year);
            var highestTemp = getMaxTemperatureIn(yearData);
            var weatherList = yearData.Where(weather => weather.High == highestTemp).ToList();

            return weatherList;
        }

        /// <summary>
        ///     Gets every <see cref="WeatherData" /> with lowest temperature for year.
        /// </summary>
        /// <param name="year">The year searched.</param>
        /// <returns>
        ///     A <see cref="ICollection{WeatherData}" /> with the each having the lowest temperature in the year.
        /// </returns>
        public ICollection<WeatherData> GetEveryWeatherDataWithLowestTemperatureInYear(int year)
        {
            var yearData = this.GetWeatherDataForYear(year);
            var lowestTemp = getMinTemperatureIn(yearData);
            var weatherDataList = yearData.Where(weather => weather.Low == lowestTemp).ToList();

            return weatherDataList;
        }

        /// <summary>
        ///     Gets the average high temperature for year.
        /// </summary>
        /// <param name="year">The year searched.</param>
        /// <returns>
        ///     A <see cref="double" /> of average high temperature for year.
        /// </returns>
        public double GetAverageHighTemperatureForYear(int year)
        {
            var yearData = this.GetWeatherDataForYear(year);
            var averageHigh = yearData.Average(weather => weather.High);

            return averageHigh;
        }

        /// <summary>
        ///     Gets the average low temperature for year.
        /// </summary>
        /// <param name="year">The year searched.</param>
        /// <returns>
        ///     A <see cref="double" /> of average low temperature for year.
        /// </returns>
        public double GetAverageLowTemperatureForYear(int year)
        {
            var yearData = this.GetWeatherDataForYear(year);
            var averageLow = yearData.Average(weather => weather.Low);

            return averageLow;
        }

        /// <summary>
        ///     Gets the weather data for the given year.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <returns>
        ///     An <see cref="ICollection{WeatherData}" />
        /// </returns>
        public ICollection<WeatherData> GetWeatherDataForYear(int year)
        {
            var yearData = this.WeatherCollection.Where(weather => weather.Date.Year == year).ToList();

            return yearData;
        }

        /// <summary>
        ///     Gets the average high temperature for month.
        /// </summary>
        /// <param name="month">The month searched.</param>
        /// <param name="year">The year of the month.</param>
        /// <returns>
        ///     A <see cref="double" /> of the average high temperature in the month.
        /// </returns>
        public double? GetAverageHighTemperatureForMonth(int month, int year) //TODO
        {
            var monthData = this.getWeatherDataForMonth(month, year);
            return getAverageHighTemperatureIn(monthData);
        }

        /// <summary>
        ///     Gets the average low temperature for month.
        /// </summary>
        /// <param name="month">The month searched.</param>
        /// <param name="year">The year of the month.</param>
        /// <returns>
        ///     A <see cref="double" /> of the average low temperature in the month.
        /// </returns>
        public double? GetAverageLowTemperatureForMonth(int month, int year)
        {
            var monthData = this.getWeatherDataForMonth(month, year);
            return getAverageLowTemperatureIn(monthData);
        }

        /// <summary>
        ///     Gets every <see cref="WeatherData" /> with highest temperature in month.
        /// </summary>
        /// <param name="month">The month.</param>
        /// <param name="year">The year of the month.</param>
        /// <returns>
        ///     A <see cref="ICollection{WeatherData}" /> all with the highest temp in the given month.
        /// </returns>
        public ICollection<WeatherData> GetEveryWeatherDataWithHighestTemperatureForMonth(int month, int year)
        {
            var monthData = this.getWeatherDataForMonth(month, year);
            return getEachWeatherWithMaxTemperatureIn(monthData);
        }

        /// <summary>
        ///     Gets every <see cref="WeatherData" /> with lowest temperature in month.
        /// </summary>
        /// <param name="month">The month searched.</param>
        /// <param name="year">The year of the month.</param>
        /// <returns>
        ///     A <see cref="ICollection{WeatherData}" /> all with the lowest temperature in the month.
        /// </returns>
        public ICollection<WeatherData> GetEveryWeatherDataWithLowestTemperatureForMonth(int month, int year)
        {
            var monthData = this.getWeatherDataForMonth(month, year);
            return getEachWeatherWithMinTemperatureIn(monthData);
        }

        /// <summary>
        ///     Gets the number of data points in the month.
        /// </summary>
        /// <param name="month">The month.</param>
        /// <param name="year">The year.</param>
        /// <returns>
        ///     The number of <see cref="WeatherData" /> in the month.
        /// </returns>
        public int CountWeatherDataForMonth(int month, int year)
        {
            var monthData =
                this.WeatherCollection.Count(weather => weather.Date.Year == year && weather.Date.Month == month);

            return monthData;
        }

        private ICollection<WeatherData> getWeatherDataForMonth(int month, int year)
        {
            var yearData = this.GetWeatherDataForYear(year);
            var monthList = yearData.Where(weather => weather.Date.Month == month).ToList();

            return monthList;
        }

        private static ICollection<WeatherData> getEachWeatherWithMinTemperatureIn(
            ICollection<WeatherData> weatherDataList)
        {
            var minTemp = getMinTemperatureIn(weatherDataList);
            var weathersWithMinTemp = weatherDataList.Where(weather => weather.Low == minTemp).ToList();

            return weathersWithMinTemp;
        }

        private static ICollection<WeatherData> getEachWeatherWithMaxTemperatureIn(
            ICollection<WeatherData> weatherDataList)
        {
            var maxTemp = getMaxTemperatureIn(weatherDataList);
            var weathersWithMaxTemp = weatherDataList.Where(weather => weather.High == maxTemp).ToList();

            return weathersWithMaxTemp;
        }

        private static double? getAverageLowTemperatureIn(IEnumerable<WeatherData> monthList)
        {
            double? value;
            try
            {
                value = monthList.Average(weather => weather.Low);
            }
            catch (InvalidOperationException)
            {
                value = null;
            }

            return value;
        }

        private static double? getAverageHighTemperatureIn(IEnumerable<WeatherData> monthList)
        {
            double? value;
            try
            {
                value = monthList.Average(weather => weather.High);
            }
            catch (InvalidOperationException)
            {
                value = null;
            }

            return value;
        }

        private static int? getMaxLowTemperatureIn(IEnumerable<WeatherData> weatherDataList)
        {
            int? value;
            try
            {
                value = weatherDataList.Max(weather => weather.Low);
            }
            catch (InvalidOperationException)
            {
                value = null;
            }

            return value;
        }

        private static int? getMinHighTemperatureIn(IEnumerable<WeatherData> weatherDataList)
        {
            int? value;
            try
            {
                value = weatherDataList.Min(weather => weather.High);
            }
            catch (InvalidOperationException)
            {
                value = null;
            }

            return value;
        }

        private static int? getMinTemperatureIn(IEnumerable<WeatherData> weatherDataList)
        {
            int? value;
            try
            {
                value = weatherDataList.Min(weather => weather.Low);
            }
            catch (InvalidOperationException)
            {
                value = null;
            }

            return value;
        }

        private static int? getMaxTemperatureIn(IEnumerable<WeatherData> weatherDataList)
        {
            int? value;
            try
            {
                value = weatherDataList.Max(weather => weather.High);
            }
            catch (InvalidOperationException)
            {
                value = null;
            }

            return value;
        }

        #endregion
    }
}