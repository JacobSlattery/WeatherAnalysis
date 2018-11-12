using Windows.UI.Xaml.Controls;
using WeatherDataAnalysis.ViewModel.Dialog;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WeatherDataAnalysis.View
{
    public sealed partial class EditDayDialog
    {
        #region Data members

        public const ContentDialogResult Done = ContentDialogResult.Primary;

        #endregion

        #region Constructors

        public EditDayDialog(EditDayViewModel viewModel)
        {
            this.InitializeComponent();
            DataContext = viewModel;
        }

        #endregion
    }
}