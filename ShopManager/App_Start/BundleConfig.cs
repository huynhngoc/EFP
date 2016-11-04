using System.Web;
using System.Web.Optimization;

namespace ShopManager
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
                      "~/Content/font-awesome/css/font-awesome.css"));
            //Animate and style CSS
            bundles.Add(new StyleBundle("~/Animate-and-style-CSS").Include(
                      "~/Content/css/animate.css",
                      "~/Content/css/style.css"));
            //awesome - bootstrap - checkbox CSS
            bundles.Add(new StyleBundle("~/Awesome-bootstrap-checkbox-CSS").Include(
                      "~/Content/css/plugins/awesome-bootstrap-checkbox/awesome-bootstrap-checkbox.css"));
            //Toastr style
            bundles.Add(new StyleBundle("~/Toastr-style").Include(
                      "~/Content/css/plugins/toastr/toastr.min.css"));
            //Gritter
            bundles.Add(new StyleBundle("~/Gritter").Include(
                      "~/Content/js/plugins/gritter/jquery.gritter.css"));
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
                      "~/Content/js/inspinia.js",
                      "~/Content/js/plugins/pace/pace.min.js"));
            //jQuery UI
            bundles.Add(new ScriptBundle("~/jQuery-UI").Include(
                      "~/Content/js/plugins/jquery-ui/jquery-ui.min.js"));
            bundles.Add(new StyleBundle("~/jQuery-UI-CSS").Include(
                      "~/Content/css/plugins/jQueryUI/jquery-ui.css",
                      "~/Content/css/plugins/jQueryUI/jquery-ui-1.10.4.custom.min.css"));
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
            //SweetAlert CSS
            bundles.Add(new StyleBundle("~/SweetAlert-CSS").Include(
                      "~/Content/css/plugins/sweetalert/sweetalert.css"));
            //SweetAlert JS
            bundles.Add(new ScriptBundle("~/SweetAlert-JS").Include(
                      "~/Content/js/plugins/sweetalert/sweetalert.min.js"));
            //TouchSpin CSS
            bundles.Add(new StyleBundle("~/TouchSpin-CSS").Include(
                      "~/Content/css/plugins/touchspin/jquery.bootstrap-touchspin.min.css"));
            //TouchSpin JS
            bundles.Add(new ScriptBundle("~/TouchSpin-JS").Include(
                      "~/Content/js/plugins/touchspin/jquery.bootstrap-touchspin.min.js"));

            //SummerNote CSS
            bundles.Add(new StyleBundle("~/SummerNote-CSS").Include(
                      "~/Content/css/plugins/summernote/summernote.css"));
            //SummerNote JS
            bundles.Add(new ScriptBundle("~/SummerNote-JS").Include(
                "~/Content/js/plugins/summernote/summernote.min.js"));
            //tokenfield CSS
            bundles.Add(new StyleBundle("~/tokenfield-CSS").Include(
                      "~/Content/css/plugins/tokenfield/bootstrap-tokenfield.css"));
            //tokenfield JS
            bundles.Add(new ScriptBundle("~/tokenfield-JS").Include(
                "~/Content/js/plugins/tokenfield/bootstrap-tokenfield.min.js"));
            //Switchery CSS
            bundles.Add(new StyleBundle("~/Switchery-CSS").Include(
                      "~/Content/css/plugins/switchery/switchery.css"));
            //Switchery JS
            bundles.Add(new ScriptBundle("~/Switchery-JS").Include(
                "~/Content/js/plugins/switchery/switchery.js"));
            //slick carousel CSS
            bundles.Add(new StyleBundle("~/slick-carousel-CSS").Include(
                      "~/Content/css/plugins/slick/slick.css",
                      "~/Content/css/plugins/slick/slick-theme.css"));
            //slick carousel JS
            bundles.Add(new ScriptBundle("~/slick-carousel-JS").Include(
                "~/Content/js/plugins/slick/slick.min.js"));
            //Image cropper CSS
            bundles.Add(new StyleBundle("~/Image-cropper-CSS").Include(
                      "~/Content/css/plugins/cropper/cropper.min.css"));
            //Image cropper JS
            bundles.Add(new ScriptBundle("~/Image-cropper-JS").Include(
                "~/Content/js/plugins/cropper/cropper.min.js"));
            //Dropzone CSS
            bundles.Add(new StyleBundle("~/DropZone-CSS").Include(
                "~/Content/css/plugins/dropzone/basic.css",
                "~/Content/css/plugins/dropzone/dropzone.css"));
            //Dropzone JS
            bundles.Add(new ScriptBundle("~/DropZone-JS").Include(
                "~/Content/js/plugins/dropzone/dropzone.js"));
            //nestable js
            bundles.Add(new ScriptBundle("~/Nestable-JS").Include(
                "~/Content/js/plugins/nestable/jquery.nestable.js"));
            //select2 CSS
            bundles.Add(new StyleBundle("~/Select2-CSS").Include(
                "~/Content/css/plugins/select2/select2.min.css"));
            //select2 JS 
            bundles.Add(new ScriptBundle("~/Select2-JS").Include(
                "~/Content/js/plugins/select2/select2.full.min.js"));
            //select2 JS 
            bundles.Add(new ScriptBundle("~/Customer-JS").Include(
                "~/Content/js/Customer_js/Customer_js.js"));

            //Utility JS 
            bundles.Add(new ScriptBundle("~/Utility-JS").Include(
                "~/Content/js/CustomJS/Utility.js"));
            //ChatComment CSS
            bundles.Add(new StyleBundle("~/ChatComment-CSS").Include(
                "~/Content/css/CustomCSS/ChatComment.css"));

            //signalR
            bundles.Add(new ScriptBundle("~/SignalR-JS").Include(
                "~/Scripts/jquery.signalR-2.2.1.js",
                "~/Scripts/jquery.signalR-2.2.1.min.js"));
            //Data picker JS
            bundles.Add(new ScriptBundle("~/Data-picker-JS").Include(
                "~/Content/js/plugins/datapicker/bootstrap-datepicker.js"));
            //Data picker CSS
            bundles.Add(new StyleBundle("~/Data-picker-CSS").Include(
                "~/Content/css/plugins/datapicker/datepicker3.css"));
            //iCheck CSS
            bundles.Add(new StyleBundle("~/iCheck-CSS").Include(
                      "~/Content/css/plugins/iCheck/custom.css"));
            //iCheck JS
            bundles.Add(new ScriptBundle("~/iCheck-JS").Include(
                      "~/Content/js/plugins/iCheck/icheck.min.js"));
        }
    }
}
