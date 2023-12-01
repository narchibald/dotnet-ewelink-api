using EWeLink.Api.Models.Devices;

namespace EWeLink.Api.Models
{
    public class Thing
    {
        public int ItemType { get; set; }

        public IDevice ItemData { get; set; }
    }
}