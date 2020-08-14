﻿using DotNetNuke.Entities.Portals;
using DotNetNuke.Services.Log.EventLog;
using DTN.Widgets.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Services.Protocols;

namespace DTN.Widgets.Services
{
    public class CashBidAPIService
    {
        private string ServerCashBidsAPI
        {
            get
            {
                var portalController = new PortalController();
                var portalID = PortalController.Instance.GetCurrentPortalSettings().PortalId;
                var PortalSettings = PortalController.Instance.GetPortalSettings(portalID);
                return PortalSettings["ServerCashBidsAPI"];
            }            
        }

        private string SiteID
        {
            get
            {
                var portalController = new PortalController();
                var portalID = PortalController.Instance.GetCurrentPortalSettings().PortalId;
                var PortalSettings = PortalController.Instance.GetPortalSettings(portalID);
                return PortalSettings["SiteID"];
            }
        }

        private string baseURL = "https://api.dtn.com";

        public async Task<List<Location>> GetSiteLocationsFromAPI(string siteID)
        {
            var locationsURL = baseURL + "/markets/sites/" + siteID + "/locations";
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("apikey", ServerCashBidsAPI);
            
            try
            {
                HttpResponseMessage response = await client.GetAsync(locationsURL).ConfigureAwait(false);
                string responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                var LocationsJSONList = JsonConvert.DeserializeObject<List<Location>>(responseBody);

                return LocationsJSONList;
            } catch (Exception ex)
            {
                var objEventLog = new EventLogController();
                objEventLog.AddLog("Could not connect to locations market API;", ex.ToString(), EventLogController.EventLogType.ADMIN_ALERT);
            }
            return new List<Location>();
        }

        public async Task<List<Commodity>> GetSiteCommoditiesFromAPI(string siteID)
        {
            var objEventLog = new EventLogController();

            var CommoditysURL = baseURL + "/markets/sites/" + siteID + "/commodities";
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("apikey", ServerCashBidsAPI);
            client.Timeout = TimeSpan.FromSeconds(30);

            try
            {
                objEventLog.AddLog("Cash Bids Breadcrumbs", "Getting Response for " + CommoditysURL, EventLogController.EventLogType.ADMIN_ALERT);
                HttpResponseMessage response = await client.GetAsync(CommoditysURL).ConfigureAwait(false);
                string responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);


                objEventLog.AddLog("Cash Bids Breadcrumbs", "Here: " + responseBody, EventLogController.EventLogType.ADMIN_ALERT);

                var CommoditysJSONList = JsonConvert.DeserializeObject<List<Commodity>>(responseBody);

                return CommoditysJSONList;
            }
            catch (Exception ex)
            {
                //var objEventLog = new EventLogController();
                objEventLog.AddLog("Could not connect to commodities market API;", ex.ToString(), EventLogController.EventLogType.ADMIN_ALERT);
            }
            return new List<Commodity>();
        }
    }
}