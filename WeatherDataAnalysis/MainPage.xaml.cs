using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using WeatherDataAnalysis.Controller;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace WeatherDataAnalysis
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage

    {
        #region Data members

        /// <summary>
        ///     The application height
        /// </summary>
        public const int ApplicationHeight = 450;

        /// <summary>
        ///     The application width
        /// </summary>
        public const int ApplicationWidth = 615;

        /// <summary>
        ///     The bucket sizes
        /// </summary>
        private const int DefaultMaxThreshold = 90;

        private const int DefaultMinThreshold = 32;
        private const int DefaultBucketSize = 10;
        private readonly Collection<int> bucketSizes = new Collection<int> {5, DefaultBucketSize, 20};
        private readonly AnalysisController controller;

        #endregion

        #region Constructors

        /// <inheritdoc />
        /// <summary>
        ///     Initializes a new instance of the <see cref="T:WeatherDataAnalysis.MainPage" /> class.
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();
            this.controller = new AnalysisController();
            this.bucketSizeComboBox.ItemsSource = this.bucketSizes;
            this.bucketSizeComboBox.SelectedItem = DefaultBucketSize;

            ApplicationView.PreferredLaunchViewSize = new Size {Width = ApplicationWidth, Height = ApplicationHeight};
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(ApplicationWidth, ApplicationHeight));
        }

        #endregion

        #region Methods

        private async void loadFile_Click(object sender, RoutedEventArgs e)
        {
            var file = await pickFileWithPickerAsync();

            if (file != null)
            {
                await this.controller.HandleNewWeatherDataFile(file);
                this.updateReport();
            }
        }

        private static async Task<StorageFile> pickFileWithPickerAsync()
        {
            var openPicker = new FileOpenPicker {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };
            openPicker.FileTypeFilter.Add(".csv");
            openPicker.FileTypeFilter.Add(".txt");

            StorageFile file;
            try
            {
                file = await openPicker.PickSingleFileAsync();
            }
            catch (NullReferenceException)
            {
                file = null;
            }

            return file;
        }

        private void onAboveThresholdNewCharacter(UIElement sender, CharacterReceivedRoutedEventArgs args)
        {
            forceIntegerValue(this.aboveThresholdTextBox);
            this.updateReport();
        }

        private void onBelowThresholdNewCharacter(UIElement sender, CharacterReceivedRoutedEventArgs args)
        {
            forceIntegerValue(this.belowThresholdTextBox);
            this.updateReport();
        }

        private static void forceIntegerValue(TextBox textBox)
        {
            int.TryParse(textBox.Text, out var result);
            if (result == 0 && textBox.Text != string.Empty && textBox.Text != "-" && textBox.Text != "0")
            {
                deleteCharacter(textBox);
            }
        }

        private static void deleteCharacter(TextBox textBox)
        {
            var pos = textBox.SelectionStart - 1;
            textBox.Text = textBox.Text.Remove(pos, 1);
            textBox.SelectionStart = pos;
        }

        private int getAboveDegreeThreshold()
        {
            int output;
            try
            {
                output = int.Parse(this.aboveThresholdTextBox.Text);
            }
            catch
            {
                output = DefaultMaxThreshold;
            }

            return output;
        }

        private int getBelowDegreeThreshold()
        {
            int output;

            try
            {
                output = int.Parse(this.belowThresholdTextBox.Text);
            }
            catch
            {
                output = DefaultMinThreshold;
            }

            return output;
        }

        private int getBucketSize()
        {
            return (int?) this.bucketSizeComboBox.SelectedItem ?? DefaultBucketSize;
        }

        private void updateReport()
        {
            this.summaryTextBox.Text = this.controller.CreateUpdatedReport(this.getAboveDegreeThreshold(),
                this.getBelowDegreeThreshold(), this.getBucketSize());
        }

        private void onClearButton_Click(object sender, RoutedEventArgs e)
        {
            this.controller.Clear();
            this.updateReport();
        }

        private async void onAddButton_Click(object sender, RoutedEventArgs e)
        {
            await this.controller.HandleAddWeather();
            this.updateReport();
        }

        private void onSelectionChange(object sender, SelectionChangedEventArgs e)
        {
            this.updateReport();
        }

        private void onSaveFile_Click(object sender, RoutedEventArgs e)
        {
            this.controller.SaveWeatherDataToFile();
        }

        #endregion
    }
}