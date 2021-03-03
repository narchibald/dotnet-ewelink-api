using System;
using System.Collections.Generic;
using System.Text;

namespace EWeLink.Api.Models.Devices
{
    using EWeLink.Api.Models.Parameters;

    interface ISingleSwitchDevice
    {
        SwitchParameters Parameters { get; }
    }
}
