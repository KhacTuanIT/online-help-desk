using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

            routes.MapRoute(
                name: "Profile",
                url: "{user}",
                defaults: new { controller = "User", action = "Index" },
                constraints: new { user = new VanityUrlContraint() });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });

            //routes.MapRoute(
            //    name: "IgnoreHome",
            //    url: "Home",
            //    defaults: new { controller = "Error", action = "NotFound" });

            //routes.MapRoute(
            //    name: "IgnoreIndex",
            //    url: "{controllerName}/Index/{*pathInfo}",
            //    defaults: new { controller = "Error", action = "NotFound" });

        }
    }

    // Thanks to @shakib
    // https://stackoverflow.com/questions/13558541/what-kind-of-route-would-i-need-to-provide-vanity-urls
    public class VanityUrlContraint : IRouteConstraint
    {
        private static readonly string[] Controllers =
            Assembly.GetExecutingAssembly().GetTypes().Where(x => typeof(IController).IsAssignableFrom(x))
                .Select(x => x.Name.ToLower().Replace("controller", "")).ToArray();

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values,
                          RouteDirection routeDirection)
        {
            return !Controllers.Contains(values[parameterName].ToString().ToLower());
        }
    }
}
