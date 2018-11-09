using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WeatherDataAnalysisUnitTesting.WeatherData
{
    /// <summary>
    ///     Input                   Expected Output
    ///     [DateTime, 0, 0]        Creates Instance
    ///     [DateTime, 0, 1]        ArgumentOutOfRangeException
    /// </summary>
    [TestClass]
    public class Constructor
    {
        #region Methods

        [TestMethod]
        public void ValidParams()
        {
            Assert.IsNotNull(new WeatherDataAnalysis.Model.WeatherData(new DateTime(), 0, 0, 0.0));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void FailIfLowIsGreaterThanHigh()
        {
            new WeatherDataAnalysis.Model.WeatherData(new DateTime(), 0, 1, 0.0);
        }

        [TestMethod]
        public void TestParamsAreSetCorrectly()
        {
            var date = new DateTime(1, 1, 1);
            const int high = 10005;
            const int low = -10023;
            const double precipitation = 0.0;
            var weatherData = new WeatherDataAnalysis.Model.WeatherData(date, high, low, precipitation);
            Assert.AreEqual(date, weatherData.Date);
            Assert.IsTrue(weatherData.High == high);
            Assert.IsTrue(weatherData.Low == low);
            Assert.IsTrue(weatherData.Precipitation.Equals(precipitation));
        }

        #endregion
    }
}