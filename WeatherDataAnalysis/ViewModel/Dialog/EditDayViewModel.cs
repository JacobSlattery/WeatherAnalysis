using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WeatherDataAnalysis.ViewModel.Dialog
{
    public class EditDayViewModel : INotifyPropertyChanged
    {

        
        private int high;
        private int low;
        private double precipitation;

        public int High
        {
            get => this.high;
            set
            {
                this.high = value;
                this.OnPropertyChanged(nameof(this.High));
            }
        }

        public int Low
        {
            get => this.low;
            set
            {
                this.low = value;
                this.OnPropertyChanged(nameof(this.Low));
            }
        }

        public double Precipitation
        {
            get => this.precipitation;
            set
            {
                this.precipitation = value;
                this.OnPropertyChanged(nameof(this.Precipitation));
            }
        }


        public EditDayViewModel(int high, int low, double precipitation)
        {
            this.high = high;
            this.low = low;
            this.precipitation = precipitation;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



    }
}
