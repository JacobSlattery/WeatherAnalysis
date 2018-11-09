using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

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
    public sealed partial class AddDataDialog
    {
        #region Data members

        /// <summary>
        ///     The content dialog return value for add
        /// </summary>
        public const ContentDialogResult Add = ContentDialogResult.Primary;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the <see cref="System.DateTime" /> given.
        /// </summary>
        /// <value>
        ///     A <see cref="System.DateTime" />.
        /// </value>
        public DateTime DateTime { get; private set; }

        /// <summary>
        ///     Gets the high for the data.
        /// </summary>
        /// <value>
        ///     An <see cref="int" /> representing the high.
        /// </value>
        public int High { get; private set; }

        /// <summary>
        ///     Gets the low for the data.
        /// </summary>
        /// <value>
        ///     An <see cref="int" /> representing the low
        /// </value>
        public int Low { get; private set; }

        public double Precipitation { get; private set; }
        #endregion

        #region Constructors

        /// <inheritdoc />
        /// <summary>
        ///     Initializes a new instance of the <see cref="AddDataDialog" /> class.
        /// </summary>
        public AddDataDialog()
        {
            this.InitializeComponent();
            DefaultButton = ContentDialogButton.Primary;
        }

        #endregion

        #region Methods

        private void onAddButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var dateTimeOffset = this.calenderDataPicker.Date;
            if (dateTimeOffset != null)
            {
                this.DateTime = dateTimeOffset.Value.Date;
                this.High = int.Parse(this.highTextBox.Text);
                this.Low = int.Parse(this.lowTextBox.Text);
                this.Precipitation = double.Parse(this.precipitationTextBox.Text);
            }
        }

        private void handleNewDate(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            this.updateAvailability();
        }

        private void handleHighBoxNewInput(object sender, KeyRoutedEventArgs e)
        {
            if (this.highTextBox.Text != string.Empty && this.highTextBox.Text != "-")
            {
                forceIntegerValue(this.highTextBox);
            }

            this.updateAvailability();
        }

        private void handleLowBoxNewInput(object sender, KeyRoutedEventArgs e)
        {
            if (this.lowTextBox.Text != string.Empty && this.lowTextBox.Text != "-")
            {
                forceIntegerValue(this.lowTextBox);
            }

            this.updateAvailability();
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

        private void updateAvailability()
        {
            if (this.isValidNumbers() && this.calenderDataPicker.Date != null)
            {
                IsPrimaryButtonEnabled = true;
            }
            else
            {
                IsPrimaryButtonEnabled = false;
            }
        }

        private bool isValidNumbers()
        {
            var isValid = false;

            var isHighParsed = int.TryParse(this.highTextBox.Text, out var highResult);
            var isLowParsed = int.TryParse(this.lowTextBox.Text, out var lowResult);

            if (isHighParsed && isLowParsed)
            {
                isValid = lowResult <= highResult;
            }

            return isValid;
        }

        #endregion
    }
}