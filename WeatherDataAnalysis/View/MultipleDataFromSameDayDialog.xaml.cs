using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WeatherDataAnalysis.View
{
    /// <inheritdoc cref="ContentDialog" />
    /// <summary>
    ///     Used to find if the user would like to keep the old data or replace it with the new one
    /// </summary>
    /// <seealso cref="T:Windows.UI.Xaml.Controls.ContentDialog" />
    /// <seealso cref="T:Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="T:Windows.UI.Xaml.Markup.IComponentConnector2" />
    public sealed partial class MultipleDataFromSameDayDialog
    {
        #region Data members

        /// <summary>
        ///     The content dialog return value for replace
        /// </summary>
        public const ContentDialogResult Replace = ContentDialogResult.Primary;

        /// <summary>
        ///     If the user checked the box "do for all" and chose keep
        /// </summary>
        public bool KeepAll;

        /// <summary>
        ///     If the user checked the box "do for all" and chose replace
        /// </summary>
        public bool ReplaceAll;

        #endregion

        #region Constructors

        /// <inheritdoc />
        /// <summary>
        ///     Initializes a new instance of the <see cref="T:WeatherDataAnalysis.View.MultipleDataFromSameDayDialog" /> class.
        /// </summary>
        public MultipleDataFromSameDayDialog()
        {
            this.InitializeComponent();
            DefaultButton = ContentDialogButton.Primary;
            this.KeepAll = false;
            this.ReplaceAll = false;
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

        private void replaceButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (this.doForAllCheckBox.IsChecked == true)
            {
                this.KeepAll = false;
                this.ReplaceAll = true;
            }
        }

        private void keepButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (this.doForAllCheckBox.IsChecked == true)
            {
                this.KeepAll = true;
                this.ReplaceAll = false;
            }
        }

        #endregion
    }
}