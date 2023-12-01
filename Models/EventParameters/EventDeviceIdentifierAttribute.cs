namespace EWeLink.Api.Models.EventParameters
{
    using System;

    [AttributeUsage(AttributeTargets.Class)]
    internal class EventDeviceIdentifierAttribute
        : Attribute
    {
        public EventDeviceIdentifierAttribute(params int[] uiids)
        {
            this.Uiids = uiids;
        }

        public int[] Uiids { get; }
    }
}
