using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Model;
using WeatherApp.ViewModel.Commands;
using WeatherApp.ViewModel.Helpers;

namespace WeatherApp.ViewModel
{
    public class WeatherVM : INotifyPropertyChanged
    {
        private string query;

        public string Query
        {
            get { return query; }
            set
            {
                query = value;
                OnPropertyChanged ("Query");
            }
        }

        public ObservableCollection<City> Cities { get; set; }

        private CurrentConditions currentConditions;

        public CurrentConditions CurrentConditions
        {
            get { return currentConditions; }
            set
            {
                currentConditions = value;
                OnPropertyChanged ("CurrentConditions");
            }
        }

        private City selectedCity;

        public City SelectedCity
        {
            get { return selectedCity; }
            set
            {
                selectedCity = value;
                OnPropertyChanged ("SelectedCity");
                GetCurrentConditions();
            }
        }

        public SearchCommand SearchCommand { get; set; }

        public WeatherVM ()
        {
            if ( DesignerProperties.GetIsInDesignMode (new System.Windows.DependencyObject ()) )
            {
                SelectedCity = new City
                {
                    LocalizedName = "Kitchener"
                };
                CurrentConditions = new CurrentConditions
                {
                    WeatherText = "Sunny",
                    Temperature = new Temperature
                    {
                        Metric = new Units
                        {
                            Value = "22"
                        }
                    }
                };
            }

            SearchCommand = new SearchCommand (this);
            Cities = new ObservableCollection<City> ();
        }

        public async void GetCurrentConditions()
        {
            Query = string.Empty;
            Cities.Clear ();
            CurrentConditions = await AccuWeatherHelper.GetCurrentConditions(SelectedCity.Key);
        }

        public async void MakeQuery()
        {
            var cities = await AccuWeatherHelper.GetCities(Query);

            Cities.Clear ();

            foreach (var city in cities)
            {
                Cities.Add (city);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged ( string propertyName )
        {
            PropertyChanged?.Invoke (this, new PropertyChangedEventArgs (propertyName));
        }
    }
}
