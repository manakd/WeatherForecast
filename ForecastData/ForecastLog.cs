using System;
using System.Collections.Generic;

namespace WeatherForecast.Models;

public partial class ForecastLog
{
    public Guid LogId { get; set; }

    public DateTime? ForecastDt { get; set; }

    public decimal? ForecastLongitude { get; set; }

    public decimal? ForecastLatitude { get; set; }

    public int? ForecastTempCurrent { get; set; }

    public string? ForecastJson { get; set; }

    public string? ForecastUrl { get; set; }

    public bool? ForecastSuccess { get; set; }

    public string? ForecastStatus { get; set; }
}
