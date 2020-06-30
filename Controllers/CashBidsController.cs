﻿/*
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
using Newtonsoft.Json;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Services.Log.EventLog;
using System;

namespace DTN.Widgets.Controllers
{
    [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
    [DnnHandleError]
    public class CashBidsController : DnnController
    {
        [HttpGet]
        public ActionResult Table()
        {
            try
            {
                // Get serilized settings
                var CashBidsSettings = new Models.CashBidsSettings();                
                CashBidsSettings = JsonConvert.DeserializeObject<Models.CashBidsSettings>(ModuleContext.Configuration.ModuleSettings.GetValueOrDefault("CashBidsTableSettings", ""));
                CashBidsSettings.defaultLocation = new Models.Location();

                // Get portal settings
                var portalController = new PortalController();
                var portalID = PortalController.Instance.GetCurrentPortalSettings().PortalId;
                var PortalSettings = PortalController.Instance.GetPortalSettings(portalID);

                CashBidsSettings.apiKey = PortalSettings.ContainsKey("WebAPIKey") ? PortalSettings["WebAPIKey"] : "";
                CashBidsSettings.siteId = PortalSettings.ContainsKey("SiteID") ? PortalSettings["SiteID"] : "";
                // Show table
                return View(CashBidsSettings);
            } catch (Exception ex) {
                var objEventLog = new EventLogController();
                objEventLog.AddLog("Cash Bids Table Exception", ex.ToString(), EventLogController.EventLogType.ADMIN_ALERT);

                var CashBidsSettings = new Models.CashBidsSettings();
                CashBidsSettings.defaultLocation = new Models.Location();
                return View(CashBidsSettings);
            }
        }
    }
}