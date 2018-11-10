using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WeatherDataAnalysis.Model;

namespace WeatherDataAnalysis.View.Report
{
    /// <summary>
    ///     Used to take the existing data and organize it into a report
    /// </summary>
    internal class ReportBuilder
    {
        #region Data members

        private readonly WeatherDataCollection weatherCollection;

        private readonly string[] monthList;

        private int aboveDegreeThreshold;
        private int belowDegreeThreshold;
        private int bucketSize;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ReportBuilder" /> class.
        /// </summary>
        /// <param name="weatherCollection">The <see cref="WeatherDataCollection" /> data is retrieved from.</param>
        /// <exception cref="ArgumentNullException">WeatherCollection - Cannot make a report builder with a null weather collection</exception>
        public ReportBuilder(WeatherDataCollection weatherCollection)
        {
            this.weatherCollection = weatherCollection ?? throw new ArgumentNullException(nameof(weatherCollection),
                                         "Cannot make a report builder with a null weather collection");

            this.monthList = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Gives a formatted <see cref="string" /> of all the data for all weather data.
        /// </summary>
        /// <param name="aboveThreshold">The above threshold.</param>
        /// <param name="belowThreshold">The below threshold.</param>
        /// <param name="bucketRange">The bucket range.</param>
        /// <returns>
        ///     The report
        /// </returns>
        public string BuildFullReport(int aboveThreshold, int belowThreshold, int bucketRange)
        {
            var output = string.Empty;
            this.aboveDegreeThreshold = aboveThreshold;
            this.belowDegreeThreshold = belowThreshold;
            this.bucketSize = bucketRange;

            foreach (var year in this.weatherCollection.GetEachDistinctYear())
            {
                output += this.buildFullYearReport(year) + Environment.NewLine + Environment.NewLine;
            }

            output.TrimEnd();

            return output;
        }

        private string buildFullYearReport(int year)
        {
            var output = this.buildYearStats(year);
            for (var month = 1; month < 13; month++)
            {
                output += this.buildMonthDisplay(month, year) + Environment.NewLine + Environment.NewLine;
            }

            return output.TrimEnd();
        }

        private string buildYearStats(int year)
        {
            string output;
            if (this.weatherCollection.Count == 0)
            {
                output = $"Weather Data {year} is empty";
            }
            else
            {
                var highestWeatherData = this.weatherCollection.GetEveryWeatherDataWithHighestTemperatureInYear(year);
                var lowestWeatherData = this.weatherCollection.GetEveryWeatherDataWithLowestTemperatureInYear(year);
                var highestLows = this.weatherCollection.GetEveryWeatherDataWithMaxLowTemperatureInYear(year);
                var lowestHighs = this.weatherCollection.GetEveryWeatherDataWithLowestHighTemperatureInYear(year);

                output = "Weather Data " + year + Environment.NewLine;
                output += $"{this.formatHigh(highestWeatherData)}" + Environment.NewLine;
                output += $"{this.formatLow(lowestWeatherData)}" + Environment.NewLine;
                output += $"{this.formatLowestHigh(lowestHighs)}" + Environment.NewLine;
                output += $"{this.formatHighestLow(highestLows)}" + Environment.NewLine;
                output += $"Average high temp: {this.weatherCollection.GetAverageHighTemperatureForYear(year):F}" +
                          Environment.NewLine;
                output += $"Average low temp: {this.weatherCollection.GetAverageLowTemperatureForYear(year):F}" +
                          Environment.NewLine;
                output +=
                    $"Days with temp above {this.aboveDegreeThreshold} degrees: {this.weatherCollection.CountDaysWithTemperatureOverDegreeInYearInclusive(this.aboveDegreeThreshold, year)}" +
                    Environment.NewLine;
                output +=
                    $"Days with temp below {this.belowDegreeThreshold} degrees: {this.weatherCollection.CountDaysWithTemperatureUnderDegreeInYearInclusive(this.belowDegreeThreshold, year)}" +
                    Environment.NewLine;

                output += this.getHistogramsFor(year);
                output += Environment.NewLine;
            }

            return output;
        }

        private string getHistogramsFor(int year)
        {
            var output = string.Empty;
            var weatherDataCollection = new WeatherDataCollection(this.weatherCollection.GetWeatherDataForYear(year));
            output +=
                $"High Histogram: {Environment.NewLine}{Histogram.MakeHistogramFrom(weatherDataCollection.GetHighs(), this.bucketSize)}";
            output +=
                $"Low Histogram:  {Environment.NewLine}{Histogram.MakeHistogramFrom(weatherDataCollection.GetLows(), this.bucketSize)}";
            return output;
        }

        private string buildMonthDisplay(int month, int year)
        {
            var monthDataCount = this.weatherCollection.CountWeatherDataForMonth(month, year);
            var output = $"{this.monthList[month - 1]} {year} ({monthDataCount} days of data)";
            if (monthDataCount > 0)
            {
                var highestWeatherData =
                    this.weatherCollection.GetEveryWeatherDataWithHighestTemperatureForMonth(month, year);
                var lowestWeatherData =
                    this.weatherCollection.GetEveryWeatherDataWithLowestTemperatureForMonth(month, year);

                output = $"{this.monthList[month - 1]} {year} ({monthDataCount} days of data)" + Environment.NewLine;
                output += $"{this.formatHigh(highestWeatherData)}" + Environment.NewLine;
                output += $"{this.formatLow(lowestWeatherData)}" + Environment.NewLine;
                output += $"Average high: {this.weatherCollection.GetAverageHighTemperatureForMonth(month, year):F}" +
                          Environment.NewLine;
                output += $"Average low: {this.weatherCollection.GetAverageLowTemperatureForMonth(month, year):F}";
            }

            return output;
        }

        private string formatDatesIn(ICollection<WeatherData> weatherDataList)
        {
            var oxfordCommaZone = weatherDataList.Count - 2;
            var output = string.Empty;
            for (var index = 0; index < weatherDataList.Count; index++)
            {
                var currentData = weatherDataList.ElementAt(index);
                output +=
                    $"{this.monthList[currentData.Date.Month - 1]} {this.convertDayToContractionString(currentData.Date.Day)}";

                if (index < oxfordCommaZone)
                {
                    output += ", ";
                }

                if (index == oxfordCommaZone)
                {
                    output += " and ";
                }
            }

            return output + ".";
        }

        private string formatHighestLow(ICollection<WeatherData> everyHighestLow)
        {
            return
                $"Highest low temp: {everyHighestLow.ElementAt(0).Low} occured on {this.formatDatesIn(everyHighestLow)}";
        }

        private string formatLowestHigh(ICollection<WeatherData> everyLowestHigh)
        {
            return
                $"Lowest high temp: {everyLowestHigh.ElementAt(0).High} occured on {this.formatDatesIn(everyLowestHigh)}";
        }

        private string formatHigh(ICollection<WeatherData> everyHighestWeatherData)
        {
            return
                $"Highest temp: {everyHighestWeatherData.ElementAt(0).High} occurred on {this.formatDatesIn(everyHighestWeatherData)}";
        }

        private string formatLow(ICollection<WeatherData> everyLowestWeatherData)
        {
            return
                $"Lowest temp: {everyLowestWeatherData.ElementAt(0).Low} occurred on {this.formatDatesIn(everyLowestWeatherData)}";
        }

        private string convertDayToContractionString(int day)
        {
            if (day < 0 || day > 31)
            {
                throw new ArgumentOutOfRangeException(nameof(day));
            }

            string output;
            var modDay = day % 10;
            switch (modDay)
            {
                case 1 when day != 11:
                    output = day + "st";
                    break;
                case 2 when day != 12:
                    output = day + "nd";
                    break;
                case 3 when day != 13:
                    output = day + "rd";
                    break;
                default:
                    output = day + "th";
                    break;
            }

            return output;
        }

        #endregion
    }
}