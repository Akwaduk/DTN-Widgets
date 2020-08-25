using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DTN.Widgets.Models
{

    public class CashBid
    {
        public Accountlocation accountLocation { get; set; }
        public bool allowTransactions { get; set; }
        public decimal basisPrice { get; set; }
        public decimal cashPrice { get; set; }
        public string commodityDisplayName { get; set; }
        public string contractDeliveryLabel { get; set; }
        public string contractMonthCode { get; set; }
        public string currency { get; set; }
        public Deliveryperiod deliveryPeriod { get; set; }
        public string futuresChange { get; set; }
        public string futuresQuote { get; set; }
        public int id { get; set; }
        public Location location { get; set; }
        public bool realTime { get; set; }
        public string settlePrice { get; set; }
        public string symbol { get; set; }
        public string unitOfMeasure { get; set; }
    }

    public class Accountlocation
    {
        public int[] grainBidElevatorIds { get; set; }
        public Links links { get; set; }
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Deliveryperiod
    {
        public DateTime end { get; set; }
        public DateTime start { get; set; }
    }

    public class Links1
    {
    }

}