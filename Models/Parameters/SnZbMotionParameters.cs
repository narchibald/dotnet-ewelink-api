using System;
using System.Collections.Generic;
using System.Text;

namespace EWeLink.Api.Models.Parameters
{
    using Newtonsoft.Json;

    public class SnZbMotionParameters : SnZbParameters
    {
        [JsonProperty("motion")]
        public int Motion { get; set; }
    }
}
