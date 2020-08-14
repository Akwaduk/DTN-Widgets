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

using DotNetNuke.Web.Mvc.Framework.Controllers;
using DotNetNuke.Collections;
using System.Web.Mvc;
using DotNetNuke.Security;
using DotNetNuke.Web.Mvc.Framework.ActionFilters;
using DotNetNuke.Services.Log.EventLog;
using System.Linq.Expressions;
using System;
using Newtonsoft.Json;
using DTN.Widgets.Models;
using DotNetNuke.Entities.Portals;
using System.Linq;
using DotNetNuke.Common.Utilities;
using System.Collections.Generic;
using DTN.Widgets.Services;

namespace DTN.Widgets.Controllers
{
    [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
    [DnnHandleError]
    public class SettingsController : DnnController
    {
        [HttpGet]
        public ActionResult Weather()
        {
            var settings = new Models.LocalWeatherSettings();

            return View(settings);
        }

        [HttpPost]
        [ValidateInput(false)]
        [DotNetNuke.Web.Mvc.Framework.ActionFilters.ValidateAntiForgeryToken]
        public ActionResult LocalWeather(Models.LocalWeatherSettings settings)
        {

            return RedirectToDefaultRoute();
        }

        [HttpGet]
        public ActionResult News()
        {
            var settings = new Models.NewsSettings();
            settings.Setting1 = ModuleContext.Configuration.ModuleSettings.GetValueOrDefault("DTNWidgets_Setting1", false);
            settings.Setting2 = ModuleContext.Configuration.ModuleSettings.GetValueOrDefault("DTNWidgets_Setting2", System.DateTime.Now);

            return View(settings);
        }

        [HttpPost]
        [ValidateInput(false)]
        [DotNetNuke.Web.Mvc.Framework.ActionFilters.ValidateAntiForgeryToken]
        public ActionResult News(Models.NewsSettings settings)
        {
            ModuleContext.Configuration.ModuleSettings["DTNWidgets_Setting1"] = settings.Setting1.ToString();
            ModuleContext.Configuration.ModuleSettings["DTNWidgets_Setting2"] = settings.Setting2.ToUniversalTime().ToString("u");

            return RedirectToDefaultRoute();
        }

        [HttpGet]
        public ActionResult CashBidsTable()
        {
            try
            {
                var settings = new Models.CashBidsSettings();

                var nonDeserializedSettings = ModuleContext.Configuration.ModuleSettings.GetValueOrDefault("CashBidsTableSettings", "");                
                if (nonDeserializedSettings != "")
                {
                    settings = JsonConvert.DeserializeObject<CashBidsSettings>(nonDeserializedSettings);
                }

                if (settings.visibleFields.Count == 0)
                {
                    settings.visibleFields = new List<VisibleField>()
                    {
                        new VisibleField{FieldName = "BASIS_PRICE",   IsChecked = true, Order = 0},
                        new VisibleField{FieldName = "CASH_PRICE",    IsChecked = true, Order = 1 },
                        new VisibleField{FieldName = "DELIVERY_END",  IsChecked = true, Order = 2 },
                        new VisibleField{FieldName = "DELIVERY_START",IsChecked = true, Order = 3 },
                        new VisibleField{FieldName = "FUTURES_CHANGE",IsChecked = true, Order = 4 },
                        new VisibleField{FieldName = "FUTURES_QUOTE", IsChecked = true, Order = 5 },
                        new VisibleField{FieldName = "SETTLE_PRICE",  IsChecked = true, Order = 6 },
                        new VisibleField{FieldName = "SYMBOL",        IsChecked = true, Order = 7 },
                        new VisibleField{FieldName = "UNIT_OF_MEASURE", IsChecked = true, Order = 8 }
                    };
                }

                settings.visibleFields = settings.visibleFields.OrderBy(x => x.Order).ToList();

                // Get portal settings
                var portalController = new PortalController();
                var portalID = PortalController.Instance.GetCurrentPortalSettings().PortalId;
                var PortalSettings = PortalController.Instance.GetPortalSettings(portalID);

                // Set portal settings after deserilization
                settings.WebCashBidsAPIKey = PortalSettings.ContainsKey("WebCashBidsAPIKey") ? PortalSettings["WebCashBidsAPIKey"] : "";
                settings.ServerCashBidsAPI = PortalSettings.ContainsKey("ServerCashBidsAPI") ? PortalSettings["ServerCashBidsAPI"] : "";
                settings.siteId = PortalSettings.ContainsKey("SiteID") ? PortalSettings["SiteID"] : "";                

                if (string.IsNullOrEmpty(settings.ServerCashBidsAPI) == false && string.IsNullOrEmpty(settings.siteId) == false)
                {
                    var objEventLog = new EventLogController();                    
                    var DTNAPIService = new CashBidAPIService();                    
                    var allowedCommodities = DTNAPIService.GetSiteCommoditiesFromAPI(settings.siteId).Result;                    
                    var allowedLocations = DTNAPIService.GetSiteLocationsFromAPI(settings.siteId).Result;

                    objEventLog.AddLog("Cash Bids Breadcrumbs", "Setting commodity checkboxes", EventLogController.EventLogType.ADMIN_ALERT);
                    settings.hiddenCommodityCheckboxes = SetCommodityCheckboxes(allowedCommodities, settings);                    
                    settings.hiddenLocationCheckboxes = SetLocationCheckboxes(allowedLocations, settings);                    
                }
                
                return View(settings);
            }catch (Exception ex) {
                var objEventLog = new EventLogController();
                objEventLog.AddLog("Cash Bids Table Exception", ex.ToString(), EventLogController.EventLogType.ADMIN_ALERT);
                return RedirectToDefaultRoute();
            }
        }

        private List<HiddenCommodity> SetCommodityCheckboxes(List<Commodity> allowedCommodities, CashBidsSettings cashBidsSettings)
        {
            List<HiddenCommodity> HiddenCommodityCheckboxes = new List<HiddenCommodity>();

            foreach (var commodity in allowedCommodities)
            {
                var CommodityCheckbox = new HiddenCommodity();
                CommodityCheckbox.Commodity = commodity;
                if (cashBidsSettings.hideCommodities.Contains(commodity.commodityName.Trim()))
                {
                    CommodityCheckbox.IsChecked = true;
                }
                else
                {
                    CommodityCheckbox.IsChecked = false;
                }

                HiddenCommodityCheckboxes.Add(CommodityCheckbox);                
            }

            return HiddenCommodityCheckboxes;
        }

        private List<HiddenLocation> SetLocationCheckboxes(List<Location> allowedLocations, CashBidsSettings cashBidsSettings)
        {
            List<HiddenLocation> HiddenLocationCheckboxes = new List<HiddenLocation>();

            foreach (var location in allowedLocations)
            {
                var LocationCheckbox = new HiddenLocation();
                LocationCheckbox.Location = location;

                if (cashBidsSettings.hideLocations.Contains(location.name.Trim()))
                {
                    LocationCheckbox.IsChecked = true;
                }
                else
                {
                    LocationCheckbox.IsChecked = false;
                }

                HiddenLocationCheckboxes.Add(LocationCheckbox);
            }

            return HiddenLocationCheckboxes;
        }

        private List<string> GetHiddenLocationStrings(List<HiddenLocation> selectedLocations)
        {
            List<string> Locations = new List<string>();

            foreach (var location in selectedLocations)
            {
                if (location.IsChecked)
                    Locations.Add(location.Location.name.Trim());
            }

            return Locations;
        }

        private List<string> GetHiddenCommodityStrings(List<HiddenCommodity> selectedCommodities)
        {
            List<string> Commodities = new List<string>();

            foreach (var Commodity in selectedCommodities)
            {
                if (Commodity.IsChecked)
                    Commodities.Add(Commodity.Commodity.commodityName.Trim());
            }

            return Commodities;
        }


        [HttpPost]
        [ValidateInput(false)]
        [DotNetNuke.Web.Mvc.Framework.ActionFilters.ValidateAntiForgeryToken]
        public ActionResult CashBidsTable(Models.CashBidsSettings settings)
        {
            // Get portal settings
            var portalID = PortalController.Instance.GetCurrentPortalSettings().PortalId;
            settings.hideCommodities = GetHiddenCommodityStrings(settings.hiddenCommodityCheckboxes);
            settings.hideLocations = GetHiddenLocationStrings(settings.hiddenLocationCheckboxes);

            PortalController.Instance.UpdatePortalSetting(portalID, "WebCashBidsAPIKey", settings.WebCashBidsAPIKey, true, "en-US", false);
            PortalController.Instance.UpdatePortalSetting(portalID, "ServerCashBidsAPI", settings.ServerCashBidsAPI, true, "en-US", false);
            PortalController.Instance.UpdatePortalSetting(portalID, "SiteID", settings.siteId, true, "en-US", false);

            settings.WebCashBidsAPIKey = "";
            settings.siteId = "";
            settings.ServerCashBidsAPI = "";

            ModuleContext.Configuration.ModuleSettings["CashBidsTableSettings"] = JsonConvert.SerializeObject(settings);
            return RedirectToDefaultRoute();
        }

        [HttpGet]
        public ActionResult Futures()
        {
            var settings = new Models.FuturesSettings();
            settings.Setting1 = ModuleContext.Configuration.ModuleSettings.GetValueOrDefault("DTNWidgets_Setting1", false);
            settings.Setting2 = ModuleContext.Configuration.ModuleSettings.GetValueOrDefault("DTNWidgets_Setting2", System.DateTime.Now);

            return View(settings);
        }

        [HttpPost]
        [ValidateInput(false)]
        [DotNetNuke.Web.Mvc.Framework.ActionFilters.ValidateAntiForgeryToken]
        public ActionResult Futures(Models.FuturesSettings settings)
        {
            ModuleContext.Configuration.ModuleSettings["DTNWidgets_Setting1"] = settings.Setting1.ToString();
            ModuleContext.Configuration.ModuleSettings["DTNWidgets_Setting2"] = settings.Setting2.ToUniversalTime().ToString("u");

            return RedirectToDefaultRoute();
        }
    }
}