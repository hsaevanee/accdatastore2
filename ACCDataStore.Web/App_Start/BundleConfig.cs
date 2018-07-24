using System.Web;
using System.Web.Optimization;


namespace ACCDataStore
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        //"~/Scripts/jquery.js",
                        "~/Scripts/jquery-2.1.1.min.js",
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/angularjs").Include(
                        "~/bower_components/angular/angular.min.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      //"~/bower_components/bootstrap/dist/js/bootstrap.min.js",
                      "~/Scripts/bootstrap-multiselect",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/highcharts").Include(
                      "~/bower_components/highcharts/highcharts.js",
                      "~/bower_components/highcharts-ng/dist/highcharts-ng.min.js",
                      "~/bower_components/highcharts/highcharts-3d.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
                     // "~/Content/bootstrap.min.css"));
                     // "~/Content/modern-business.css"));
                      //"~/Content/heroic-features.css"));
        }
    }
}