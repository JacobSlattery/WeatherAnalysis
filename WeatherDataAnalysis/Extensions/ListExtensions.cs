using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WeatherDataAnalysis.Extensions
{
    public static class ListExtensions
    {
        #region Methods

        /// <summary>
        ///     To the observable collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <returns>An ObservableCollection from a collection</returns>
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> collection)
        {
            return new ObservableCollection<T>(collection);
        }

        #endregion
    }
}