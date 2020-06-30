using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace DTN.Widgets.Models
{
    public class DTNModuleBase
    {
        [DisplayName("API Key")]
        public string apiKey { get; set; }
    }
}