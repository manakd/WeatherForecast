using Api.WeatherForecast.Models;

namespace Api.WeatherForecast.Logic
{
    public static class WeatherForecastTransformers
    {

        public static Func<ApiWeatherForecastResultPeriod, WeatherForecastBlock> ToApiForecastBlock()
        {
            var result = new Func<ApiWeatherForecastResultPeriod, WeatherForecastBlock>(src =>
            {
                var dayNames = src?.Name?.Split(' ');

                var dayName = (src?.Name == "Tonight" || src?.Name == "Today")
                    ? src.Name
                    : (dayNames != null && dayNames.Length > 0) ? dayNames[0] : "";
                var partName = (dayNames != null && (dayNames.Length) > 1) ? dayNames[1] : "";
                var dest = new WeatherForecastBlock
                {
                    DayOfWeek = dayName,
                    PartOfDay = partName,
                    TemperatureF = src.Temperature,
                    Winds = src.WindDirection + " " + src.WindSpeed,
                    ChanceOfPrecipt = src.ProbabilityOfPrecipitation.Value,
                    Icon = src?.Icon ?? "",
                    Summary = src.ShortForecast,
                    Details = src.DetailedForecast
                };
                return dest;
            });
            return result;
        }

    }
}
