using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WeatherDataAnalysisUnitTesting.WeatherDataCollection
{
    /// <summary>
    ///     Input                                   Expected Out
    ///     1 (empty list)                          []
    ///     1 ({1/1/1, 0, 0})                       [{1/1/1, 0, 0}]
    ///     1 ({1/1/1, 0, 0}, {2/3/1, 0, 0})        [{1/1/1, 0, 0}, {2/3/1, 0, 0}]
    ///     1 ({1/1/2, 1, 0}, {2/3/1, 0, 0})        [{1/1/2, 1, 0}]
    ///     2 ({1/1/2, 1, 0}, {2/3/1, 0, 0})        [{2/3/1, 0, 0}]
    ///     0 ({1/1/2, 1, 0}, {2/3/1, 0, 0})        []
    /// </summary>
    [TestClass]
    public class GetEveryWeatherDataWithLowestHighTemperatureInYear
    {
        #region Methods

        [TestMethod]
        public void EmptyCollection()
        {
            var weatherDataCollection = new WeatherDataAnalysis.Model.WeatherDataCollection();
            var returnedWeathers = weatherDataCollection.GetEveryWeatherDataWithLowestHighTemperatureInYear(1);
            Assert.AreEqual(0, returnedWeathers.Count);
        }

        [TestMethod]
        public void OneWeatherDataInCollection()
        {
            var onlyWeather = new WeatherDataAnalysis.Model.WeatherData(new DateTime(1, 1, 1), 0, 0, 0.0);
            var weatherDataCollection = new WeatherDataAnalysis.Model.WeatherDataCollection {
                onlyWeather
            };
            var returnedWeathers = weatherDataCollection.GetEveryWeatherDataWithLowestHighTemperatureInYear(1);
            Assert.IsTrue(returnedWeathers.Count == 1);
            Assert.IsTrue(returnedWeathers.Contains(onlyWeather));
        }

        [TestMethod]
        public void MultipleWeatherDataInCollectionSameYearSameTemp()
        {
            var weather1 = new WeatherDataAnalysis.Model.WeatherData(new DateTime(1, 1, 1), 0, 0, 0.0);
            var weather2 = new WeatherDataAnalysis.Model.WeatherData(new DateTime(1, 2, 3), 0, 0, 0.0);
            var weatherDataCollection = new WeatherDataAnalysis.Model.WeatherDataCollection {
                weather1,
                weather2
            };
            var returnedWeathers = weatherDataCollection.GetEveryWeatherDataWithLowestHighTemperatureInYear(1);
            Assert.AreEqual(2, returnedWeathers.Count);
            Assert.IsTrue(returnedWeathers.Contains(weather1));
            Assert.IsTrue(returnedWeathers.Contains(weather2));
        }

        [TestMethod]
        public void MultipleWeatherDataInCollectionDifferentYearDifferentTemp()
        {
            var weather1 = new WeatherDataAnalysis.Model.WeatherData(new DateTime(2, 1, 1), 1, 0, 0.0);
            var weather2 = new WeatherDataAnalysis.Model.WeatherData(new DateTime(1, 2, 3), 0, 0, 0.0);
            var weatherDataCollection = new WeatherDataAnalysis.Model.WeatherDataCollection {
                weather1,
                weather2
            };
            var returnedWeathers = weatherDataCollection.GetEveryWeatherDataWithLowestHighTemperatureInYear(1);
            Assert.AreEqual(1, returnedWeathers.Count);
            Assert.IsTrue(returnedWeathers.Contains(weather2));
        }

        [TestMethod]
        public void MultipleWeatherDataInCollectionDifferentYearDifferentTempDifferentNumber()
        {
            var weather1 = new WeatherDataAnalysis.Model.WeatherData(new DateTime(2, 1, 1), 1, 0, 0.0);
            var weather2 = new WeatherDataAnalysis.Model.WeatherData(new DateTime(1, 2, 3), 0, 0, 0.0);
            var weatherDataCollection = new WeatherDataAnalysis.Model.WeatherDataCollection {
                weather1,
                weather2
            };
            var returnedWeathers = weatherDataCollection.GetEveryWeatherDataWithLowestHighTemperatureInYear(2);
            Assert.AreEqual(1, returnedWeathers.Count);
            Assert.IsTrue(returnedWeathers.Contains(weather1));
        }

        [TestMethod]
        public void MultipleWeatherDataInCollectionDifferentYearDifferentTempNumberNotContained()
        {
            var weather1 = new WeatherDataAnalysis.Model.WeatherData(new DateTime(2, 1, 1), 1, 0, 0.0);
            var weather2 = new WeatherDataAnalysis.Model.WeatherData(new DateTime(1, 2, 3), 0, 0, 0.0);
            var weatherDataCollection = new WeatherDataAnalysis.Model.WeatherDataCollection {
                weather1,
                weather2
            };
            var returnedWeathers = weatherDataCollection.GetEveryWeatherDataWithLowestHighTemperatureInYear(0);
            Assert.AreEqual(0, returnedWeathers.Count);
        }

        #endregion
    }
}