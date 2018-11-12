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
    /// <summary>
    ///     The view model of the Main Page
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    public class WeatherAnalysisViewModel : INotifyPropertyChanged
    {
        #region Data members

        public const string DefaultYearSelection = "ALL";
        private const int DefaultMaxThreshold = 90;
        private const int DefaultMinThreshold = 32;
        private ObservableCollection<WeatherData> days;
        private string selectedYearFilter;
        private ObservableCollection<string> years;
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

        /// <summary>
        ///     Gets or sets the save to file command.
        /// </summary>
        public RelayCommand SaveToFileCommand { get; set; }

        /// <summary>
        ///     Gets or sets the load file command.
        /// </summary>
        public RelayCommand LoadFileCommand { get; set; }

        /// <summary>
        ///     Gets or sets the add weather data command.
        /// </summary>
        public RelayCommand AddWeatherDataCommand { get; set; }

        /// <summary>
        ///     Gets or sets the clear all data command.
        /// </summary>
        public RelayCommand ClearAllDataCommand { get; set; }

        /// <summary>
        ///     Gets or sets the delete day command.
        /// </summary>
        public RelayCommand DeleteDayCommand { get; set; }

        /// <summary>
        ///     Gets or sets the edit day command.
        /// </summary>
        public RelayCommand EditDayCommand { get; set; }

        /// <summary>
        ///     Gets or sets the size of the selected bucket.
        /// </summary>
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

        /// <summary>
        ///     Gets or sets the ListView days.
        /// </summary>
        public ObservableCollection<WeatherData> ListViewDays
        {
            get => this.listViewDays;
            set
            {
                this.listViewDays = value;
                this.OnPropertyChanged(nameof(this.ListViewDays));
            }
        }

        /// <summary>
        ///     Gets or sets the selected year filter.
        /// </summary>
        public string SelectedYearFilter
        {
            get => this.selectedYearFilter;
            set
            {
                if (value == null || this.selectedYearFilter == null)
                {
                    this.ListViewDays = this.Days;
                    this.selectedYearFilter = DefaultYearSelection;
                }
                else if (value.Equals(DefaultYearSelection))
                {
                    this.ListViewDays = this.Days;
                    this.selectedYearFilter = value;
                }
                else
                {
                    this.selectedYearFilter = value;
                    var filter = int.Parse(this.selectedYearFilter);
                    this.ListViewDays = this.Days.Where(x => x.Date.Year.Equals(filter)).ToObservableCollection();
                }

                this.OnPropertyChanged(nameof(this.Days));
            }
        }

        /// <summary>
        ///     Gets or sets the days which is the collection of all weather data held.
        /// </summary>
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

        /// <summary>
        ///     Gets or sets the years.
        /// </summary>
        public ObservableCollection<string> Years
        {
            get => this.years;
            set
            {
                if (value == null || value.Count == 0 || this.selectedYearFilter == null)
                {
                    this.years = new ObservableCollection<string> {DefaultYearSelection};
                    this.selectedYearFilter = DefaultYearSelection;
                }
                else
                {
                    this.years = new ObservableCollection<string> {DefaultYearSelection}
                                 .Union(value.OrderBy(x => x)).ToObservableCollection();
                }

                if (this.selectedYearFilter == null)
                {
                    this.SelectedYearFilter = DefaultYearSelection;
                }

                this.OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Gets or sets the bucket sizes.
        /// </summary>
        public ObservableCollection<int> BucketSizes
        {
            get => this.bucketSizes;
            set
            {
                this.bucketSizes = value;
                this.OnPropertyChanged(nameof(this.BucketSizes));
            }
        }

        /// <summary>
        ///     Gets or sets the selected weather data.
        /// </summary>
        public WeatherData SelectedWeatherData
        {
            get => this.selectedWeatherData;
            set
            {
                this.selectedWeatherData = value;
                this.OnPropertyChanged(nameof(this.SelectedWeatherData));
                this.DeleteDayCommand.OnCanExecuteChanged();
                this.EditDayCommand.OnCanExecuteChanged();
            }
        }

        /// <summary>
        ///     Gets or sets the maximum threshold.
        /// </summary>
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

        /// <summary>
        ///     Gets or sets the minimum threshold.
        /// </summary>
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

        /// <summary>
        ///     Gets or sets the report.
        /// </summary>
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

        /// <summary>
        ///     Initializes a new instance of the <see cref="WeatherAnalysisViewModel" /> class.
        /// </summary>
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
            this.selectedYearFilter = DefaultYearSelection;
            this.updateReport();
        }

        private void loadCommands()
        {
            this.LoadFileCommand = new RelayCommand(this.loadFile, this.canLoadFile);
            this.SaveToFileCommand = new RelayCommand(this.saveToFile, this.canSaveToFile);
            this.AddWeatherDataCommand = new RelayCommand(this.addWeatherData, this.canAddWeatherData);
            this.ClearAllDataCommand = new RelayCommand(this.clearData, this.isThereAnyData);
            this.DeleteDayCommand = new RelayCommand(this.deleteDay, this.isDaySelected);
            this.EditDayCommand = new RelayCommand(this.editDay, this.isDaySelected);
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

        private bool isThereAnyData(object obj)
        {
            return this.days.Count > 0;
        }

        private bool isDaySelected(object obj)
        {
            return this.selectedWeatherData != null;
        }

        private void deleteDay(object obj)
        {
            if (this.days != null && this.days.Count > 0)
            {
                var lastIndex = this.days.IndexOf(this.selectedWeatherData);
                this.Days.Remove(this.selectedWeatherData);
                this.ListViewDays.Remove(this.selectedWeatherData);
                this.updateReport();
                int nextIndex;
                if (this.days.Count > lastIndex)
                {
                    nextIndex = lastIndex;
                    this.SelectedWeatherData = this.days[nextIndex];
                }
                else if (this.days.Count != 0)
                {
                    nextIndex = lastIndex - 1;
                    this.SelectedWeatherData = this.days[nextIndex];
                }

                this.OnPropertyChanged(nameof(this.Years));
                this.OnPropertyChanged(nameof(this.Days));
            }
        }

        private async void editDay(object obj)
        {
            var viewModel = new EditDayViewModel(this.selectedWeatherData.High, this.selectedWeatherData.Low,
                this.selectedWeatherData.Precipitation);
            var result = await MainPage.LaunchDayEditDialog(viewModel);
            if (result == EditDayDialog.Done)
            {
                var dayIndex = this.days.IndexOf(this.selectedWeatherData);
                var viewIndex = this.ListViewDays.IndexOf(this.selectedWeatherData);
                this.selectedWeatherData.Precipitation = viewModel.Precipitation;
                this.selectedWeatherData.High = viewModel.High;
                this.selectedWeatherData.Low = viewModel.Low;
                this.days[dayIndex] = this.selectedWeatherData;
                this.SelectedWeatherData = this.ListViewDays[viewIndex];
                this.OnPropertyChanged(nameof(this.ListViewDays));
                this.updateReport();
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
            var viewModel = await MainPage.LaunchDialogForAddData();
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
            this.OnPropertyChanged(nameof(this.SelectedYearFilter));
        }

        private async Task handleNewWeatherDataFile(StorageFile file)
        {
            var weatherFileReader = new WeatherFileReader();
            ICollection<WeatherData> weathers;
            if (file.FileType.Equals(".csv"))
            {
                weathers = await weatherFileReader.ReadFileToWeatherCollection(file);
            }
            else if (file.FileType.Equals(".xml"))
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
            if (await MainPage.LaunchDialogForMergeOrReplace() == MergeOrReplaceDialog.Merge)
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
                var passed = int.TryParse(this.selectedYearFilter, out var result);
                if (passed && newWeatherData.Date.Year.Equals(result))
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
                var result = await MainPage.LaunchMultipleDataOnSameDayDialog(oldWeatherData.ToString(),
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

        private IEnumerable<string> findAllYears()
        {
            return this.Days.Select(x => x.Date.Year.ToString()).Distinct();
        }

        #endregion
    }
}