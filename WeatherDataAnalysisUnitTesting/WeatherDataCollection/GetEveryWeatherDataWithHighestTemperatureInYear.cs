using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WeatherDataAnalysisUnitTesting.WeatherDataCollection
{
    /// <summary>
    ///     Input                                   Expected Out
    ///     1 (Empty)                               []
    ///     1 ({1/1/1, 0, 0})                       [{1/1/1, 0, 0}]
    ///     1 ({1/1/1, 0, 0}, {1/1/1, 0, 0})        [{1/1/1, 0, 0}, {1/1/1, 0, 0}]
    ///     1 ({1/1/1, 0, 0}, {2/2/2, 2, 2})        [{1/1/1, 0, 0}]
    ///     2 ({1/1/1, 0, 0}, {2/2/2, 2, 2})        [{2/2/2, 2, 2}]
    ///     0 ({1/1/1, 0, 0}, {2/2/2, 2, 2})        []
    /// </summary>
    [TestClass]
    public class GetEveryWeatherDataWithHighestTemperatureInYear
    {
        #region Methods

        [TestMethod]
        public void EmptyList()
        {
            var weatherDataCollection = new WeatherDataAnalysis.Model.WeatherDataCollection();
            var returnedValue = weatherDataCollection.GetEveryWeatherDataWithHighestTemperatureInYear(1);
            Assert.AreEqual(0, returnedValue.Count);
        }

        [TestMethod]
        public void SingleWeatherDateContained()
        {
            var weather1 = new WeatherDataAnalysis.Model.WeatherData(new DateTime(1, 1, 1), 0, 0);
            var weatherDataCollection = new WeatherDataAnalysis.Model.WeatherDataCollection {
                weather1
            };
            var returnedValues = weatherDataCollection.GetEveryWeatherDataWithHighestTemperatureInYear(1);
            Assert.IsTrue(returnedValues.Contains(weather1));
        }

        [TestMethod]
        public void DuplicateWeatherDateContained()
        {
            var weather1 = new WeatherDataAnalysis.Model.WeatherData(new DateTime(1, 1, 1), 0, 0);
            var weatherDataCollection = new WeatherDataAnalysis.Model.WeatherDataCollection {
                weather1,
                weather1
            };
            var returnedValues = weatherDataCollection.GetEveryWeatherDataWithHighestTemperatureInYear(1);
            Assert.AreEqual(2, returnedValues.Count);
            Assert.IsTrue(returnedValues.Contains(weather1));
        }

        [TestMethod]
        public void MultipleDistinctWeatherDateContained()
        {
            var weather1 = new WeatherDataAnalysis.Model.WeatherData(new DateTime(1, 1, 1), 0, 0);
            var weather2 = new WeatherDataAnalysis.Model.WeatherData(new DateTime(2, 2, 2), 2, 2);
            var weatherDataCollection = new WeatherDataAnalysis.Model.WeatherDataCollection {
                weather1,
                weather2
            };
            var returnedValues = weatherDataCollection.GetEveryWeatherDataWithHighestTemperatureInYear(1);
            Assert.AreEqual(1, returnedValues.Count);
            Assert.IsTrue(returnedValues.Contains(weather1));
            Assert.IsFalse(returnedValues.Contains(weather2));
        }

        [TestMethod]
        public void MultipleDistinctWeatherDateContainedDifferentSearch()
        {
            var weather1 = new WeatherDataAnalysis.Model.WeatherData(new DateTime(1, 1, 1), 0, 0);
            var weather2 = new WeatherDataAnalysis.Model.WeatherData(new DateTime(2, 2, 2), 2, 2);
            var weatherDataCollection = new WeatherDataAnalysis.Model.WeatherDataCollection {
                weather1,
                weather2
            };
            var returnedValues = weatherDataCollection.GetEveryWeatherDataWithHighestTemperatureInYear(2);
            Assert.AreEqual(1, returnedValues.Count);
            Assert.IsTrue(returnedValues.Contains(weather2));
            Assert.IsFalse(returnedValues.Contains(weather1));
        }

        [TestMethod]
        public void MultipleDistinctWeatherDateNotContained()
        {
            var weather1 = new WeatherDataAnalysis.Model.WeatherData(new DateTime(1, 1, 1), 0, 0);
            var weather2 = new WeatherDataAnalysis.Model.WeatherData(new DateTime(2, 2, 2), 2, 2);
            var weatherDataCollection = new WeatherDataAnalysis.Model.WeatherDataCollection {
                weather1,
                weather2
            };
            var returnedValues = weatherDataCollection.GetEveryWeatherDataWithHighestTemperatureInYear(0);
            Assert.AreEqual(0, returnedValues.Count);
        }

        #endregion
    }
}