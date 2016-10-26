using System.Web;
using System.Web.Optimization;

namespace ProductPage
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
            //Header CSS
            bundles.Add(new StyleBundle("~/Header-CSS").Include(
                      "~/Content/css/bootstrap.min.css",
                      "~/Content/font-awesome/css/font-awesome.css",
                      "~/Content/css/animate.css",
                      "~/Content/css/style.css"));
            //Toastr style
            bundles.Add(new StyleBundle("~/Toastr-style").Include(
                      "~/Content/css/plugins/toastr/toastr.min.css"));
            //Gritter
            bundles.Add(new StyleBundle("~/Gritter").Include(
                      "~/Content/js/plugins/gritter/jquery.gritter.css"));
            //Sweet Alert CSS
            bundles.Add(new StyleBundle("~/Sweet-Alert-CSS").Include(
                      "~/Content/css/plugins/sweetalert/sweetalert.css"));
            //Sweet Alert JS
            bundles.Add(new ScriptBundle("~/Sweet-Alert-JS").Include(
                      "~/Content/js/plugins/sweetalert/sweetalert.min.js"));
            //Mainly scripts
            bundles.Add(new ScriptBundle("~/Mainly-scripts").Include(
                      "~/Content/js/jquery-2.1.1.js",
                      "~/Content/js/bootstrap.min.js",
                      "~/Content/js/plugins/metisMenu/jquery.metisMenu.js",
                      "~/Content/js/plugins/slimscroll/jquery.slimscroll.min.js"));
            //Flot
            bundles.Add(new ScriptBundle("~/Flot").Include(
                      "~/Content/js/plugins/flot/jquery.flot.js",
                      "~/Content/js/plugins/flot/jquery.flot.tooltip.min.js",
                      "~/Content/js/plugins/flot/jquery.flot.spline.js",
                      "~/Content/js/plugins/flot/jquery.flot.resize.js",
                      "~/Content/js/plugins/flot/jquery.flot.symbol.js",
                      "~/Content/js/plugins/flot/jquery.flot.time.js",
                      "~/Content/js/plugins/flot/jquery.flot.pie.js"));
            //Peity
            bundles.Add(new ScriptBundle("~/Peity").Include(
                      "~/Content/js/plugins/peity/jquery.peity.min.js",
                      "~/Content/js/demo/peity-demo.js"));
            //Custom and plugin javascript
            bundles.Add(new ScriptBundle("~/Custom-and-plugin-javascript").Include(
                      "~/Content/js/inspinia.js"));
            //jQuery UI JS
            bundles.Add(new ScriptBundle("~/jQuery-UI-JS").Include(
                      "~/Content/js/plugins/jquery-ui/jquery-ui.min.js"));
            //jQuery UI CSS
            bundles.Add(new StyleBundle("~/jQuery-UI-CSS").Include(
                      "~/Content/js/plugins/jquery-UI/jquery-ui.min.css"));
            //GITTER
            bundles.Add(new ScriptBundle("~/GITTER").Include(
                      "~/Content/js/plugins/gritter/jquery.gritter.min.js"));
            //Sparkline
            bundles.Add(new ScriptBundle("~/Sparkline").Include(
                      "~/Content/js/plugins/sparkline/jquery.sparkline.min.js"));
            //ChartJS
            bundles.Add(new ScriptBundle("~/ChartJS").Include(
                      "~/Content/js/plugins/chartJs/Chart.min.js"));
            //Toastr
            bundles.Add(new ScriptBundle("~/Toastr").Include(
                      "~/Content/js/plugins/toastr/toastr.min.js"));
            //EasyPIE
            bundles.Add(new ScriptBundle("~/EasyPIE").Include(
                      "~/Content/js/plugins/easypiechart/jquery.easypiechart.js"));
            //DataTable CSS
            bundles.Add(new StyleBundle("~/DataTable-CSS").Include(
                      "~/Content/css/plugins/dataTables/datatables.min.css"));
            //DataTable JS
            bundles.Add(new ScriptBundle("~/DataTable-JS").Include(
                      "~/Content/js/plugins/dataTables/datatables.min.js"));
            //Slick CSS
            bundles.Add(new StyleBundle("~/Slick-CSS").Include(
                      "~/Content/css/plugins/slick/slick.css",
                      "~/Content/css/plugins/slick/slick-theme.css"));
            //Slick JS
            bundles.Add(new ScriptBundle("~/Slick-carousel").Include(
                      "~/Content/js/plugins/slick/slick.min.js"));
            //Home customjs
            bundles.Add(new ScriptBundle("~/Home-customjs").Include(
                      "~/Content/customjs/CustomJavaScript.js"));
            //TouchSpin
            bundles.Add(new ScriptBundle("~/TouchSpin-JS").Include(
                      "~/Content/js/plugins/touchspin/jquery.bootstrap-touchspin.min.js"));
            //Typeahead
            bundles.Add(new ScriptBundle("~/Typeahead-JS").Include(
                      "~/Content/js/plugins/typehead/bootstrap3-typeahead.min.js"));
        }
    }
}
