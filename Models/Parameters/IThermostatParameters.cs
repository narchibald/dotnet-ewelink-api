using System;
using System.Collections.Generic;
using System.Text;

namespace EWeLink.Api.Models.Parameters
{
    interface IThermostatParameters
    {
        public decimal? Temperature { get; set; }

        public decimal? Humidity { get; set; }
    }
}
