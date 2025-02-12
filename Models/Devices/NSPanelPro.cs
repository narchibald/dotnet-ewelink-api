namespace EWeLink.Api.Models.Devices
{
    using EWeLink.Api.Models.Parameters;

    [DeviceIdentifier(195)]
    [DeviceIdentifier(228)]
    [DeviceIdentifier(7018)]
    public class NSPanelPro : Device<NSPanelProParameters>
    {
        public override bool IsPoweredByCube => true;
    }
}