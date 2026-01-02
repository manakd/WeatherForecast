using WeatherForecast.Models;

namespace Api.WeatherForecast.Repositories
{
    public interface IForecastLogRepository : IRepository<ForecastLog>
    {
        bool CreateForecastLog(decimal latitude, decimal longitude, int? tempCurrent, string fcJson, string fcUrl, bool fcSuccess, string fcStatus);
    }

    public class ForecastLogRepository : Repository<ForecastLog>, IForecastLogRepository
    {
        public ForecastLogRepository(WeatherForecastDbContext context) : base(context)
        {
            
        }

        public bool CreateForecastLog(decimal latitude, decimal longitude, int? tempCurrent, string fcJson, string fcUrl, bool fcSuccess, string fcStatus)
        {
            bool dbSuccess = false;
            try
            {
                var forecastLog = new ForecastLog();
                // GUID and Date set by SQL
                //forecastLog.LogId = new Guid();
                //forecastLog.ForecastDt = DateTime.Now;
                forecastLog.ForecastLatitude = latitude;
                forecastLog.ForecastLongitude = longitude;
                forecastLog.ForecastTempCurrent = tempCurrent;
                forecastLog.ForecastJson = fcJson;
                forecastLog.ForecastUrl = fcUrl;
                forecastLog.ForecastSuccess = fcSuccess;
                forecastLog.ForecastStatus = fcStatus;
                Context.Add<ForecastLog>(forecastLog);
            }
            catch (Exception ex)
            {
                // Future - return information on exception
            }
            return dbSuccess;
        }
    }
}
