namespace EWeLink.Api.Models.Parameters
{
    public interface IThermostatParameters
    {
        public decimal? Temperature { get; set; }

        public decimal? Humidity { get; set; }
    }
}
