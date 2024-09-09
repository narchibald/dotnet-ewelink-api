namespace EWeLink.Api.Models.Devices
{
    using EWeLink.Api.Models.Parameters;

    [DeviceIdentifier(204)]
    public class AiBridge26 : Device<IHostParameters>
    {
        public override bool IsPoweredByCube => true;
    }
}
