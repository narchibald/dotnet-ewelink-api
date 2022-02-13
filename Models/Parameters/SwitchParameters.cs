﻿namespace EWeLink.Api.Models.Parameters
{
    using Newtonsoft.Json;

    public class SwitchParameters : Paramaters
    {
        [JsonProperty("pulse")]
        public string? Pulse { get; set; }

        [JsonProperty("pulseWidth")]
        public int PulseWidth { get; set; }

        [JsonProperty("switch")]
        public SwitchState Switch { get; set; }

        public override dynamic CreateParameters()
        {
            var parameters = base.CreateParameters();
            parameters.@switch = Switch;
            return parameters;
        }
    }
}