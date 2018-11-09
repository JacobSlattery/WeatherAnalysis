using System;
using System.Collections.Generic;
using System.Linq;

namespace WeatherDataAnalysis.View.Report
{
    /// <summary>
    ///     Used for creating histograms using a collection of <see cref="int" />
    /// </summary>
    internal class Histogram
    {

        #region Methods

        /// <summary>
        ///     Makes the histogram using a collection of integers.
        ///     Histogram surrounds negatives with parenthesis.
        /// </summary>
        /// <param name="numbers">The numbers.</param>
        /// <param name="bucketSize">The bucket size.</param>
        /// <returns>
        ///     A string of the complete histogram.
        /// </returns>
        public static string MakeHistogramFrom(IEnumerable<int> numbers, int bucketSize)
        {
            if (bucketSize < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(bucketSize), "Bucket size must be greater than 0");
            }

            var output = string.Empty;
            var numbersList = numbers.ToList();
            var high = numbersList.Max();
            var low = numbersList.Min();

            var histogramHighestValue = getHistogramHighestValue(bucketSize, high);
            var histogramLowestValue = getHistogramLowestValue(bucketSize, low);

            if (histogramLowestValue == histogramHighestValue)
            {
                histogramHighestValue++;
            }

            for (var min = histogramLowestValue; min < histogramHighestValue; min += bucketSize)
            {
                var max = min + bucketSize - 1;
                var rangeCount = numbersList.Count(number => number >= min && number <= max);
                var minString = min.ToString();
                var maxString = max.ToString();

                if (min < 0)
                {
                    minString = $"({min})";
                }

                if (max < 0)
                {
                    maxString = $"({max})";
                }

                output += $"{minString}-{maxString}: {rangeCount}" + Environment.NewLine;
            }

            return output;
        }

        private static int getHistogramHighestValue(int bucketSize, int high)
        {
            int histogramHighestValue;
            if (high >= 0)
            {
                histogramHighestValue = high - high % bucketSize + bucketSize - 1;
            }
            else
            {
                histogramHighestValue = high - high % bucketSize;
            }

            return histogramHighestValue;
        }

        private static int getHistogramLowestValue(int bucketSize, int low)
        {
            int histogramLowestValue;
            if (low >= 0)
            {
                histogramLowestValue = low - low % bucketSize;
            }
            else if (low < 0 && low % bucketSize == 0)
            {
                histogramLowestValue = low;
            }
            else
            {
                histogramLowestValue = low - (bucketSize + low % bucketSize);
            }

            return histogramLowestValue;
        }

        #endregion
    }
}