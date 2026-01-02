using Api.WeatherForecast.Logic;
using Api.WeatherForecast.Models;
using Microsoft.AspNetCore.Mvc;
using WeatherForecast.Models;

namespace Api.WeatherForecast.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly WeatherForecastDbContext _wfContext;
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IWeatherForecastLogic _weatherForecastLogic;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, WeatherForecastDbContext wfContext, IWeatherForecastLogic wfLogic)
        {
            _logger = logger;
            _wfContext = wfContext;
            _weatherForecastLogic = wfLogic;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<WeatherForecastData> Get(decimal lat, decimal lon)
        {
            var result = await _weatherForecastLogic.GetForecastByPosition(lat, lon, _wfContext);
            return result; 
        }

    }
}
