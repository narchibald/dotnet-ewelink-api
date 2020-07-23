namespace EWeLink.Api.Models
{
    public class EventParameters
    {
        public SwitchState? Switch { get; set; }

        public LinkSwitch?[] Switches { get; set; }
    }
}