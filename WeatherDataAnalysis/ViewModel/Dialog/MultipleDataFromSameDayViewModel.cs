using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WeatherDataAnalysis.ViewModel.Dialog
{
    public class MultipleDataFromSameDayViewModel : INotifyPropertyChanged
    {
        #region Data members

        private bool isChecked;

        #endregion

        #region Properties

        public bool IsChecked
        {
            get => this.isChecked;
            set
            {
                this.isChecked = value;
                this.OnPropertyChanged(nameof(this.IsChecked));
            }
        }

        #endregion

        #region Constructors

        public MultipleDataFromSameDayViewModel()
        {
            this.isChecked = false;
        }

        #endregion

        #region Methods

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}