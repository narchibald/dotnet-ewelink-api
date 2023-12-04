﻿namespace EWeLink.Api.Models.Devices
{
    using EWeLink.Api.Models.Parameters;

    [DeviceIdentifier(77)]
    public class Micro : MultiSwitchDevice<OneSwitchParameters>, IOneSwitchDevice
    {
    }
}
