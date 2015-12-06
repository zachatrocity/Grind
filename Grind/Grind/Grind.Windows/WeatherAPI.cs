using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ForecastIOPortable;
using Windows.Devices.Geolocation;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace Grind
{
    class WeatherAPI
    {
        //https://api.darkskyapp.com/v1/forecast/d41d8cd98f00b204e9800998ecf8427e/42.7243,-73.6927
        private static string APIKEY = "bb70271303471570aff4370dabe92cdf";
        private Geolocator Locator = new Geolocator();
        private double LAT;
        private double LONG;
        private ForecastApi API = new ForecastApi(APIKEY);
        public ForecastIOPortable.Models.CurrentDataPoint HourlyForecast;
        //0 is bad, 1 is good.
        public WeatherAPI()
        {
            var API = new ForecastApi(APIKEY);
        }

        public async Task<string> getCurrentTemp()
        {
            if (SettingsPage.Weather && SettingsPage.weatherLocation == "")
            {
                return "0";
            }
            else
            {
                await setLocation();
                var weatherURL = "https://api.forecast.io/forecast/bb70271303471570aff4370dabe92cdf/" + LAT + "," + LONG;

                var httpClient = new HttpClient();
                var content = await httpClient.GetStringAsync(weatherURL);

                JObject weatherJson = JObject.Parse(content);

                return (string)weatherJson["currently"]["temperature"];
            }

        }

        public async Task<string> getCurrentSummary()
        {
            if (SettingsPage.Weather && SettingsPage.weatherLocation == "")
            {
                return "Enter a location.";
            }
            else
            {
                await setLocation();

                var weatherURL = "https://api.forecast.io/forecast/bb70271303471570aff4370dabe92cdf/" + LAT + "," + LONG;

                var httpClient = new HttpClient();
                var content = await httpClient.GetStringAsync(weatherURL);

                JObject weatherJson = JObject.Parse(content);

                return (string)weatherJson["currently"]["summary"];
            }
        }

        public async Task setLocationByString(){
            var geocodeURL = "http://www.mapquestapi.com/geocoding/v1/address?key=AAs1zLHhuSXaxh6INFO6GSOMUK7szrwY&location=" + SettingsPage.weatherLocation;

            var httpClient = new HttpClient();
            var content = await httpClient.GetStringAsync(geocodeURL);

            Regex r = new Regex("\"lat\":(?<lat>\\d*.\\d*)", RegexOptions.IgnoreCase);
            Match m = r.Match(content);

            if(m.Success)
            {
                LAT = Convert.ToDouble(m.Groups["lat"].ToString());
            }

            r = new Regex("\"lng\":(?<lng>-\\d*.\\d*)", RegexOptions.IgnoreCase);
            m = r.Match(content);

            if (m.Success)
            {
                LONG = Convert.ToDouble(m.Groups["lng"].ToString());
            }
        }

        public async Task setLocation()
        {
            try
            {
                Locator.DesiredAccuracy = PositionAccuracy.High;
                Geoposition pos = await Locator.GetGeopositionAsync();
                LAT = pos.Coordinate.Latitude;
                LONG = pos.Coordinate.Longitude;
            }
            catch
            {
                setLocationByString();
            }

            if (LAT == 0.0 || LONG == 0.0)
            {
                await setLocationByString();
            }
        }
    }
}
