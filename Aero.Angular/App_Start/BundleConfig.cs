﻿using System.Web;
using System.Web.Optimization;

namespace Aero.Angular
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/jslib").Include(
                "~/Scripts/q.js",
                "~/Scripts/amplify.js",
                "~/Scripts/underscore.js",
                "~/Scripts/moment.js",
                "~/Scripts/toastr.js"));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                        "~/Scripts/angular.js",
                        "~/Scripts/angular-locale_en.js",
                        "~/Scripts/angular-strap.js",
                        "~/Scripts/angular-cookies.js",
                        "~/Scripts/ng-grid-{version}.js",
                        "~/Scripts/ng-grid-flexible-height.js",
                        "~/Scripts/ui-bootstrap-tpls-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/ajaxlogin").Include(
                "~/app/ajaxlogin.js"));

            bundles.Add(new ScriptBundle("~/bundles/breeze").Include(                        
                        "~/Scripts/breeze.debug.js",
                        "~/Scripts/breeze.min.js",
                        "~/Scripts/breeze.savequeuing.js",
                        "~/Scripts/datajs-1.1.1beta2.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap",
                                        "//netdna.bootstrapcdn.com/twitter-bootstrap/2.3.2/js/bootstrap.min.js")
                .Include("~/Scripts/bootstrap.js", 
                    "~/Scripts/bootstrap-datepicker.js",
                    "~/Scripts/bootstrap-select.js"));

            bundles.Add(new ScriptBundle("~/bundles/aero").Include(
                "~/app/aero.main.js", // must be first
                "~/app/aero.model.js",
                "~/app/routingConfig.js",
                "~/app/services/aero.datacontext.js",
                "~/app/services/aero.store.js",
                "~/app/services/aero.auth.js",
                "~/app/controllers/aero.controller.js",
                "~/app/controllers/about.controller.js",
                "~/app/controllers/search.controller.js",
                "~/app/controllers/rfq.controller.js",
                "~/app/controllers/my.rfq.controller.js",
                "~/app/directives/fade-in-right.js",
                "~/app/directives/part-summary.js",
                "~/app/directives/datepicker.js",
                "~/app/directives/dropdown.js",
                "~/app/directives/select.js",
                "~/app/about.logger.js"
                ));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/Site.css",
                "~/Content/TodoList.css",
                "~/Content/bootstrap.css",
                "~/Content/bootstrap-responsive.css",
                "~/Content/bootstrap-datepicker.css",
                "~/Content/bootstrap-select.css",
                "~/Content/ng-grid.css",
                "~/Content/fade-in-right.css",
                "~/Content/toastr.css",
                "~/Content/Aero.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));
        }
    }
}