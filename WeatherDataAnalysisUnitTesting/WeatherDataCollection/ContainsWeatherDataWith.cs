using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WeatherDataAnalysisUnitTesting.WeatherDataCollection
{
    /// <summary>
    ///     Input                       Expected Out
    ///     1/1/1 (not contained)       false
    ///     1/1/1 (contained)           true
    ///     1/1/1 (contained)           true
    ///     1/1/1 (contained)           true
    ///     1/1/1 (not contained)       false
    /// </summary>
    [TestClass]
    public class ContainsWeatherDataWith
    {
        #region Methods

        [TestMethod]
        public void EmptyWeatherDataCollection()
        {
            var weatherDataCollection = new WeatherDataAnalysis.Model.WeatherDataCollection();
            Assert.IsFalse(weatherDataCollection.ContainsWeatherDataWith(new DateTime(1, 1, 1)));
        }

        [TestMethod]
        public void OneWeatherDataInCollectionSearchIsContained()
        {
            var date = new DateTime(1, 1, 1);
            var weatherDataCollection = new WeatherDataAnalysis.Model.WeatherDataCollection {
                new WeatherDataAnalysis.Model.WeatherData(date, 0, 0)
            };
            Assert.IsTrue(weatherDataCollection.ContainsWeatherDataWith(date));
        }

        [TestMethod]
        public void MultipleWeatherDataInCollectionSearchIsContained()
        {
            var date = new DateTime(1, 1, 1);
            var weatherDataCollection = new WeatherDataAnalysis.Model.WeatherDataCollection {
                new WeatherDataAnalysis.Model.WeatherData(date, 0, 0),
                new WeatherDataAnalysis.Model.WeatherData(new DateTime(1, 1, 2), 1, 0),
                new WeatherDataAnalysis.Model.WeatherData(new DateTime(1, 2, 1), 2, 0)
            };
            Assert.IsTrue(weatherDataCollection.ContainsWeatherDataWith(date));
        }

        [TestMethod]
        public void MultipleWeatherDataInCollectionSearchIsContainedTwice()
        {
            var date = new DateTime(1, 1, 1);
            var weatherDataCollection = new WeatherDataAnalysis.Model.WeatherDataCollection {
                new WeatherDataAnalysis.Model.WeatherData(date, 0, 0),
                new WeatherDataAnalysis.Model.WeatherData(date, 1, 0),
                new WeatherDataAnalysis.Model.WeatherData(new DateTime(1, 2, 1), 2, 0)
            };
            Assert.IsTrue(weatherDataCollection.ContainsWeatherDataWith(date));
        }

        [TestMethod]
        public void MultipleWeatherDataInCollectionSearchIsNotContained()
        {
            var date = new DateTime(1, 1, 1);
            var weatherDataCollection = new WeatherDataAnalysis.Model.WeatherDataCollection {
                new WeatherDataAnalysis.Model.WeatherData(new DateTime(2, 2, 1), 0, 0),
                new WeatherDataAnalysis.Model.WeatherData(new DateTime(1, 2, 1), 1, 0),
                new WeatherDataAnalysis.Model.WeatherData(new DateTime(1, 1, 2), 2, 0)
            };
            Assert.IsFalse(weatherDataCollection.ContainsWeatherDataWith(date));
        }

        #endregion
    }
}