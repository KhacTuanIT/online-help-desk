using System.Web;
using System.Web.Optimization;

namespace OnlineHelpDesk
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/font-awesome.css",
                      "~/Content/datatables.min.css"));

            bundles.Add(new StyleBundle("~/Content/login").Include(
                      "~/Content/login.css"));
            bundles.Add(new ScriptBundle("~/bundles/custom").Include(
                      "~/Scripts/index_custom.js"));
            bundles.Add(new ScriptBundle("~/bundles/datatables").Include(
                      "~/Scripts/datatables.min.js"));

            bundles.Add(new StyleBundle("~/Content/stylesheet").Include(
                      "~/Content/AppStyleSheet/bootstrap/bootstrap.min.css",
                      "~/Content/AppStyleSheet/css/orionicons.css"));
            bundles.Add(new ScriptBundle("~/bundles/javascript").Include(
                      "~/Scripts/AppJavaScript/jquery/jquery.min.js",
                      "~/Scripts/AppJavaScript/popper/umd/popper.min.js",
                      "~/Scripts/AppJavaScript/bootstrap/bootstrap.min.js",
                      "~/Scripts/AppJavaScript/jquery.cookie/jquery.cookie.js",
                      "~/Scripts/AppJavaScript/js/front.js"));
            bundles.Add(new ScriptBundle("~/bundles/chart").Include(
                      "~/Scripts/AppJavaScript/chart/Chart.min.js",
                      "~/Scripts/AppJavaScript/js/charts-home.js"));
            bundles.Add(new StyleBundle("~/Content/modal").Include(
                      "~/Content/AppStyleSheet/css/modal.css"));
        }
    }
}
