using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace USSD.App.Models
{
    public class AdapterRequest
    {
        [JsonProperty("Stage")]
        public int Stage { get; set; }

        [JsonProperty("SessionID")]
        public string SessionID { get; set; }

        [JsonProperty("USSDString")]
        public string USSDString { get; set; }

        [JsonProperty("ScreenID")]
        public int ScreenID { get; set; }

        [JsonProperty("PhoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("ServiceProvider")]
        public string ServiceProvider { get; set; }

        [JsonProperty("Tag")]
        public string Tag { get; set; }
    }
}
