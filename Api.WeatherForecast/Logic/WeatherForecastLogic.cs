using Api.WeatherForecast.Logic;
using Api.WeatherForecast.Models;
using Api.WeatherForecast.Repositories;
using Azure;
using Azure.Core;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text.Json;
using WeatherForecast.Models;

namespace Api.WeatherForecast.Logic
{
    public interface IWeatherForecastLogic
    {
        public Task<WeatherForecastData> GetForecastByPosition(decimal lat, decimal lon, WeatherForecastDbContext wfContext);
    }

    public class WeatherForecastLogic : IWeatherForecastLogic
    {
        //private IConfiguration _configuration;
        //public WeatherForecastLogic(IConfiguration configuration) 
        public WeatherForecastLogic()
        {
            //_configuration = configuration;
        }

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public async Task<WeatherForecastData> GetForecastByPosition(decimal lat, decimal lon, WeatherForecastDbContext wfContext)
        {
            var returnData = new WeatherForecastData(lat, lon);
            string forecastUrl = string.Empty;
            int? currentTemp = null;
            string step = string.Empty;

            try
            {

                // Get forecast properties
                //example url = "https://api.weather.gov/points/39.7456,-97.0892";
                step = "get points";
                var url = $"https://api.weather.gov/points/{lat},{lon}";
                var result = await GetExternalResponse(url);

                step = "parse points";
                JObject joResponse = JObject.Parse(result);
                string data = JObject.Parse(result)["properties"]?.ToString() ?? "";
                var props = JObject.Parse(result)["properties"]?.ToString() ?? "";
                forecastUrl = JObject.Parse(props)["forecast"]?.ToString() ?? "";
                
                var relativeLocation = JObject.Parse(props)["relativeLocation"]?.ToString();
                var city = "";
                var state = "";
                if (relativeLocation != null)
                {
                    var locProps = JObject.Parse(relativeLocation)["properties"]?.ToString();
                    city = JObject.Parse(locProps)["city"]?.ToString();
                    state = JObject.Parse(locProps)["state"]?.ToString();
                }
                returnData.City = city;
                returnData.State = state;

                if (!string.IsNullOrEmpty(forecastUrl))
                {
                    step = "get forecast";
                    string fcResult = await GetExternalResponse(forecastUrl) ?? "";
                    ApiWeatherForecastResultClass fcObj = JsonConvert.DeserializeObject<ApiWeatherForecastResultClass>(fcResult) ?? new ApiWeatherForecastResultClass();
                    if (fcObj.Properties?.Periods?.Count > 0)
                    {
                        step = "Parse Forecasts";
                        currentTemp = fcObj.Properties.Periods[0].Temperature;
                        // Convert data to return object
                        returnData.DayForecasts = fcObj.Properties.Periods
                            .Select(WeatherForecastTransformers.ToApiForecastBlock())
                            .ToList();
                    }
                    returnData.Success = true;
                    returnData.Message = "Success";
                }
                else
                {
                    returnData.Success = false;
                    returnData.Message = "Location outside of forecast supplied range";
                }
            }
            catch (Exception ex)
            {
                var exInfo = ex.ToString();
                returnData.Message = (step == "get points") ? "Location outside of forecastable range"
                    : (step == "parse points") ? "Unable to parse points: " + ex.Message
                    : (step == "get forecast") ? "Unable to get forecast: " + ex.Message
                    : (step == "Parse forecasts") ? "Unable to read forecast data: " + ex.Message
                    : ex.Message;
                returnData.Success = false;
            }

            // Log the forecast event
            try
            {
                var forecastLogRepository = new ForecastLogRepository(wfContext);
                //string jsonString = JsonSerializer.Serialize(returnData);
                string jsonString = JsonConvert.SerializeObject(returnData);
                forecastLogRepository.CreateForecastLog(lat, lon, currentTemp, jsonString, forecastUrl, returnData.Success, returnData.Message);
                returnData.LogSuccess = (forecastLogRepository.SaveChanges() > 0);
            }
            catch (Exception ex)
            {
                returnData.LogSuccess = false;
            }

            return returnData;
        }

        private async Task<string> GetExternalResponse(string url)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "Other");
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }

        // Test only
        public IEnumerable<WeatherForecastBlock> GetForecastByPositionTEST(decimal lat, decimal lon)
        {
            // Log data
            var forecastLog = new ForecastLog { ForecastLatitude = lat, ForecastLongitude = lon };
            var ForecastTempHi = Random.Shared.Next(-20, 55);
            var ForecastTempLo = ForecastTempHi - 20;
            return Enumerable.Range(1, 5).Select(index => new WeatherForecastBlock
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureF = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }

}
