using Windows.UI.Xaml.Controls;
using WeatherDataAnalysis.ViewModel;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WeatherDataAnalysis.View
{
    /// <inheritdoc cref="ContentDialog" />
    /// <summary>
    ///     Prompts the user to input a high and low and to select a date.
    ///     User cannot press the add button until all data is entered properly.
    /// </summary>
    /// <seealso cref="T:Windows.UI.Xaml.Controls.ContentDialog" />
    /// <seealso cref="T:Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="T:Windows.UI.Xaml.Markup.IComponentConnector2" />
    public partial class AddDataDialog
    {
        #region Data members

        /// <summary>
        ///     The content dialog return value for add
        /// </summary>
        public const ContentDialogResult Add = ContentDialogResult.Primary;

        #endregion

        #region Properties

        public AddDataViewModel ViewModel { get; }

        #endregion

        #region Constructors

        /// <inheritdoc />
        /// <summary>
        ///     Initializes a new instance of the <see cref="AddDataDialog" /> class.
        /// </summary>
        public AddDataDialog()
        {
            this.InitializeComponent();
            this.ViewModel = new AddDataViewModel();
            DataContext = this.ViewModel;

            DefaultButton = ContentDialogButton.Primary;
        }

        #endregion
    }
}