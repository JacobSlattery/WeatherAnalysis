using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WeatherDataAnalysisUnitTesting.WeatherDataCollection
{
    [TestClass]
    public class CountDaysWithTemperatureOverDegreeInYearInclusive
    {
        #region Methods

        [TestMethod]
        public void EmptyWeatherDataCollection()
        {
            var weatherDataCollection = new WeatherDataAnalysis.Model.WeatherDataCollection();
            Assert.AreEqual(0, weatherDataCollection.CountDaysWithTemperatureOverDegreeInYearInclusive(80, 2017));
        }

        [TestMethod]
        public void TestOneDayUsingCorrectYear()
        {
            var weatherDataCollection = new WeatherDataAnalysis.Model.WeatherDataCollection
                {new WeatherDataAnalysis.Model.WeatherData(DateTime.Now, 100, 1, 1)};
            Assert.AreEqual(1, weatherDataCollection.CountDaysWithTemperatureOverDegreeInYearInclusive(80, 2018));
        }

        [TestMethod]
        public void TestOneDayUsingWrongYear()
        {
            var weatherDataCollection = new WeatherDataAnalysis.Model.WeatherDataCollection
                {new WeatherDataAnalysis.Model.WeatherData(DateTime.Now, 100, 1, 1)};
            Assert.AreEqual(0, weatherDataCollection.CountDaysWithTemperatureOverDegreeInYearInclusive(80, 2017));
        }
        [TestMethod]
        public void TestOneDayAtGivenTemp()
        {
            var weatherDataCollection = new WeatherDataAnalysis.Model.WeatherDataCollection
                {new WeatherDataAnalysis.Model.WeatherData(DateTime.Now, 100, 20, 1)};
            Assert.AreEqual(1, weatherDataCollection.CountDaysWithTemperatureUnderDegreeInYearInclusive(100, 2018));
        }

        [TestMethod]
        public void TestOneDayUsingHighLessThanMinimum()
        {
            var weatherDataCollection = new WeatherDataAnalysis.Model.WeatherDataCollection
                {new WeatherDataAnalysis.Model.WeatherData(DateTime.Now, 70, 1, 1)};
            Assert.AreEqual(0, weatherDataCollection.CountDaysWithTemperatureOverDegreeInYearInclusive(80, 2017));
        }

        [TestMethod]
        public void TestThreeDayUsingSameYear()
        {
            var weatherDataCollection = new WeatherDataAnalysis.Model.WeatherDataCollection {
                new WeatherDataAnalysis.Model.WeatherData(DateTime.Now, 100, 1, 1),
                new WeatherDataAnalysis.Model.WeatherData(DateTime.Now.AddDays(1), 100, 1, 1),
                new WeatherDataAnalysis.Model.WeatherData(DateTime.Now.AddDays(2), 100, 1, 1)
            };
            Assert.AreEqual(3, weatherDataCollection.CountDaysWithTemperatureOverDegreeInYearInclusive(80, 2018));
        }
        [TestMethod]
        public void TestThreeDayUsingDifferentYear()
        {
            var weatherDataCollection = new WeatherDataAnalysis.Model.WeatherDataCollection {
                new WeatherDataAnalysis.Model.WeatherData(DateTime.Now, 100, 1, 1),
                new WeatherDataAnalysis.Model.WeatherData(DateTime.Now.AddYears(1), 100, 1, 1),
                new WeatherDataAnalysis.Model.WeatherData(DateTime.Now.AddYears(2), 100, 1, 1)
            };
            Assert.AreEqual(1, weatherDataCollection.CountDaysWithTemperatureOverDegreeInYearInclusive(80, 2018));
        }

        #endregion
    }
}