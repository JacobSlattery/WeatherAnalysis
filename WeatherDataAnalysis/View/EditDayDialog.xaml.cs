using Windows.UI.Xaml.Controls;
using WeatherDataAnalysis.ViewModel.Dialog;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WeatherDataAnalysis.View
{
    public sealed partial class EditDayDialog
    {

        public const ContentDialogResult Done = ContentDialogResult.Primary;
        public EditDayDialog(EditDayViewModel viewModel)
        {
            this.InitializeComponent();
            this.DataContext = viewModel;
        }

    }
}
