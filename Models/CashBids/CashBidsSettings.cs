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
using System.ComponentModel;
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
        [DisplayName("Site (e) ID")]
        public string siteId { get; set; }
        [DisplayName("Group Table By")]
        public string groupBy { get; set; } = "LOCATION"; // LOCATION || COMMODITY
        [DisplayName("Show Group By Options")]
        public bool showGroupByOptions { get; set; } = true;
        [DisplayName("Table View")]
        public string view { get; set; } = "DETAIL"; // PRICE_MATRIX || DETAIL
        [DisplayName("Show View Options")]
        public bool showViewOptions { get; set; } = true;
        [DisplayName("Show Locations Select")]
        public bool showLocationsSelect { get; set; } = true;
        [DisplayName("Show Commodities Select")]
        public bool showCommoditiesSelect { get; set; } = true;
        [DisplayName("Default Location")]
        public Location defaultLocation { get; set; }
        [DisplayName("Hide Commodities")]
        public List<string> hideCommodities { get; set; } = new List<string>();
        [DisplayName("Hide Locations")]
        public List<string> hideLocations { get; set; } = new List<string>();
        [DisplayName("Visible Fields")]
        public List<VisibleField> visibleFields { get; set; } = new List<VisibleField>();
    }

    public class VisibleField
    {
        public string FieldName { get; set; }
        public bool IsChecked { get; set; }
        public int Order { get; set; }
    }

    public enum CashBidView
    {
        DETAIL,
        PRICE_MATRIX
    }

    public enum GroupViewBy
    {
        LOCATION,
        COMMODITY
    }

}
