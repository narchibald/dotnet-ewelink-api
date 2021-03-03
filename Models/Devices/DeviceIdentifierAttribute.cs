namespace EWeLink.Api.Models.Devices
{
    using System;

    [AttributeUsage(AttributeTargets.Class)]
    internal class DeviceIdentifierAttribute : Attribute
    {
        public DeviceIdentifierAttribute(int uiid)
        {
            this.Uiid = uiid;
        }

        public int Uiid { get; }
    }
}
