using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace DTN.Widgets.Models
{
    public class DTNModuleBase
    {
        [DisplayName("Web Cash Bids API Key")]
        public string WebCashBidsAPIKey { get; set; }

        [DisplayName("Server Cash Bids API Key")]
        public string ServerCashBidsAPI { get; set; }
    }
}