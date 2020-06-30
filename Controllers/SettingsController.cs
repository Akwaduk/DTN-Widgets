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

                // Get portal settings
                var portalController = new PortalController();
                var portalID = PortalController.Instance.GetCurrentPortalSettings().PortalId;
                var PortalSettings = PortalController.Instance.GetPortalSettings(portalID);

                // Set portal settings after deserilization
                settings.apiKey = PortalSettings.ContainsKey("WebAPIKey") ? PortalSettings["WebAPIKey"] : "";
                settings.siteId = PortalSettings.ContainsKey("SiteID") ? PortalSettings["SiteID"] : "";

                return View(settings);
            }catch (Exception ex) {
                var objEventLog = new EventLogController();
                objEventLog.AddLog("Cash Bids Table Exception", ex.ToString(), EventLogController.EventLogType.ADMIN_ALERT);
                return RedirectToDefaultRoute();
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        [DotNetNuke.Web.Mvc.Framework.ActionFilters.ValidateAntiForgeryToken]
        public ActionResult CashBidsTable(Models.CashBidsSettings settings)
        {
            // Get portal settings
            var portalController = new PortalController();
            var portalID = PortalController.Instance.GetCurrentPortalSettings().PortalId;
            var PortalSettings = PortalController.Instance.GetPortalSettings(portalID);

            PortalController.Instance.UpdatePortalSetting(portalID, "WebAPIKey", settings.apiKey, true, "en-US");
            PortalController.Instance.UpdatePortalSetting(portalID, "SiteID", settings.siteId, true, "en-US");

            settings.apiKey = "";
            settings.siteId = "";

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