using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace USSD.App.Models
{
    public class EngineRequest
    {
        [JsonProperty("SessionID")]
        public string SessionID { get; set; }

        [JsonProperty("ResponseRequired")]
        public bool ResponseRequired { get; set; }

        [JsonProperty("ScreenID")]
        public int ScreenID { get; set; }

        [JsonProperty("Message")]
        public string Message { get; set; }
    }
}
