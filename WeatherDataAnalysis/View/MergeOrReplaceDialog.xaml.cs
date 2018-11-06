using System;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WeatherDataAnalysis.View
{
    /// <inheritdoc cref="ContentDialog" />
    /// <summary>
    ///     Used for asking if the user wants to merge or replace old data with new data
    /// </summary>
    /// <seealso cref="T:Windows.UI.Xaml.Controls.ContentDialog" />
    /// <seealso cref="T:Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="T:Windows.UI.Xaml.Markup.IComponentConnector2" />
    public sealed partial class MergeOrReplaceDialog
    {
        #region Data members

        /// <summary>
        ///     The content dialog return value for merge
        /// </summary>
        public const ContentDialogResult Merge = ContentDialogResult.Primary;

        #endregion

        #region Constructors

        /// <inheritdoc />
        /// <summary>
        ///     Initializes a new instance of the <see cref="T:WeatherDataAnalysis.View.MergeOrReplaceDialog" /> class.
        /// </summary>
        public MergeOrReplaceDialog()
        {
            this.InitializeComponent();
            DefaultButton = ContentDialogButton.Primary;
            Content = "Would you like to merge old data with new data" + Environment.NewLine +
                      "or replace the old data with the new data?";
        }

        #endregion
    }
}