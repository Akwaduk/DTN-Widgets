using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DTN.Widgets.Models
{

    public class Location
    {
        public int[] grainBidElevatorIds { get; set; }
        public Links links { get; set; }
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Links
    {
    }

}