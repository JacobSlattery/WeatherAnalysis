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
        #region Properties

        /// <summary>
        ///     Gets the histogram bucket size.
        /// </summary>
        /// <value>
        ///     The size of the histogram bucket.
        /// </value>
        public int BucketSize { get; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Histogram" /> class.
        /// </summary>
        /// <param name="bucketSize">Size of the histogram bucket.</param>
        /// <exception cref="ArgumentOutOfRangeException">bucketSize - Bucket size must be greater than 0</exception>
        public Histogram(int bucketSize)
        {
            if (bucketSize < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(bucketSize), "Bucket size must be greater than 0");
            }

            this.BucketSize = bucketSize;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Makes the histogram using a collection of integers.
        ///     Histogram surrounds negatives with parenthesis.
        /// </summary>
        /// <param name="numbers">The numbers.</param>
        /// <returns>
        ///     A string of the complete histogram.
        /// </returns>
        public string MakeHistogramFrom(ICollection<int> numbers)
        {
            var output = string.Empty;
            var high = numbers.Max();
            var low = numbers.Min();

            var histogramHighestValue = getHistogramHighestValue(this.BucketSize, high);
            var histogramLowestValue = getHistogramLowestValue(this.BucketSize, low);

            if (histogramLowestValue == histogramHighestValue)
            {
                histogramHighestValue++;
            }

            for (var min = histogramLowestValue; min < histogramHighestValue; min += this.BucketSize)
            {
                var max = min + this.BucketSize - 1;
                var rangeCount = numbers.Count(number => number >= min && number <= max);
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