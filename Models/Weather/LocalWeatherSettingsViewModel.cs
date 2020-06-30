using DotNetNuke.Entities.Portals;
using DTN.Widgets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DTN.Widgets.Models
{
    public class LocalWeatherSettingsViewModel
    {
        public LocalWeatherSettings WeatherSettings { get; set; }
        public PortalSettings PortalSettings { get; set; }
    }
}