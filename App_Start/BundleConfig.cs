using System.Web;
using System.Web.Optimization;

namespace Resume_Portal
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
                      "~/Content/profileCardStyle.css",
                      "~/Content/responsive.css",
                      "~/Content/cssV3.css"));

            bundles.Add(new StyleBundle("~/plugins/css2").Include(
                      "~/plugins/font-awesome-4.7.0/css/font-awesome.min.css",
                      "~/plugins/mCustomScrollbar/jquery.mCustomScrollbar.css"
                      ));

            bundles.Add(new ScriptBundle("~/plugins/plug-ins").Include(
                    //"~/plugins/scrollmagic/ScrollMagic.min.js",
                    //"~/plugins/greensock/animation.gsap.min.js",
                    //"~/plugins/greensock/ScrollToPlugin.min.js",
                    "~/plugins/mCustomScrollbar/jquery.mCustomScrollbar.js"
/*                    "~/plugins/parallax-js-master/parallax.min.js"*/));


        }
    }
}
