/*
' Copyright (c) 2020 Akwaduk
'  All rights reserved.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' 
*/

using System;
using System.Collections.Generic;
using System.Web.Caching;
using DotNetNuke.Common.Utilities;
using DotNetNuke.ComponentModel.DataAnnotations;
using DotNetNuke.Entities.Content;

namespace DTN.Widgets.Models
{
    [Serializable]
    public class CashBidsSettings : DTNModuleBase
    {        
        public string container { get; set; }
        public string siteId { get; set; }
        public string groupBy { get; set; } = "LOCATION"; // LOCATION || COMMODITY
        public bool showGroupByOptions { get; set; } = true;
        public string view { get; set; } = "DETAIL"; // PRICE_MATRIX || DETAIL
        public bool showViewOptions { get; set; } = true;
        public bool showLocationsSelect { get; set; } = true;
        public bool showCommoditiesSelect { get; set; } = true;
        public Location defaultLocation { get; set; }
        public List<string> hideCommodities { get; set; } = new List<string>();
        public List<string> hideLocations { get; set; } = new List<string>();
        public List<string> visibleFields { get; set; } = new List<string>()
        {
            "BASIS_PRICE",
            "CASH_PRICE",
            "DELIVERY_END",
            "DELIVERY_START",
            "FUTURES_CHANGE",
            "FUTURES_QUOTE",
            "SETTLE_PRICE",
            "SYMBOL",
            "UNIT_OF_MEASURE"
        };
    }

}
