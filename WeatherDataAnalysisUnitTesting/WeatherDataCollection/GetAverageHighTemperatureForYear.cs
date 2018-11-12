using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WeatherDataAnalysisUnitTesting.WeatherDataCollection
{
    [TestClass]
    public class GetAverageHighTemperatureForYear
    {
        [TestMethod]
        public void EmptyWeatherDataCollection()
        {
            var weatherDataCollection = new WeatherDataAnalysis.Model.WeatherDataCollection();
            Assert.ThrowsException<InvalidOperationException>(() => weatherDataCollection.GetAverageHighTemperatureForYear(2018));
        }

        [TestMethod]
        public void TestOneDayUsingCorrectYear()
        {
            var weatherDataCollection = new WeatherDataAnalysis.Model.WeatherDataCollection
                {new WeatherDataAnalysis.Model.WeatherData(DateTime.Now, 100, 1, 1)};
            Assert.AreEqual(100, weatherDataCollection.GetAverageHighTemperatureForYear(2018));
        }

        [TestMethod]
        public void TestOneDayUsingWrongYear()
        {
            var weatherDataCollection = new WeatherDataAnalysis.Model.WeatherDataCollection
                {new WeatherDataAnalysis.Model.WeatherData(DateTime.Now, 100, 1, 1)};
            Assert.ThrowsException<InvalidOperationException>(() => weatherDataCollection.GetAverageHighTemperatureForYear(2019));
        }
        
        [TestMethod]
        public void TestTwoDayUsingSameYear()
        {
            var weatherDataCollection = new WeatherDataAnalysis.Model.WeatherDataCollection {
                new WeatherDataAnalysis.Model.WeatherData(DateTime.Now, 1, 1, 1),
                new WeatherDataAnalysis.Model.WeatherData(DateTime.Now.AddDays(1), 2, 1, 1)
            };
            Assert.AreEqual(1.5, weatherDataCollection.GetAverageHighTemperatureForYear(2018));
        }
        [TestMethod]
        public void TestThreeDayUsingDifferentYears()
        {
            var weatherDataCollection = new WeatherDataAnalysis.Model.WeatherDataCollection {
                new WeatherDataAnalysis.Model.WeatherData(DateTime.Now, 100, 1, 1),
                new WeatherDataAnalysis.Model.WeatherData(DateTime.Now.AddYears(1), 1, 1, 1),
                new WeatherDataAnalysis.Model.WeatherData(DateTime.Now.AddYears(2), 1, 1, 1)
            };
            Assert.AreEqual(100, weatherDataCollection.GetAverageHighTemperatureForYear(2018));
        }
    }
}
