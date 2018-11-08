using System;
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

        #endregion

        #region Constructors

        /// <inheritdoc />
        /// <summary>
        ///     Initializes a new instance of the <see cref="T:WeatherDataAnalysis.MainPage" /> class.
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();

            ApplicationView.PreferredLaunchViewSize = new Size {Width = ApplicationWidth, Height = ApplicationHeight};
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(ApplicationWidth, ApplicationHeight));
        }

        #endregion

        #region Methods

        private void onAboveThresholdNewCharacter(UIElement sender, CharacterReceivedRoutedEventArgs args)
        {
            forceIntegerValue(this.aboveThresholdTextBox);
        }

        private void onBelowThresholdNewCharacter(UIElement sender, CharacterReceivedRoutedEventArgs args)
        {
            forceIntegerValue(this.belowThresholdTextBox);
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

        #endregion
    }
}