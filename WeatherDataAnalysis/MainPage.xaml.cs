using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using WeatherDataAnalysis.Model;
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
        public const int ApplicationHeight = 450;

        /// <summary>
        ///     The application width
        /// </summary>
        public const int ApplicationWidth = 615;

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


        public static async Task<StorageFile> PickFileWithPickerAsync()
        {
            var openPicker = new FileOpenPicker
            {
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


        public static async Task<AddDataViewModel> ExecuteDialogForAddData()
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

        #endregion
    }
}