using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WeatherDataAnalysis.Model;
using WeatherDataAnalysis.Utility;

namespace WeatherDataAnalysis.ViewModel
{
    public class AddDataViewModel : INotifyPropertyChanged
    {


        private DateTimeOffset date;
        private int? high;
        private int? low;
        private double? precipitation;



        public DateTimeOffset Date
        {
            get => this.date;
            set
            {
                this.date = value.Date;
                this.isValidInput();
                this.OnPropertyChanged(nameof(this.Date));
            }
        }
        public int? High
        {
            get => this.high;
            set
            {
                this.high = value;
                this.isValidInput();
                this.OnPropertyChanged(nameof(this.High));
            }
        }

        public int? Low
        {
            get => this.low;
            set
            {
                this.low = value;
                this.isValidInput();
                this.OnPropertyChanged(nameof(this.Low));
            }
        }

        public double? Precipitation
        {
            get => this.precipitation;
            set
            {
                var oldValue = this.precipitation;
                this.precipitation = value;
                this.isValidInput();
                this.OnPropertyChanged(nameof(this.Precipitation));
            }
        }

        public WeatherData WeatherData { get; set; }

        private bool isAddable;

        public bool IsAddable
        {
            get => this.isAddable;
            set
            {
                this.isAddable = value;
                this.OnPropertyChanged(nameof(this.IsAddable));
            }
        }



        public AddDataViewModel()
        {
            this.date = DateTimeOffset.Now.Date;
            this.high = null;
            this.low = null;
            this.precipitation = null;
            this.WeatherData = null;
            this.isAddable = false;

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void isValidInput()
        {
            this.IsAddable = this.high != null && this.low != null && this.precipitation != null && this.low <= this.high;
        }

    }
}
