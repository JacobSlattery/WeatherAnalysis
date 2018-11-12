using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WeatherDataAnalysisUnitTesting.WeatherDataCollection
{
    [TestClass]
    public class GetAverageLowTemperatureForYear
    {
        #region Methods

        [TestMethod]
        public void EmptyWeatherDataCollection()
        {
            var weatherDataCollection = new WeatherDataAnalysis.Model.WeatherDataCollection();
            Assert.ThrowsException<InvalidOperationException>(() =>
                weatherDataCollection.GetAverageLowTemperatureForYear(2018));
        }

        [TestMethod]
        public void TestOneDayUsingCorrectYear()
        {
            var weatherDataCollection = new WeatherDataAnalysis.Model.WeatherDataCollection
                {new WeatherDataAnalysis.Model.WeatherData(DateTime.Now, 100, 20, 1)};
            Assert.AreEqual(20, weatherDataCollection.GetAverageLowTemperatureForYear(2018));
        }

        [TestMethod]
        public void TestOneDayUsingWrongYear()
        {
            var weatherDataCollection = new WeatherDataAnalysis.Model.WeatherDataCollection
                {new WeatherDataAnalysis.Model.WeatherData(DateTime.Now, 100, 1, 1)};
            Assert.ThrowsException<InvalidOperationException>(() =>
                weatherDataCollection.GetAverageLowTemperatureForYear(2019));
        }

        [TestMethod]
        public void TestTwoDayUsingSameYear()
        {
            var weatherDataCollection = new WeatherDataAnalysis.Model.WeatherDataCollection {
                new WeatherDataAnalysis.Model.WeatherData(DateTime.Now, 1, 1, 1),
                new WeatherDataAnalysis.Model.WeatherData(DateTime.Now.AddDays(1), 3, 2, 1)
            };
            Assert.AreEqual(1.5, weatherDataCollection.GetAverageLowTemperatureForYear(2018));
        }

        [TestMethod]
        public void TestThreeDayUsingDifferentYears()
        {
            var weatherDataCollection = new WeatherDataAnalysis.Model.WeatherDataCollection {
                new WeatherDataAnalysis.Model.WeatherData(DateTime.Now, 100, 20, 1),
                new WeatherDataAnalysis.Model.WeatherData(DateTime.Now.AddYears(1), 1, 1, 1),
                new WeatherDataAnalysis.Model.WeatherData(DateTime.Now.AddYears(2), 1, 1, 1)
            };
            Assert.AreEqual(20, weatherDataCollection.GetAverageLowTemperatureForYear(2018));
        }

        #endregion
    }
}