using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace EWeLink.Api.Models.Parameters
{
    public class ZbCurtainParameters : SnZbParameters
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

    public enum MotorDirection
    {
        [EnumMember(Value = "forward")] 
        Forward,
        [EnumMember(Value = "backward")]
        Backward,
    }

    public enum CurtainAction
    {
        [EnumMember(Value = "close")]
        Close,
        
        [EnumMember(Value = "open")] 
        Open,
        
        [EnumMember(Value = "stop")]
        Stop,
    }
}