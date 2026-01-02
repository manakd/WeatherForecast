using Newtonsoft.Json;

namespace Api.WeatherForecast.Models
{
    // These classes define the structure of the api weather.gov return data
    public class ApiWeatherForecastResultClass
    {
        [JsonProperty("properties")]
        public ApiWeatherForecastResultProperty Properties { get; set; }
    }

    public class ApiWeatherForecastResultProperty
    {
        [JsonProperty("units")]
        public string? Units { get; set; }

        [JsonProperty("forecastGenerator")]
        public string? ForecastGenerator { get; set; }
        
        [JsonProperty("periods")]
        public List<ApiWeatherForecastResultPeriod> Periods { get; set; }
    }

    public class ApiWeatherForecastResultPeriod
    {
        [JsonProperty("number")]
        public int Number { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("temperature")]
        public int Temperature { get; set; }

        [JsonProperty("temperatureUnit")]
        public string? TemperatureUnit { get; set; }

        [JsonProperty("windSpeed")]
        public string? WindSpeed { get; set; }

        [JsonProperty("windDirection")]
        public string? WindDirection { get; set; }

        [JsonProperty("icon")]
        public string? Icon { get; set; }

        [JsonProperty("probabilityOfPrecipitation")]
        public ApiWeatherForecastResultPrecip ProbabilityOfPrecipitation { get; set; }

        [JsonProperty("detailedForecast")]
        public string? DetailedForecast { get; set; }

        [JsonProperty("shortForecast")]
        public string? ShortForecast { get; set; }
    }

    public class ApiWeatherForecastResultPrecip
    {
        [JsonProperty("unitCode")]
        public string UnitCode { get; set; }

        [JsonProperty("value")]
        public int Value { get; set; }
    }

}
