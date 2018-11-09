using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Controls;
using WeatherDataAnalysis.DataTier;
using WeatherDataAnalysis.Model;
using WeatherDataAnalysis.Utility;
using WeatherDataAnalysis.View;
using WeatherDataAnalysis.View.Report;

namespace WeatherDataAnalysis.ViewModel
{
    public class WeatherAnalysisViewModel : INotifyPropertyChanged
    {
        #region Data members

        private MultipleDataFromSameDayDialog sameDayDataDialog;

        private WeatherDataCollection weatherDataCollection;

        private WeatherData selectedWeatherData;

        private ObservableCollection<int> bucketSizes;

        private int selectedBucketSize;
        private int maxThreshold;
        private int minThreshold;

        private string report;


        #endregion

        #region Properties

        public RelayCommand SaveToFileCommand { get; set; }

        public RelayCommand LoadFileCommand { get; set; }

        public RelayCommand AddWeatherDataCommand { get; set; }

        public RelayCommand ClearAllDataCommand { get; set; }

        public RelayCommand UpdateDisplayCommand { get; set; }


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

        public WeatherData SelectedWeatherData
        {
            get => this.selectedWeatherData;
            set
            {
                if (value != this.selectedWeatherData)
                {
                    this.selectedWeatherData = value;
                    this.OnPropertyChanged(nameof(this.SelectedBucketSize));
                }
            }
        }

        public int MaxThreshold
        {
            get => this.maxThreshold;
            set
            {
                if (value != this.maxThreshold)
                {
                    this.maxThreshold = value;
                    this.OnPropertyChanged(nameof(this.MaxThreshold));
                    this.updateReport();
                }
            }
        }

        public int MinThreshold
        {
            get => this.minThreshold;
            set
            {
                if (value != this.minThreshold)
                {
                    this.minThreshold = value;
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
            this.bucketSizes = new ObservableCollection<int>() { 5, 10, 20 };
            this.selectedBucketSize = this.BucketSizes[1];
            this.maxThreshold = 90;
            this.minThreshold = 32;
            this.updateReport();
        }

        private void loadCommands()
        {
            this.LoadFileCommand = new RelayCommand(this.loadFile, this.canLoadFile);
            this.SaveToFileCommand = new RelayCommand(this.saveToFile, this.canSaveToFile);
            this.AddWeatherDataCommand = new RelayCommand(this.addWeatherData, this.canAddWeatherData);
            this.ClearAllDataCommand = new RelayCommand(this.clearData, this.canClearData);
            this.UpdateDisplayCommand = new RelayCommand(this.updateReport, this.canUpdateDisplay);
            
        }

        private bool canLoadFile(object obj) => true;
        private bool canSaveToFile(object obj) => this.weatherDataCollection.Count != 0;
        private bool canAddWeatherData(object obj) => true;
        private bool canClearData(object obj) => this.report.Length > 0;
        private bool canUpdateDisplay(object obj) => true;

        private async void loadFile(object obj)
        {
            var file = await pickFileWithPickerAsync();

            if (file != null)
            {
                await this.handleNewWeatherDataFile(file);
                this.updateReport();
            }
        }

        private async void saveToFile(object obj)
        {
            var fileSaver = new FileSavePicker
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };
            fileSaver.FileTypeChoices.Add("csv", new List<string> { ".csv" });
            fileSaver.FileTypeChoices.Add("xml", new List<string>{".xml"});

            var saveFile = await fileSaver.PickSaveFileAsync();

            if (saveFile != null)
            {
                CachedFileManager.DeferUpdates(saveFile);

                var fileWriter = new WeatherFileWriter();

                await fileWriter.ParseWeatherDataCollectionToFile(this.weatherDataCollection, saveFile);
            }
        }

        private async void addWeatherData(object obj)
        {
            var dialog = new AddDataDialog();
            var result = await dialog.ShowAsync();
            if (result == AddDataDialog.Add)
            {
                var weatherData = new WeatherData(dialog.DateTime, dialog.High, dialog.Low, dialog.Precipitation);
                await this.handleNewWeatherData(weatherData);
                this.updateReport();
            }
        }

        private void clearData(object obj)
        {
            this.weatherDataCollection.Clear();
            this.updateReport();
        }

        private void updateReport(object obj = null)
        {
            this.Report = new ReportBuilder(this.weatherDataCollection).BuildFullReport(this.maxThreshold, this.minThreshold, this.selectedBucketSize);
        }

        private static async Task<StorageFile> pickFileWithPickerAsync()
        {
            var openPicker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };
            openPicker.FileTypeFilter.Add(".csv");
            openPicker.FileTypeFilter.Add(".txt");

            StorageFile file;
            try
            {
                file = await openPicker.PickSingleFileAsync();
            }
            catch (NullReferenceException)
            {
                file = null;
            }

            return file;
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

            this.sameDayDataDialog = new MultipleDataFromSameDayDialog();
            await this.reportUnreadLines(weatherFileReader);
        }

        private async Task handleNewWeatherDataCollection(WeatherDataCollection newWeatherDataCollection)
        {
            var dialog = new MergeOrReplaceDialog();
            var result = await dialog.ShowAsync();
            if (result == MergeOrReplaceDialog.Merge)
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
            if (this.sameDayDataDialog.ReplaceAll)
            {
                this.weatherDataCollection.Add(newWeatherData);
                this.weatherDataCollection.Remove(oldWeatherData);
            }
            else if (!this.sameDayDataDialog.KeepAll)
            {
                var message = $"Old Data: {oldWeatherData}" + Environment.NewLine +
                              $"New Data: {newWeatherData}";

                this.sameDayDataDialog = new MultipleDataFromSameDayDialog();
                this.sameDayDataDialog.SetMessage(message);

                var mergeOrKeepDialogResult = await this.sameDayDataDialog.ShowAsync();
                if (mergeOrKeepDialogResult == MultipleDataFromSameDayDialog.Replace)
                {
                    this.weatherDataCollection.Add(newWeatherData);
                    this.weatherDataCollection.Remove(oldWeatherData);
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

            var dialog = new ContentDialog
            {
                Title = "Unreadable Lines",
                Content = message,
                PrimaryButtonText = "Ok",
                DefaultButton = ContentDialogButton.Primary
            };
            if (message != string.Empty)
            {
                await dialog.ShowAsync();
            }
        }

        #endregion
    }
}
