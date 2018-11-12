using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.Storage;
using WeatherDataAnalysis.DataTier;
using WeatherDataAnalysis.Extensions;
using WeatherDataAnalysis.Model;
using WeatherDataAnalysis.Utility;
using WeatherDataAnalysis.View;
using WeatherDataAnalysis.View.Report;
using WeatherDataAnalysis.ViewModel.Dialog;

namespace WeatherDataAnalysis.ViewModel
{
    public class WeatherAnalysisViewModel : INotifyPropertyChanged
    {
        #region Data members

        private const int DefaultMaxThreshold = 90;
        private const int DefaultMinThreshold = 32;
        private ObservableCollection<WeatherData> days;
        private int selectedYearFilter;
        private ObservableCollection<int> years;
        private WeatherData selectedWeatherData;
        private ObservableCollection<int> bucketSizes;
        private int selectedBucketSize;
        private int maxThreshold;
        private int minThreshold;
        private bool replaceAll;
        private bool keepAll;
        private string report;
        private ObservableCollection<WeatherData> listViewDays;

        #endregion

        #region Properties

        public RelayCommand SaveToFileCommand { get; set; }

        public RelayCommand LoadFileCommand { get; set; }

        public RelayCommand AddWeatherDataCommand { get; set; }

        public RelayCommand ClearAllDataCommand { get; set; }

        public RelayCommand DeleteDayCommand { get; set; }

        public ObservableCollection<WeatherData> Days
        {
            get => this.days;
            set
            {
                this.days = value.OrderBy(x => x.Date).ToObservableCollection();
                this.OnPropertyChanged();
                this.OnPropertyChanged(nameof(this.SelectedYearFilter));
                if (this.listViewDays == null || this.listViewDays.Count == 0)
                {
                    this.ListViewDays = this.days;
                }
                this.updateReport();
            }
        }

        public ObservableCollection<int> Years
        {
            get => this.years;
            set
            {
                this.years = value.OrderBy(x => x).ToObservableCollection();
                this.OnPropertyChanged();
            }
        }

        public ObservableCollection<int> BucketSizes
        {
            get => this.bucketSizes;
            set
            {
                this.bucketSizes = value;
                this.OnPropertyChanged(nameof(this.BucketSizes));
            }
        }

        public WeatherData SelectedWeatherData
        {
            get => this.selectedWeatherData;
            set
            {
                this.selectedWeatherData = value;
                this.OnPropertyChanged(nameof(this.SelectedWeatherData));
                this.DeleteDayCommand.OnCanExecuteChanged();
            }
        }

        public int? MaxThreshold
        {
            get => this.maxThreshold;
            set
            {
                if (value != this.maxThreshold)
                {
                    if (value == null)
                    {
                        this.maxThreshold = DefaultMaxThreshold;
                    }
                    else
                    {
                        this.maxThreshold = (int) value;
                    }

                    this.OnPropertyChanged(nameof(this.MaxThreshold));
                    this.updateReport();
                }
            }
        }

        public int? MinThreshold
        {
            get => this.minThreshold;
            set
            {
                if (value != this.minThreshold)
                {
                    if (value == null)
                    {
                        this.minThreshold = DefaultMinThreshold;
                    }
                    else
                    {
                        this.minThreshold = (int) value;
                    }

                    this.OnPropertyChanged(nameof(this.MinThreshold));
                    this.updateReport();
                }
            }
        }

        public int SelectedBucketSize
        {
            get => this.selectedBucketSize;
            set
            {
                this.selectedBucketSize = value;
                this.OnPropertyChanged(nameof(this.SelectedBucketSize));
                this.updateReport();
            }
        }

        public ObservableCollection<WeatherData> ListViewDays
        {
            get => this.listViewDays;
            set
            {
                this.listViewDays = value;
                this.OnPropertyChanged();
            }
        }

        public int SelectedYearFilter
        {
            get => this.selectedYearFilter;
            set
            {
                this.selectedYearFilter = value;
                this.ListViewDays = this.days.Where(x => x.Date.Year.Equals(this.selectedYearFilter))
                                .ToObservableCollection();
                this.OnPropertyChanged(nameof(this.ListViewDays));
            }
        }

