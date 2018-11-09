using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WeatherDataAnalysisUnitTesting.WeatherData
{
    /// <summary>
    ///     Input                                   Expected Output
    ///     [10/10/10, 0, 0] & [10/10/10, 0, 0]     0
    ///     [10/9/10, 0, 0] & [10/10/10, 0, 0]      negative
    ///     [9/10/10, 0, 0] & [10/10/10, 0, 0]      negative
    ///     [10/10/9, 0, 0] & [10/10/10, 0, 0]      negative
    ///     [10/11/10, 0, 0] & [10/10/10, 0, 0]     positive
    ///     [11/10/10, 0, 0] & [10/10/10, 0, 0]     positive
    ///     [10/10/11, 0, 0] & [10/10/10, 0, 0]     positive
    ///     [1/1/1, 1, 0] & [1/1/1, 0, 0]           positive
    ///     [1/1/1, 0, 0] & [1/1/1, 1, 0]           negative
    ///     [1/1/1, 0, 0] & [1/1/1, 0, -1]          positive
    ///     [1/1/1, 0, -1] & [1/1/1, 0, 0]          negative
    /// </summary>
    [TestClass]
    public class CompareTo
    {
        #region Methods

        [TestMethod]
        public void SameObjectsShouldReturnZero()
        {
            var firstWeatherData = new WeatherDataAnalysis.Model.WeatherData(new DateTime(), 0, 0, 0.0);
            var secondWeatherData = new WeatherDataAnalysis.Model.WeatherData(new DateTime(), 0, 0, 0.0);
            var actual = firstWeatherData.CompareTo(secondWeatherData);
            Assert.AreEqual(0, actual);
        }

        [TestMethod]
        public void LowerDayShouldReturnNegative()
        {
            var firstWeatherData = new WeatherDataAnalysis.Model.WeatherData(new DateTime(10, 10, 9), 0, 0, 0.0);
            var secondWeatherData = new WeatherDataAnalysis.Model.WeatherData(new DateTime(10, 10, 10), 0, 0, 0.0);
            Assert.IsTrue(firstWeatherData.CompareTo(secondWeatherData) < 0);
        }

        [TestMethod]
        public void LowerMonthShouldReturnNegative()
        {
            var firstWeatherData = new WeatherDataAnalysis.Model.WeatherData(new DateTime(10, 9, 10), 0, 0, 0.0);
            var secondWeatherData = new WeatherDataAnalysis.Model.WeatherData(new DateTime(10, 10, 10), 0, 0, 0.0);
            Assert.IsTrue(firstWeatherData.CompareTo(secondWeatherData) < 0);
        }

        [TestMethod]
        public void LowerYearShouldReturnNegative()
        {
            var firstWeatherData = new WeatherDataAnalysis.Model.WeatherData(new DateTime(9, 10, 10), 0, 0, 0.0);
            var secondWeatherData = new WeatherDataAnalysis.Model.WeatherData(new DateTime(10, 10, 10), 0, 0, 0.0);
            Assert.IsTrue(firstWeatherData.CompareTo(secondWeatherData) < 0);
        }

        [TestMethod]
        public void HigherDayShouldReturnPositive()
        {
            var firstWeatherData = new WeatherDataAnalysis.Model.WeatherData(new DateTime(10, 10, 11), 0, 0, 0.0);
            var secondWeatherData = new WeatherDataAnalysis.Model.WeatherData(new DateTime(10, 10, 10), 0, 0, 0.0);
            Assert.IsTrue(firstWeatherData.CompareTo(secondWeatherData) > 0);
        }

        [TestMethod]
        public void HigherMonthShouldReturnPositive()
        {
            var firstWeatherData = new WeatherDataAnalysis.Model.WeatherData(new DateTime(10, 11, 10), 0, 0, 0.0);
            var secondWeatherData = new WeatherDataAnalysis.Model.WeatherData(new DateTime(10, 10, 10), 0, 0, 0.0);
            Assert.IsTrue(firstWeatherData.CompareTo(secondWeatherData) > 0);
        }

        [TestMethod]
        public void HigherYearShouldReturnPositive()
        {
            var firstWeatherData = new WeatherDataAnalysis.Model.WeatherData(new DateTime(11, 10, 10), 0, 0, 0.0);
            var secondWeatherData = new WeatherDataAnalysis.Model.WeatherData(new DateTime(10, 10, 10), 0, 0, 0.0);
            Assert.IsTrue(firstWeatherData.CompareTo(secondWeatherData) > 0);
        }

        [TestMethod]
        public void HigherHighShouldReturnPositive()
        {
            var firstWeatherData = new WeatherDataAnalysis.Model.WeatherData(new DateTime(1, 1, 1), 1, 0, 0.0);
            var secondWeatherData = new WeatherDataAnalysis.Model.WeatherData(new DateTime(1, 1, 1), 0, 0, 0.0);
            Assert.IsTrue(firstWeatherData.CompareTo(secondWeatherData) > 0);
        }

        [TestMethod]
        public void LowerHighShouldReturnNegative()
        {
            var firstWeatherData = new WeatherDataAnalysis.Model.WeatherData(new DateTime(1, 1, 1), 0, 0, 0.0);
            var secondWeatherData = new WeatherDataAnalysis.Model.WeatherData(new DateTime(1, 1, 1), 1, 0, 0.0);
            Assert.IsTrue(firstWeatherData.CompareTo(secondWeatherData) < 0);
        }

        [TestMethod]
        public void HigherLowShouldReturnPositive()
        {
            var firstWeatherData = new WeatherDataAnalysis.Model.WeatherData(new DateTime(1, 1, 1), 0, 0, 0.0);
            var secondWeatherData = new WeatherDataAnalysis.Model.WeatherData(new DateTime(1, 1, 1), 0, -1, 0.0);
            Assert.IsTrue(firstWeatherData.CompareTo(secondWeatherData) > 0);
        }

        [TestMethod]
        public void LowerLowShouldReturnNegative()
        {
            var firstWeatherData = new WeatherDataAnalysis.Model.WeatherData(new DateTime(1, 1, 1), 0, -1, 0.0);
            var secondWeatherData = new WeatherDataAnalysis.Model.WeatherData(new DateTime(1, 1, 1), 0, 0, 0.0);
            Assert.IsTrue(firstWeatherData.CompareTo(secondWeatherData) < 0);
        }

        #endregion
    }
}