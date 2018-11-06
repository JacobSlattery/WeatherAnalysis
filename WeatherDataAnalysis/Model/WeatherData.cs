using System;

namespace WeatherDataAnalysis.Model
{
    /// <inheritdoc />
    /// <summary>
    ///     Weather class which stores the date of the weather along with the day's high and low temperature
    /// </summary>
    public class WeatherData : IComparable<WeatherData>
    {
        #region Properties

        /// <summary>
        ///     Gets the weather date.
        /// </summary>
        public DateTime Date { get; }

        /// <summary>
        ///     Gets the high for the day.
        /// </summary>
        public int High { get; }

        /// <summary>
        ///     Gets the low for the day.
        /// </summary>
        public int Low { get; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="WeatherData" /> class.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="high">The highest temperature.</param>
        /// <param name="low">The lowest temperature.</param>
        /// <exception cref="ArgumentOutOfRangeException">high - High cannot be lower than the low</exception>
        public WeatherData(DateTime date, int high, int low)
        {
            if (high < low)
            {
                throw new ArgumentOutOfRangeException(nameof(high), "High cannot be lower than the low");
            }

            this.Date = date;
            this.High = high;
            this.Low = low;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Compares to another <see cref="WeatherData" /> object, with priority put to date, high, then low.
        /// </summary>
        /// <param name="nextData">The <see cref="WeatherData" /> compared.</param>
        /// <returns></returns>
        public int CompareTo(WeatherData nextData)
        {
            int value;
            if (this.Date.CompareTo(nextData.Date) == 0)
            {
                value = this.High.CompareTo(nextData.High) == 0
                    ? this.Low.CompareTo(nextData.Low)
                    : this.High.CompareTo(nextData.High);
            }
            else
            {
                value = this.Date.CompareTo(nextData.Date);
            }

            return value;
        }

        /// <summary>
        ///     Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        ///     A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"{this.Date.ToShortDateString()} High:{this.High} Low:{this.Low}";
        }

        #endregion
    }
}