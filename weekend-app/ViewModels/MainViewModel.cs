using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using weekend_app.Helpers;
using weekend_app.Models;
using weekend_app.Views;

namespace weekend_app.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private List<Country> _countryList;
        private List<Weekend> _weekendList;
        private string _countryDownloadStatus;
        private bool _isCountriesDownloaded = false;
        private string _countryName;
        private string _fullCountryName;
        private string _countryCodeFromInput;
        private bool _isWeekendsDownloaded = false;
        private string _weekendDownloadStatus;
        private string _year = DateTime.Now.Year.ToString();
        public DelegateCommand LoadedCommand => new DelegateCommand(async _ => ExecuteLoadedAsync());
        public DelegateCommand DownloadCountriesAgainCommand => new DelegateCommand(async _ => DownloadCountriesAgainAsync());
        public DelegateCommand SearchWeekendsCommand => new DelegateCommand(async _ => SearchWeekendsAsync());
        public DelegateCommand SaveInDatabaseCommand => new DelegateCommand(async _ => SaveInDatabaseAsync());
        public DelegateCommand ShowDatabaseCommand => new DelegateCommand(async _ => ShowDatabase());
        public List<Country> CountryList
        {
            get { return _countryList; }
            set
            {
                _countryList = value;
            }
        }
        public List<Weekend> WeekendList
        {
            get { return _weekendList; }
            set
            {
                _weekendList = value;
                OnPropertyChanged(nameof(WeekendList));
            }
        }
        public string WeekendDownloadStatus
        {
            get { return _weekendDownloadStatus; }
            set
            {
                _weekendDownloadStatus = value;
                OnPropertyChanged(nameof(WeekendDownloadStatus));
            }
        }
        public bool IsWeekendsDownloaded
        {
            get { return _isWeekendsDownloaded; }
            set
            {
                _isWeekendsDownloaded = value;
                OnPropertyChanged(nameof(IsWeekendsDownloaded));
            }
        }
        public string CountryDownloadStatus
        {
            get { return _countryDownloadStatus; }
            set {
                _countryDownloadStatus = value;
                OnPropertyChanged(nameof(CountryDownloadStatus));
            }
        }
        public bool IsCountriesDownloaded
        {
            get { return _isCountriesDownloaded; }
            set {
                _isCountriesDownloaded = value;
                OnPropertyChanged(nameof(IsCountriesDownloaded));
            }
        }
        public string CountryName {
            get { return _countryName; }
            set
            {
                _countryName = value;
                OnPropertyChanged(CountryName);
                SearchWeekendsAsync();
            }
        }
        public string Year
        {
            get { return _year; }
            set
            {
                _year = value;
                OnPropertyChanged(Year);
                SearchWeekendsAsync();
            }
        }
        public MainViewModel()
        {
           
        }
        private async Task ExecuteLoadedAsync()
        {
            CountryResponseObject countryResponseObject = await RestAPIHelper.GetAllCountries();
            HandleCountryRequest(countryResponseObject);
            
        }
        private async Task SearchWeekendsAsync()
        {
            string fullCountryName;
            _countryCodeFromInput = ConvertCountryNameToCountryCode(CountryName, out fullCountryName);
            _fullCountryName = fullCountryName;
            WeekendResponseObject weekendResponseObject = await RestAPIHelper.GetAllWeekendsForCountryAndYear(_countryCodeFromInput, Year);
            HandleWeekendRequest(weekendResponseObject);
        }
        private void HandleWeekendRequest(WeekendResponseObject weekendResponseObject)
        {
            WeekendList = new List<Weekend>();
            if (weekendResponseObject == null)
            {
                WeekendDownloadStatus = $"Response object is null";
                IsWeekendsDownloaded = false;
            }
            if (weekendResponseObject.Status)
            {
                if (weekendResponseObject.WeekendList != null && weekendResponseObject.WeekendList.Count != 0)
                {
                    WeekendDownloadStatus = $"Status for downloading weekends - {weekendResponseObject.Status}. Status code - {weekendResponseObject.Code}. Weekends count - {weekendResponseObject.WeekendList.Count}. Country - {_countryCodeFromInput}, Year - {Year}";
                    IsWeekendsDownloaded = true;
                    WeekendList = weekendResponseObject.WeekendList;
                }
                else
                {
                    WeekendDownloadStatus = $"Status for downloading weekends - {weekendResponseObject.Status}. Status code - {weekendResponseObject.Code}. Weekends count - 0. Input - {_countryCodeFromInput}, Year - {Year}";
                    IsWeekendsDownloaded = false;
                }
            }
            else
            {
                WeekendDownloadStatus = $"Status for downloading weekends - {weekendResponseObject.Status}. Reason - {weekendResponseObject.FailMessage}. Input - {_countryCodeFromInput}, Year - {Year}";
                IsWeekendsDownloaded = false;
            }
        }
        private string ConvertCountryNameToCountryCode(string countryName, out string fullName)
        {
            fullName = "";
            if (string.IsNullOrEmpty(countryName))
            {
                return "";
            }
            if(CountryList == null)
            {
                return countryName;
            }
            foreach(var country in CountryList)
            {
                if (country.Name.ToLower().StartsWith(countryName.ToLower()))
                {
                    fullName = country.Name;
                    return country.Code;
                }
            }
            return countryName;
        }
        private async Task DownloadCountriesAgainAsync()
        {
            CountryResponseObject countryResponseObject = await RestAPIHelper.GetAllCountries();
            HandleCountryRequest(countryResponseObject);
        }
        private void HandleCountryRequest(CountryResponseObject countryResponseObject)
        {
            if(countryResponseObject == null)
            {
                CountryDownloadStatus = $"Response object is null";
                IsCountriesDownloaded=false;
            }
            if (countryResponseObject.Status)
            {
                if(countryResponseObject.CountryList != null && countryResponseObject.CountryList.Count != 0)
                {
                    CountryDownloadStatus = $"Status for downloading countries - {countryResponseObject.Status}. Status code - {countryResponseObject.Code}. Countries count - {countryResponseObject.CountryList.Count}";
                    IsCountriesDownloaded = true;
                    CountryList = countryResponseObject.CountryList;
                }
                else
                {
                    CountryDownloadStatus = $"Status for downloading countries - {countryResponseObject.Status}. Status code - {countryResponseObject.Code}. Countries count - 0";
                    IsCountriesDownloaded = false;
                }
            }
            else
            {
                CountryDownloadStatus = $"Status for downloading countries - {countryResponseObject.Status}. Reason - {countryResponseObject.FailMessage}";
                IsCountriesDownloaded = false;
            }
        }
        private void SaveInDatabaseAsync()
        {
            DataBaseHelper.SaveDataInDatabase(_fullCountryName, _countryCodeFromInput, WeekendList);
        }
        private void ShowDatabase()
        {
            List<DatabaseModel> dataList = DataBaseHelper.GetDataFromDataBase();
            if(dataList != null)
            {
                Window messageWindow = new MessageWindow();
                messageWindow.DataContext = new MessageViewModel(dataList);
                messageWindow.Show();
            }
        }


    }

    

}
