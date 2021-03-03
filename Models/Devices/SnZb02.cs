using System;
using System.Collections.Generic;
using System.Text;

namespace EWeLink.Api.Models.Devices
{
    using EWeLink.Api.Models.Parameters;

    [DeviceIdentifier(1770)]
    public class SnZb02 : Device<SnZbThermostatParmeters>
    {
    }
}
