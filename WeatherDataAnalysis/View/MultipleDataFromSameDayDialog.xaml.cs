using Windows.UI.Xaml.Controls;
using WeatherDataAnalysis.ViewModel.Dialog;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WeatherDataAnalysis.View
{
    /// <inheritdoc cref="ContentDialog" />
    /// <summary>
    ///     Used to find if the user would like to keep the old data or replace it with the new one
    /// </summary>
    /// <seealso cref="ContentDialog" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    public sealed partial class MultipleDataFromSameDayDialog
    {
        #region Data members

        /// <summary>
        ///     The content dialog return value for replace
        /// </summary>
        public const ContentDialogResult Replace = ContentDialogResult.Primary;

        #endregion

        #region Constructors

        /// <inheritdoc />
        /// <summary>
        ///     Initializes a new instance of the <see cref="T:WeatherDataAnalysis.View.MultipleDataFromSameDayDialog" /> class.
        /// </summary>
        public MultipleDataFromSameDayDialog(MultipleDataFromSameDayViewModel viewModel)
        {
            this.InitializeComponent();
            DataContext = viewModel;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Sets the message that is displayed.
        /// </summary>
        /// <param name="message">The message.</param>
        public void SetMessage(string message)
        {
            this.messageTextBlock.Text = message;
        }

        #endregion
    }
}