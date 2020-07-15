using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DTN.Widgets.Models
{
    public class InteractiveWeatherSettings
    {
        public string apiKey { get; set; }
        public string container { get; set; }
        public string defaultLayers { get; set; }            
    }
}