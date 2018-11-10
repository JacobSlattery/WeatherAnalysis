using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WeatherDataAnalysis.Extensions;
using WeatherDataAnalysis.Model;

namespace WeatherDataAnalysis.ViewModel
{
    public class ListViewViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<WeatherData> days;

        public ObservableCollection<WeatherData> Days
        {
            get => this.days;
            set
            {
                this.days = value;
                this.OnPropertyChanged();
            } 
        }

        public ListViewViewModel(WeatherDataCollection weatherCollection)
        {
            this.days = weatherCollection.ToObservableCollection();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
    }
}