        public string Report
        {
            get => this.report;
            set
            {
                if (value != this.report)
                {
                    this.report = value;
                    this.OnPropertyChanged(nameof(this.Report));
                    this.SaveToFileCommand.OnCanExecuteChanged();
                    this.ClearAllDataCommand.OnCanExecuteChanged();
                }
            }
        }

        #endregion

        #region Constructors

        public WeatherAnalysisViewModel()
        {
            this.loadCommands();
            this.loadProperties();
        }

        #endregion

        #region Methods

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void loadProperties()
        {
            this.Days = new ObservableCollection<WeatherData>();
            this.Years = this.findAllYears().ToObservableCollection();
            this.bucketSizes = new ObservableCollection<int> {5, 10, 20};
            this.selectedBucketSize = this.BucketSizes[1];
            this.maxThreshold = DefaultMaxThreshold;
            this.minThreshold = DefaultMinThreshold;
            this.replaceAll = false;
            this.keepAll = false;
            this.updateReport();

        }

        private void loadCommands()
        {
            this.LoadFileCommand = new RelayCommand(this.loadFile, this.canLoadFile);
            this.SaveToFileCommand = new RelayCommand(this.saveToFile, this.canSaveToFile);
            this.AddWeatherDataCommand = new RelayCommand(this.addWeatherData, this.canAddWeatherData);
            this.ClearAllDataCommand = new RelayCommand(this.clearData, this.canClearData);
            this.DeleteDayCommand = new RelayCommand(this.deleteDay, this.canDeleteDay);
        }

        private bool canLoadFile(object obj)
        {
            return true;
        }

        private bool canSaveToFile(object obj)
        {
            return this.days.Count != 0;
        }

        private bool canAddWeatherData(object obj)
        {
            return true;
        }

        private bool canClearData(object obj)
        {
            return this.report.Length > 0;
        }

        private bool canDeleteDay(object obj)
        {
            return this.selectedWeatherData != null;
        }

        private void deleteDay(object obj)
        {
            if (this.days != null && this.days.Count > 0)
            {
                var lastIndex = this.days.IndexOf(this.selectedWeatherData);
                int nextIndex;
                this.Days.Remove(this.selectedWeatherData);
                this.ListViewDays.Remove(this.selectedWeatherData);
                this.updateReport();
                if (this.days.Count > lastIndex)
                {
                    nextIndex = lastIndex;
                    this.SelectedWeatherData = this.days[nextIndex];
                } else if (this.days.Count != 0)
                {
                    nextIndex = lastIndex - 1;
                    this.SelectedWeatherData = this.days[nextIndex];
                }

                this.OnPropertyChanged(nameof(this.Days));
            }
        }


        private async void loadFile(object obj)
        {
            var file = await MainPage.PickFileWithOpenPicker();

            if (file != null)
            {
                await this.handleNewWeatherDataFile(file);
                this.updateReport();
            }
        }

        private async void saveToFile(object obj)
        {
            var saveFile = await MainPage.PickFileWithSavePicker();

            if (saveFile != null)
            {
                CachedFileManager.DeferUpdates(saveFile);

                var fileWriter = new WeatherFileWriter();

                await fileWriter.ParseWeatherDataCollectionToFile(this.days, saveFile);
            }
        }

        private async void addWeatherData(object obj)
        {
            var viewModel = await MainPage.ExecuteDialogForAddData();
            if (viewModelHasNoNullValues(viewModel))
            {
                // ReSharper disable twice PossibleInvalidOperationException
                var weatherData = new WeatherData(viewModel.Date.Date, viewModel.High.Value, viewModel.Low.Value,
                    viewModel.Precipitation.Value);
                await this.handleNewWeatherData(weatherData);
                this.keepAll = false;
                this.replaceAll = false;
                this.updateReport();
            }
        }

        private void clearData(object obj)
        {
            this.Days.Clear();
            this.ListViewDays.Clear();
            this.Years = this.findAllYears().ToObservableCollection();
            this.updateReport();
        }

        private void updateReport()
        {
            this.Report = new ReportBuilder(new WeatherDataCollection(this.days)).BuildFullReport(this.maxThreshold,
                this.minThreshold, this.selectedBucketSize);
        }

