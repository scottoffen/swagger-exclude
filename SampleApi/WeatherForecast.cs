using System;
using SampleApi.Attributes;

namespace SampleApi;

public class WeatherForecast
{
    public DateTime Date { get; set; }

    public int TemperatureC { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    public string Summary { get; set; }

    [SwaggerIgnore]
    public Guid ApiKey { get; set; }

    public HiddenEnum HiddenValue { get; set; }
}
