using System.Web;
using System.Web.Optimization;

namespace WebApp_OpenIDConnect_DotNet_B2C
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/angular.min.js",
                      "~/Scripts/toastr.min.js",
                      "~/Scripts/moment.min.js",
                      "~/Scripts/angular-moment.min.js",
                      "~/Scripts/select2.min.js",
                      "~/Scripts/app/app.js"));

            bundles.Add(new StyleBundle("~/Content/appcss").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/toastr.min.css",
                      "~/css/font-awesome.min.css",
                      "~/Content/css/select2.min.css",
                      "~/Content/site.css"));
        }
    }
}
