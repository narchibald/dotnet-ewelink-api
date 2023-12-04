namespace EWeLink.Api.Models.Parameters
{
    using Newtonsoft.Json;

    public class ZbCurtainParameters : SnZbBatteryParameters
    {
        [JsonProperty("supportPowConfig")]
        public int SupportPowerConfiguration { get; set; }

        [JsonProperty("curPercent")]
        public int CurrentPercent { get; set; }

        [JsonProperty("switch")]
        public SwitchState @Switch { get; set; }

        [JsonProperty("curtainAction")]
        public CurtainAction Action { get; set; }

        [JsonProperty("motorDir")]
        public MotorDirection MotorDirection { get; set; }

        [JsonProperty("motorClb")]
        public string MotorClb { get; set; }

        [JsonProperty("openPercent")]
        public int OpenPercent { get; set; }
    }
}