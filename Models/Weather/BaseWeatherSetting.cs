using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DTN.WidgetsDTNWidgets.Models.Weather
{
    [Serializable]
    public class BaseWeatherSetting
    {
        public Defaultlocation defaultLocation { get; set; }
    }

    [Serializable]
    public class Defaultlocation
    {
        public string postalCode { get; set; }
        public string zoom { get; set; }
    }
}