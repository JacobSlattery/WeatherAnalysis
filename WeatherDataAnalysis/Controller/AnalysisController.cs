using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using WeatherDataAnalysis.DataTier;
using WeatherDataAnalysis.Model;
using WeatherDataAnalysis.View;
using WeatherDataAnalysis.View.Report;

namespace WeatherDataAnalysis.Controller
{
    /// <summary>
    ///     The controller for the program. Used for handling input from the user with manipulation to the weather collection.
    /// </summary>
    public class AnalysisController
    {
        #region Data members

        private WeatherDataCollection weatherDataCollection;
        private MultipleDataFromSameDayDialog sameDayDataDialog;
        private WeatherFileReader weatherFileReader;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="AnalysisController" /> class.
        /// </summary>
        public AnalysisController()
        {
            this.weatherDataCollection = new WeatherDataCollection();
            this.sameDayDataDialog = new MultipleDataFromSameDayDialog();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Creates an updated report on the current collection.
        /// </summary>
        /// <param name="aboveThreshold">The threshold for the above degree measuring.</param>
        /// <param name="belowThreshold">The threshold for the below degree measuring..</param>
        /// <param name="bucketSize">Size of the buckets in the histogram.</param>
        /// <returns>
        ///     A <see cref="string" /> report built on the current collection.
        /// </returns>
        public string CreateUpdatedReport(int aboveThreshold, int belowThreshold, int bucketSize)
        {
            var reportBuilder = new ReportBuilder(this.weatherDataCollection);

            return reportBuilder.BuildFullReport(aboveThreshold, belowThreshold, bucketSize);
        }

        /// <summary>
        ///     Clears the current weather collection.
        /// </summary>
        public void Clear()
        {
            this.weatherDataCollection.Clear();
        }

        /// <summary>
        ///     Calls an <see cref="AddDataDialog" /> to create a weather that is added to the collection.
        /// </summary>
        /// <returns>
        ///     A <see cref="Task" />
        /// </returns>
        public async Task HandleAddWeather()
        {
            var dialog = new AddDataDialog();
            var result = await dialog.ShowAsync();
            if (result == AddDataDialog.Add)
            {
                var weatherData = new WeatherData(dialog.DateTime, dialog.High, dialog.Low);
                await this.handleNewWeatherData(weatherData);
            }
        }

        /// <summary>
        ///     Handles a new weather data file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>
        ///     A <see cref="Task" />
        /// </returns>
        public async Task HandleNewWeatherDataFile(StorageFile file)
        {
            this.weatherFileReader = new WeatherFileReader();
            var weathers = await this.weatherFileReader.ReadFileToWeatherCollection(file);
            if (this.weatherDataCollection.Count > 0)
            {
                await this.handleNewWeatherDataCollection(weathers);
            }
            else
            {
                this.weatherDataCollection = weathers;
            }

            this.sameDayDataDialog = new MultipleDataFromSameDayDialog();
            await this.reportUnreadLines();
        }

        /// <summary>
        ///     Saves the weather data to file.
        /// </summary>
        public async void SaveWeatherDataToFile()
        {
            var storageFolder = ApplicationData.Current.LocalFolder;
            var sampleFile =
                await storageFolder.CreateFileAsync("WeatherDataSave.csv", CreationCollisionOption.ReplaceExisting);

            await new WeatherFileWriter().ParseWeatherDataCollectionToFile(this.weatherDataCollection, sampleFile);
        }

        private async Task reportUnreadLines()
        {
            var message = string.Empty;
            foreach (var line in this.weatherFileReader.UnreadableLines)
            {
                message += line + Environment.NewLine;
            }

            var dialog = new ContentDialog {
                Title = "Unreadable Lines",
                Content = message,
                PrimaryButtonText = "Ok",
                DefaultButton = ContentDialogButton.Primary
            };
            if (message != string.Empty)
            {
                await dialog.ShowAsync();
            }
        }

        private async Task handleNewWeatherDataCollection(WeatherDataCollection newWeatherDataCollection)
        {
            var dialog = new MergeOrReplaceDialog();
            var result = await dialog.ShowAsync();
            if (result == MergeOrReplaceDialog.Merge)
            {
                foreach (var newWeatherData in newWeatherDataCollection)
                {
                    await this.handleNewWeatherData(newWeatherData);
                }
            }
            else
            {
                this.weatherDataCollection = newWeatherDataCollection;
            }
        }

        private async Task handleNewWeatherData(WeatherData newWeatherData)
        {
            if (!this.weatherDataCollection.Contains(newWeatherData) ||
                !this.weatherDataCollection.ContainsWeatherDataWith(newWeatherData.Date))
            {
                this.weatherDataCollection.Add(newWeatherData);
            }
            else if (newWeatherData.CompareTo(this.weatherDataCollection.GetWeatherAtDate(newWeatherData.Date)) != 0)
            {
                await this.handleDifferentDataOnSameDate(newWeatherData,
                    this.weatherDataCollection.GetWeatherAtDate(newWeatherData.Date));
            }
        }

        private async Task handleDifferentDataOnSameDate(WeatherData newWeatherData, WeatherData oldWeatherData)
        {
            if (this.sameDayDataDialog.ReplaceAll)
            {
                this.weatherDataCollection.Add(newWeatherData);
                this.weatherDataCollection.Remove(oldWeatherData);
            }
            else if (!this.sameDayDataDialog.KeepAll)
            {
                var message = $"Old Data: {oldWeatherData}" + Environment.NewLine +
                              $"New Data: {newWeatherData}";

                this.sameDayDataDialog = new MultipleDataFromSameDayDialog();
                this.sameDayDataDialog.SetMessage(message);

                var mergeOrKeepDialogResult = await this.sameDayDataDialog.ShowAsync();
                if (mergeOrKeepDialogResult == MultipleDataFromSameDayDialog.Replace)
                {
                    this.weatherDataCollection.Add(newWeatherData);
                    this.weatherDataCollection.Remove(oldWeatherData);
                }
            }
        }

        #endregion
    }
}