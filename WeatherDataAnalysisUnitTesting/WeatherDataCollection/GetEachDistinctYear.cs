using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WeatherDataAnalysisUnitTesting.WeatherDataCollection
{
    /// <summary>
    ///     Input                           Expected Output
    ///     Count == 0                      true
    ///     10                              true
    ///     10 & Count == 1                 true
    ///     10, 11, 100 & Count == 3        true
    ///     5, 0, 9                         false
    /// </summary>
    [TestClass]
    public class GetEachDistinctYear
    {
        #region Methods

        [TestMethod]
        public void NoWeatherData()
        {
            var weatherDataCollection = new WeatherDataAnalysis.Model.WeatherDataCollection();
            var years = weatherDataCollection.GetEachDistinctYear();
            Assert.AreEqual(0, years.Count());
        }

        [TestMethod]
        public void OneWeatherData()
        {
            var weatherDataCollection = new WeatherDataAnalysis.Model.WeatherDataCollection {
                new WeatherDataAnalysis.Model.WeatherData(new DateTime(10, 10, 10), 0, 0, 0.0)
            };
            var years = weatherDataCollection.GetEachDistinctYear();
            Assert.IsTrue(years.Contains(10));
        }

        [TestMethod]
        public void MultipleWeatherDataOneYear()
        {
            var weatherDataCollection = new WeatherDataAnalysis.Model.WeatherDataCollection {
                new WeatherDataAnalysis.Model.WeatherData(new DateTime(10, 1, 1), 0, 0, 0.0),
                new WeatherDataAnalysis.Model.WeatherData(new DateTime(10, 1, 1), 1, 1, 0.0),
                new WeatherDataAnalysis.Model.WeatherData(new DateTime(10, 2, 2), 2, 2, 0.0)
            };
            var years = weatherDataCollection.GetEachDistinctYear();
            var enumerable = years.ToList();
            Assert.IsTrue(enumerable.Contains(10));
            Assert.AreEqual(1, enumerable.Count);
        }

        [TestMethod]
        public void MultipleYears()
        {
            var weatherDataCollection = new WeatherDataAnalysis.Model.WeatherDataCollection {
                new WeatherDataAnalysis.Model.WeatherData(new DateTime(10, 1, 1), 0, 0, 0.0),
                new WeatherDataAnalysis.Model.WeatherData(new DateTime(11, 1, 1), 1, 1, 0.0),
                new WeatherDataAnalysis.Model.WeatherData(new DateTime(100, 2, 2), 2, 2, 0.0),
                new WeatherDataAnalysis.Model.WeatherData(new DateTime(100, 2, 2), 2, 1, 0.0)
            };
            var years = weatherDataCollection.GetEachDistinctYear();
            var enumerable = years.ToList();
            Assert.IsTrue(enumerable.Contains(10));
            Assert.IsTrue(enumerable.Contains(11));
            Assert.IsTrue(enumerable.Contains(100));
            Assert.AreEqual(3, enumerable.Count);
        }

        [TestMethod]
        public void DoesNotContainSearchedYear()
        {
            var weatherDataCollection = new WeatherDataAnalysis.Model.WeatherDataCollection {
                new WeatherDataAnalysis.Model.WeatherData(new DateTime(10, 1, 1), 0, 0, 0.0),
                new WeatherDataAnalysis.Model.WeatherData(new DateTime(11, 1, 1), 1, 1, 0.0),
                new WeatherDataAnalysis.Model.WeatherData(new DateTime(100, 2, 2), 2, 2, 0.0),
                new WeatherDataAnalysis.Model.WeatherData(new DateTime(100, 2, 2), 2, 1, 0.0)
            };
            var years = weatherDataCollection.GetEachDistinctYear();
            var enumerable = years.ToList();
            Assert.IsFalse(enumerable.Contains(5));
            Assert.IsFalse(enumerable.Contains(0));
            Assert.IsFalse(enumerable.Contains(9));
        }

        #endregion
    }
}