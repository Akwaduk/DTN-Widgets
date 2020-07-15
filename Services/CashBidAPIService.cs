using DotNetNuke.Entities.Portals;
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
        private string ServerAPIKey
        {
            get
            {
                var portalController = new PortalController();
                var portalID = PortalController.Instance.GetCurrentPortalSettings().PortalId;
                var PortalSettings = PortalController.Instance.GetPortalSettings(portalID);
                return PortalSettings["ServerAPIKey"];
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

        private string baseURL = "https://api.dtn.com/";

        public async Task<List<Location>> GetSiteLocationsFromAPI(int siteID)
        {
            var locationsURL = baseURL + "/markets/sites/" + siteID + "/locations";
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("apikey", ServerAPIKey);
            
            try
            {
                HttpResponseMessage response = await client.GetAsync(locationsURL);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                var LocationsJSONList = JsonConvert.DeserializeObject<List<Location>>(responseBody);

                return LocationsJSONList;
            } catch (Exception ex)
            {
                var objEventLog = new EventLogController();
                objEventLog.AddLog("Could not connect to locations market API;", ex.ToString(), EventLogController.EventLogType.ADMIN_ALERT);
            }
            return new List<Location>();
        }

        public async Task<List<Commodity>> GetSiteCommoditiesFromAPI(int siteID)
        {
            var CommoditysURL = baseURL + "/markets/sites/" + siteID + "/commodities";
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("apikey", ServerAPIKey);
            
            try
            {
                HttpResponseMessage response = await client.GetAsync(CommoditysURL);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                var CommoditysJSONList = JsonConvert.DeserializeObject<List<Commodity>>(responseBody);

                return CommoditysJSONList;
            }
            catch (Exception ex)
            {
                var objEventLog = new EventLogController();
                objEventLog.AddLog("Could not connect to commodities market API;", ex.ToString(), EventLogController.EventLogType.ADMIN_ALERT);
            }
            return new List<Commodity>();
        }
    }
}