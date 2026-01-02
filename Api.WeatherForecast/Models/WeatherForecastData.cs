namespace Api.WeatherForecast.Models
{
    public class WeatherForecastData
    {
        public WeatherForecastData(decimal lat, decimal lon)
        {
            Latitude = lat;
            Longitude = lon;
            Success = false;
            Message = string.Empty;
            DayForecasts = new List<WeatherForecastBlock>();
        }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public List<WeatherForecastBlock> DayForecasts { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public bool LogSuccess { get; set; }
    }

    public class WeatherForecastBlock
    {
        public DateOnly Date { get; set; }
        public string DayOfWeek { get; set; }
        public string PartOfDay { get; set; }

        //public int TemperatureC { get; set; }
        //public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
        public int TemperatureF { get; set; }
        public int TemperatureC => (int) ((TemperatureF - 32) * 5/9);

        public int ChanceOfPrecipt { get; set; }
        public string Winds { get; set; }
        public string Icon { get; set; }

        public string? Details { get; set; }
        public string? Summary { get; set; }
    }
}
