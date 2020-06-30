using DTN.Widgets.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.Mvc;

namespace DTN.WidgetsDTNWidgets.Controllers
{
    public class WeatherController : Controller
    {
        // GET: Weather
        public ActionResult LocalWeather()
        {                       
            
            return View();
        }
    }
}