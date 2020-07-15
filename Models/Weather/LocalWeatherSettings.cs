using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DTN.Widgets.Models
{
    [Serializable]
    public class LocalWeatherSettings : BaseWeatherSetting
    {
        public string apiKey { get; set; }
        public string container { get; set; }
        public string units { get; set; }
        public Station[] stations { get; set; }
        public bool showStationsSelect { get; set; }
        public bool showPostalCodeInput { get; set; }
        public bool showForecast { get; set; }
        public bool showCurrentConditions { get; set; }
        public bool showWeatherDetails { get; set; }
        public Callbacks callbacks { get; set; }
    }

    public class Callbacks
    {
        public string onStationChange { get; set; }
        public string onPostalCodeChange { get; set; }
        public string onWeatherChange { get; set; }
    }

    public class Station
    {
        public string id { get; set; }
        public string displayName { get; set; }
    }
}