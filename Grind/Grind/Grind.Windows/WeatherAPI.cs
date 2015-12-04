using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ForecastIOPortable;
using Windows.Devices.Geolocation;

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
        public ForecastIOPortable.Models.HourlyForecast HourlyForecast;
        //0 is bad, 1 is good.
        public int statusCode = 0;
        public WeatherAPI()
        {
            setLocation();
            updateWeatherForecast();
        }

        public async void updateWeatherForecast(){
            var API = new ForecastApi(APIKEY);
            var currentForecast = await API.GetWeatherDataAsync(LAT, LONG, Unit.US);
            HourlyForecast = currentForecast.Hourly;
        }

        public async Task<string> getCurrentTemp()
        {
            try
            {
                Locator.DesiredAccuracy = PositionAccuracy.High;
                Geoposition pos = await Locator.GetGeopositionAsync();
                LAT = pos.Coordinate.Latitude;
                LONG = pos.Coordinate.Longitude;
                statusCode = 1;
            }
            catch
            {
                //set to searcy AR
                LAT = 35.2469;
                LONG = -91.7336;
            }

            if (HourlyForecast == null)
            {
                var currentForecast = await API.GetWeatherDataAsync(LAT, LONG, Unit.US);
                HourlyForecast = currentForecast.Hourly;
                return Math.Truncate(HourlyForecast.Hours[0].Temperature).ToString();
            }
            else
            {
                return Math.Truncate(HourlyForecast.Hours[0].Temperature).ToString();
            }
        }

        public async Task<string> getCurrentSummary()
        {
            try
            {
                Locator.DesiredAccuracy = PositionAccuracy.High;
                Geoposition pos = await Locator.GetGeopositionAsync();
                LAT = pos.Coordinate.Latitude;
                LONG = pos.Coordinate.Longitude;
                statusCode = 1;
            }
            catch
            {
                //set to searcy AR
                LAT = 35.2469;
                LONG = -91.7336;
            }

            if (HourlyForecast == null)
            {
                var currentForecast = await API.GetWeatherDataAsync(LAT, LONG, Unit.US);
                HourlyForecast = currentForecast.Hourly;
                return HourlyForecast.Hours[0].Summary.ToString();
            }
            else
            {
                return HourlyForecast.Hours[0].Summary.ToString();
            }
        }

        public async void setLocation()
        {
            try
            {
                Locator.DesiredAccuracy = PositionAccuracy.High;
                Geoposition pos = await Locator.GetGeopositionAsync();
                LAT = pos.Coordinate.Latitude;
                LONG = pos.Coordinate.Longitude;
                statusCode = 1;
            }
            catch
            {
                //set to searcy AR
                LAT = 35.2469;
                LONG = -91.7336;
            }
        }
    }
}
