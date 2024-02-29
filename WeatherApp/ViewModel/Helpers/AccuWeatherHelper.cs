﻿using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Model;
using static WeatherApp.Model.Weather;

namespace WeatherApp.ViewModel.Helpers
{
    public class AccuWeatherHelper
    {
        public const string BASE_URL = "http://dataservice.accuweather.com/";
        public const string AUTOCOMPLETE_ENDPOINT = "locations/v1/cities/autocomplete?apikey={0}&q={1}";
        public const string CURRENT_CONDITION_ENDPOINT = "currentconditions/v1/{0}?apikey={1}";       
        public const string API_KEY = "g5Z8hJIuaXsitlqKEQAjSz4UrBYYYUU4";

        public static async Task<List<City>> GetCities(string query)
        {
            List<City> cities = new();

            string url = BASE_URL + string.Format(AUTOCOMPLETE_ENDPOINT, API_KEY, query);

            using(HttpClient client = new())
            {
                var response = await client.GetAsync (url);
                string json = await response.Content.ReadAsStringAsync();

                cities = JsonConvert.DeserializeObject<List<City>> (json);
            }

            return cities;
        }

        public static async Task<CurrentConditions> GetCurrentConditions(string cityKey)
        {
            CurrentConditions currentConditions = new ();

            string url = BASE_URL + string.Format (CURRENT_CONDITION_ENDPOINT, cityKey, API_KEY);

            using ( HttpClient client = new () )
            {
                var response = await client.GetAsync (url);
                string json = await response.Content.ReadAsStringAsync();

                currentConditions = (JsonConvert.DeserializeObject<List<CurrentConditions>> (json)).FirstOrDefault();
            }

            return currentConditions;
        }

    }
}