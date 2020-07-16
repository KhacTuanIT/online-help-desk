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
                        "~/Scripts/AppJavascript/jquery/jquery.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/AppJavaScript/jquery-validation/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/AppJavaScript/moderniz/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/AppJavaScript/bootstrap/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/AppStyleSheet/bootstrap/bootstrap.css",
                      "~/Content/AppStyleSheet/css/font-awesome.css",
                      "~/Content/AppStyleSheet/datatables/datatables.min.css",
                      "~/Content/AppStyleSheet/datatables/orionicons.css"));

            bundles.Add(new StyleBundle("~/Content/login").Include(
                      "~/Content/AppStyleSheet/css/login.css"));
            bundles.Add(new ScriptBundle("~/bundles/datatables").Include(
                      "~/Scripts/AppJavaScript/datatables/datatables.min.js"));

            bundles.Add(new StyleBundle("~/Content/stylesheet").Include(
                      "~/Content/AppStyleSheet/bootstrap/bootstrap.min.css",
                      "~/Content/AppStyleSheet/css/orionicons.css",
                      "~/Content/AppStyleSheet/css/toastr.css"));
            bundles.Add(new ScriptBundle("~/bundles/javascript").Include(
                      "~/Scripts/AppJavaScript/jquery/jquery.min.js",
                      "~/Scripts/AppJavaScript/popper/umd/popper.min.js",
                      "~/Scripts/AppJavaScript/bootstrap/bootstrap.min.js",
                      "~/Scripts/AppJavaScript/jquery.cookie/jquery.cookie.js",
                      "~/Scripts/AppJavaScript/js/front.js",
                      "~/Scripts/AppJavaScript/js/toastr.js"));
            bundles.Add(new ScriptBundle("~/bundles/chart").Include(
                      "~/Scripts/AppJavaScript/chart/Chart.min.js",
                      "~/Scripts/AppJavaScript/js/charts-home.js"));
            bundles.Add(new StyleBundle("~/Content/modal").Include(
                      "~/Content/AppStyleSheet/css/modal.css"));
            bundles.Add(new ScriptBundle("~/bundles/notification").Include(
                      "~/Scripts/AppJavaScript/js/notification-custom.js",
                      "~/Scripts/AppJavaScript/js/cal-custom.js"));
            bundles.Add(new ScriptBundle("~/bundles/newrequest").Include(
                      "~/Scripts/AppJavaScript/js/create-new-request.js"));
            bundles.Add(new ScriptBundle("~/bundles/toast-custom").Include(
                      "~/Scripts/AppJavaScript/js/toastr-custom.js"));
            bundles.Add(new StyleBundle("~/Content/status-request").Include(
                      "~/Content/AppStyleSheet/css/assign-custom.css"));
        }
    }
}
