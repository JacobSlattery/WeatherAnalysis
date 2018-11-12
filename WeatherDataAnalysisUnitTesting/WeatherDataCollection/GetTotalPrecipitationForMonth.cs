using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WeatherDataAnalysisUnitTesting.WeatherDataCollection
{
    [TestClass]
    public class GetTotalPrecipitationForMonth
    {
        #region Methods

        [TestMethod]
        public void EmptyWeatherDataCollection()
        {
            var weatherDataCollection = new WeatherDataAnalysis.Model.WeatherDataCollection();
            Assert.AreEqual(0, weatherDataCollection.GetTotalPrecipitationForMonth(11, 2018));
        }

        [TestMethod]
        public void OneDayUsingCorrectYear()
        {
            var weatherDataCollection = new WeatherDataAnalysis.Model.WeatherDataCollection
                {new WeatherDataAnalysis.Model.WeatherData(DateTime.Now, 1, 1, .1)};
            Assert.AreEqual(.1, weatherDataCollection.GetTotalPrecipitationForMonth(11, 2018));
        }

        [TestMethod]
        public void OneDayUsingWrongYear()
        {
            var weatherDataCollection = new WeatherDataAnalysis.Model.WeatherDataCollection
                {new WeatherDataAnalysis.Model.WeatherData(DateTime.Now, 1, 1, .1)};
            Assert.AreEqual(0, weatherDataCollection.GetTotalPrecipitationForMonth(11, 2017));
        }

        [TestMethod]
        public void TestThreeDayUsingSameYear()
        {
            var weatherDataCollection = new WeatherDataAnalysis.Model.WeatherDataCollection {
                new WeatherDataAnalysis.Model.WeatherData(DateTime.Now, 100, 1, 1),
                new WeatherDataAnalysis.Model.WeatherData(DateTime.Now.AddDays(1), 100, 1, 1),
                new WeatherDataAnalysis.Model.WeatherData(DateTime.Now.AddDays(2), 100, 1, 1)
            };
            Assert.AreEqual(3, weatherDataCollection.GetTotalPrecipitationForMonth(11, 2018));
        }

        [TestMethod]
        public void TestThreeDayUsingDifferentYear()
        {
            var weatherDataCollection = new WeatherDataAnalysis.Model.WeatherDataCollection {
                new WeatherDataAnalysis.Model.WeatherData(DateTime.Now, 100, 1, 1),
                new WeatherDataAnalysis.Model.WeatherData(DateTime.Now.AddYears(1), 100, 1, 1),
                new WeatherDataAnalysis.Model.WeatherData(DateTime.Now.AddYears(2), 100, 1, 1)
            };
            Assert.AreEqual(1, weatherDataCollection.GetTotalPrecipitationForMonth(11, 2018));
        }

        #endregion
    }
}