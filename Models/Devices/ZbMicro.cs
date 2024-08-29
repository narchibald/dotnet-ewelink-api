namespace EWeLink.Api.Models.Devices
{
    using EWeLink.Api.Models.Parameters;

    [DeviceIdentifier(7010)]
    public class ZbMicro : Device<ZbMicroParameters>, ISingleSwitchDevice
    {
        SwitchParameters ISingleSwitchDevice.Parameters => new ZbMircoSwitchParameters(Parameters);

        private class ZbMircoSwitchParameters : SwitchParameters
        {
            private readonly ZbMicroParameters parameters;

            public ZbMircoSwitchParameters(ZbMicroParameters parameters)
            {
                this.parameters = parameters;
            }

            public override SwitchState Switch
            {
                get => this.parameters.Switch;
                set => this.parameters.Switch = value;
            }
        }
    }
}