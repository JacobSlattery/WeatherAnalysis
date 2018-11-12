using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using WeatherDataAnalysis.View;
using WeatherDataAnalysis.ViewModel;
using WeatherDataAnalysis.ViewModel.Dialog;

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
        public const int ApplicationHeight = 510;

        /// <summary>
        ///     The application width
        /// </summary>
        public const int ApplicationWidth = 915;

        public WeatherAnalysisViewModel ViewModel;

        #endregion

        #region Constructors

        /// <inheritdoc />
        /// <summary>
        ///     Initializes a new instance of the <see cref="T:WeatherDataAnalysis.MainPage" /> class.
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();
            this.ViewModel = new WeatherAnalysisViewModel();
            DataContext = this.ViewModel;
            ApplicationView.PreferredLaunchViewSize = new Size {Width = ApplicationWidth, Height = ApplicationHeight};
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(ApplicationWidth, ApplicationHeight));
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Picks the file to open with a file open picker.
        /// </summary>
        /// <returns>
        ///     A file
        /// </returns>
        public static async Task<StorageFile> PickFileWithOpenPicker()
        {
            var openPicker = new FileOpenPicker {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };
            openPicker.FileTypeFilter.Add(".csv");
            openPicker.FileTypeFilter.Add(".txt");
            openPicker.FileTypeFilter.Add(".xml");

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

        /// <summary>
        ///     Picks the file to save to with file save picker.
        /// </summary>
        /// <returns>
        ///     A file
        /// </returns>
        public static async Task<StorageFile> PickFileWithSavePicker()
        {
            var fileSaver = new FileSavePicker {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };
            fileSaver.FileTypeChoices.Add(".", new List<string> {".csv", ".xml"});
            fileSaver.FileTypeChoices.Add("csv", new List<string> {".csv"});
            fileSaver.FileTypeChoices.Add("xml", new List<string> {".xml"});

            return await fileSaver.PickSaveFileAsync();
        }

        /// <summary>
        ///     Launches the dialog for add data.
        /// </summary>
        /// <returns></returns>
        public static async Task<AddDataViewModel> LaunchDialogForAddData()
        {
            AddDataViewModel viewModel = null;

            var dialog = new AddDataDialog();
            var result = await dialog.ShowAsync();
            if (result == AddDataDialog.Add)
            {
                viewModel = dialog.ViewModel;
            }

            return viewModel;
        }

        /// <summary>
        ///     Launches a dialog for merge or replace.
        /// </summary>
        /// <returns>
        ///     The result
        /// </returns>
        public static async Task<ContentDialogResult> LaunchDialogForMergeOrReplace()
        {
            var dialog = new MergeOrReplaceDialog();
            return await dialog.ShowAsync();
        }

        /// <summary>
        ///     Launches a multiple data on same day dialog.
        /// </summary>
        /// <param name="newData">The new data.</param>
        /// <param name="oldData">The old data.</param>
        /// <param name="viewModel">The view model.</param>
        /// <returns></returns>
        public static async Task<ContentDialogResult> LaunchMultipleDataOnSameDayDialog(string newData, string oldData,
            MultipleDataFromSameDayViewModel viewModel)
        {
            var message = "Old Data: " + newData + Environment.NewLine +
                          "New Data: " + oldData;

            var dialog = new MultipleDataFromSameDayDialog(viewModel);
            dialog.SetMessage(message);

            return await dialog.ShowAsync();
        }

        /// <summary>
        ///     Displays the unread lines.
        /// </summary>
        /// <param name="lines">The lines.</param>
        /// <returns>
        ///     A Task
        /// </returns>
        public static async Task DisplayUnreadLines(string lines)
        {
            var dialog = new ContentDialog {
                Title = "Unreadable Lines",
                Content = lines,
                PrimaryButtonText = "Ok",
                DefaultButton = ContentDialogButton.Primary
            };
            if (lines != string.Empty)
            {
                await dialog.ShowAsync();
            }
        }

        /// <summary>
        ///     Launches the day edit dialog.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <returns>
        ///     The result
        /// </returns>
        public static async Task<ContentDialogResult> LaunchDayEditDialog(EditDayViewModel viewModel)
        {
            var dialog = new EditDayDialog(viewModel);
            var result = await dialog.ShowAsync();
            return result;
        }

        #endregion
    }
}