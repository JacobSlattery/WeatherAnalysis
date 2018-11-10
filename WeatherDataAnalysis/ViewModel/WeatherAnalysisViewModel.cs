﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.Storage;
using WeatherDataAnalysis.DataTier;
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
        private WeatherDataCollection weatherDataCollection;
        private WeatherData selectedWeatherData;
        private ObservableCollection<int> bucketSizes;
        private int selectedBucketSize;
        private int maxThreshold;
        private int minThreshold;
        private bool replaceAll;
        private bool keepAll;
        private string report;

        #endregion

        #region Properties

        public RelayCommand SaveToFileCommand { get; set; }

        public RelayCommand LoadFileCommand { get; set; }

        public RelayCommand AddWeatherDataCommand { get; set; }

        public RelayCommand ClearAllDataCommand { get; set; }

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

        public ObservableCollection<int> BucketSizes
        {
            get => this.bucketSizes;
            set
            {
                this.bucketSizes = value;
                this.OnPropertyChanged(nameof(this.BucketSizes));
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
            this.weatherDataCollection = new WeatherDataCollection();
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
        }

        private bool canLoadFile(object obj)
        {
            return true;
        }

        private bool canSaveToFile(object obj)
        {
            return this.weatherDataCollection.Count != 0;
        }

        private bool canAddWeatherData(object obj)
        {
            return true;
        }

        private bool canClearData(object obj)
        {
            return this.report.Length > 0;
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

                await fileWriter.ParseWeatherDataCollectionToFile(this.weatherDataCollection, saveFile);
            }
        }

        private async void addWeatherData(object obj)
        {
            var viewModel = await MainPage.ExecuteDialogForAddData();
            if (viewModelHasNonNullValues(viewModel))
            {
                // ReSharper disable twice PossibleInvalidOperationException
                var weatherData = new WeatherData(viewModel.Date.Date, viewModel.High.Value, viewModel.Low.Value,
                    viewModel.Precipitation.Value);
                await this.handleNewWeatherData(weatherData);
                this.updateReport();
            }
        }

        private void clearData(object obj)
        {
            this.weatherDataCollection.Clear();
            this.updateReport();
        }

        private void updateReport()
        {
            this.Report = new ReportBuilder(this.weatherDataCollection).BuildFullReport(this.maxThreshold,
                this.minThreshold, this.selectedBucketSize);
        }

        private async Task handleNewWeatherDataFile(StorageFile file)
        {
            var weatherFileReader = new WeatherFileReader();
            var weathers = await weatherFileReader.ReadFileToWeatherCollection(file);
            if (this.weatherDataCollection.Count == 0)
            {
                this.weatherDataCollection = weathers;
            }
            else
            {
                await this.handleNewWeatherDataCollection(weathers);
            }

            this.keepAll = false;
            this.replaceAll = false;
            await this.reportUnreadLines(weatherFileReader);
        }

        private async Task handleNewWeatherDataCollection(WeatherDataCollection newWeatherDataCollection)
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
                this.weatherDataCollection = newWeatherDataCollection;
            }
        }

        private async Task handleNewWeatherData(WeatherData newWeatherData)
        {
            if (!this.weatherDataCollection.Contains(newWeatherData) ||
                !this.weatherDataCollection.ContainsWeatherDataWith(newWeatherData.Date))
            {
                this.weatherDataCollection.Add(newWeatherData);
            }
            else if (newWeatherData.CompareTo(this.weatherDataCollection.GetWeatherAtDate(newWeatherData.Date)) != 0)
            {
                await this.handleDifferentDataOnSameDate(newWeatherData,
                    this.weatherDataCollection.GetWeatherAtDate(newWeatherData.Date));
            }
        }

        private async Task handleDifferentDataOnSameDate(WeatherData newWeatherData, WeatherData oldWeatherData)
        {
            if (this.replaceAll)
            {
                this.weatherDataCollection.Add(newWeatherData);
                this.weatherDataCollection.Remove(oldWeatherData);
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

                    this.weatherDataCollection.Add(newWeatherData);
                    this.weatherDataCollection.Remove(oldWeatherData);
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

        private static bool viewModelHasNonNullValues(AddDataViewModel viewModel)
        {
            return viewModel?.High != null && viewModel.Low != null && viewModel.Precipitation != null;
        }

        #endregion
    }
}