        private async Task handleNewWeatherDataFile(StorageFile file)
        {
            var weatherFileReader = new WeatherFileReader();
            ICollection<WeatherData> weathers;
            if (file.FileType.Equals(".csv"))
            {
                weathers = await weatherFileReader.ReadFileToWeatherCollection(file);
            }else if (file.FileType.Equals(".xml"))
            {
                weathers = await WeatherCollectionXmlDeserializer.XmlToWeatherCollection(file);
            }
            else
            {
                weathers = new ObservableCollection<WeatherData>();
            }
            
            if (this.days.Count == 0)
            {
                this.Days = weathers.ToObservableCollection();
                this.Years = this.findAllYears().ToObservableCollection();
            }
            else
            {
                await this.handleNewWeatherDataCollection(weathers);
            }

            await this.reportUnreadLines(weatherFileReader);
        }

        private async Task handleNewWeatherDataCollection(ICollection<WeatherData> newWeatherDataCollection)
        {
            if (await MainPage.ExecuteDialogForMergeOrReplace())
            {
                foreach (var newWeatherData in newWeatherDataCollection)
                {
                    await this.handleNewWeatherData(newWeatherData);
                }
            }
            else
            {
                this.Days = newWeatherDataCollection.ToObservableCollection();
                this.Years = this.findAllYears().ToObservableCollection();
            }
            this.keepAll = false;
            this.replaceAll = false;
        }

        private async Task handleNewWeatherData(WeatherData newWeatherData)
        {
            var currentWeatherDataCollection = new WeatherDataCollection(this.days);
            if (!this.days.Contains(newWeatherData) &&
                !currentWeatherDataCollection.ContainsWeatherDataWith(newWeatherData.Date))
            {
                this.Days.Add(newWeatherData);
                if (newWeatherData.Date.Year.Equals(this.SelectedYearFilter))
                {
                    this.ListViewDays.Add(newWeatherData);
                }
            }
            else if (newWeatherData.CompareTo(currentWeatherDataCollection.GetWeatherAtDate(newWeatherData.Date)) != 0)
            {
                await this.handleDifferentDataOnSameDate(newWeatherData,
                    currentWeatherDataCollection.GetWeatherAtDate(newWeatherData.Date));
            }

            this.Days = this.Days;
            this.ListViewDays.OrderBy(x => x.Date).ToObservableCollection();
            this.OnPropertyChanged(nameof(this.ListViewDays));
            this.Years = this.findAllYears().ToObservableCollection();
        }

        private async Task handleDifferentDataOnSameDate(WeatherData newWeatherData, WeatherData oldWeatherData)
        {
            if (this.replaceAll)
            {
                this.days.Add(newWeatherData);
                this.days.Remove(oldWeatherData);
                if (this.listViewDays.Contains(oldWeatherData))
                {
                    this.ListViewDays.Add(newWeatherData);
                    this.ListViewDays.Remove(oldWeatherData);
                }
            }
            else if (!this.keepAll)
            {
                var viewModel = new MultipleDataFromSameDayViewModel();
                var result = await MainPage.ExecuteMultipleDataOnSameDayDialog(oldWeatherData.ToString(),
                    newWeatherData.ToString(), viewModel);

                if (result == MultipleDataFromSameDayDialog.Replace)
                {
                    if (viewModel.IsChecked)
                    {
                        this.replaceAll = true;
                        this.keepAll = false;
                    }

                    this.days.Add(newWeatherData);
                    this.days.Remove(oldWeatherData);
                    if (this.listViewDays.Contains(oldWeatherData))
                    {
                        this.ListViewDays.Add(newWeatherData);
                        this.ListViewDays.Remove(oldWeatherData);
                    }
                }
                else
                {
                    if (viewModel.IsChecked)
                    {
                        this.keepAll = true;
                        this.replaceAll = false;
                    }
                }
            }
        }

        private async Task reportUnreadLines(WeatherFileReader weatherFileReader)
        {
            var message = string.Empty;
            foreach (var line in weatherFileReader.UnreadableLines)
            {
                message += line + Environment.NewLine;
            }

            await MainPage.DisplayUnreadLines(message);
        }

        private static bool viewModelHasNoNullValues(AddDataViewModel viewModel)
        {
            return viewModel?.High != null && viewModel.Low != null && viewModel.Precipitation != null;
        }

        private IEnumerable<int> findAllYears()
        {
            return this.Days.Select(x => x.Date.Year).Distinct();
        }

        #endregion
    }
}