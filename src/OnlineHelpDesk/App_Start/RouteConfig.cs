using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace OnlineHelpDesk
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //routes.IgnoreRoute("Account/{*pathInfo}");
            //routes.Ignore("Home/{*pathInfo}");
            //routes.Ignore("{controller}/Index");

            routes.MapRoute(
                name: "Login",
                url: "Login",
                defaults: new { controller = "Account", action = "Login" });

            //routes.MapRoute(
            //    name: "IgnoreHome",
            //    url: "Home",
            //    defaults: new { controller = "Error", action = "NotFound" });

            //routes.MapRoute(
            //    name: "IgnoreIndex",
            //    url: "{controllerName}/Index/{*pathInfo}",
            //    defaults: new { controller = "Error", action = "NotFound" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });

        }
    }
}
