using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Collections;
using System.Configuration;

namespace informix.Web
{

    public class RouteConfig
    {

        public static void RegisterRoutes(RouteCollection routes)
        {
            log4net.Config.XmlConfigurator.Configure();
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }
            );
        }
    }
}
