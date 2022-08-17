using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace USSD.App.Settings
{
    public class USSDSettings : IUSSDSettings
    {
        private readonly IConfiguration _configuration;

        public USSDSettings(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Port
        {
            get
            {
                return this._configuration["USSDSettings:Port"];
            }
        }
        public string Address
        {
            get
            {
                return this._configuration["USSDSettings:Address"];
            }
        }
    }
}
