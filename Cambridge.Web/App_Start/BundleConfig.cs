using System.Web.Optimization;

namespace Cambridge.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/core").Include(
                "~/Scripts/app.js",
                "~/Scripts/directives.js",
                "~/Scripts/controllers.js",
                "~/Scripts/services.js"));

            bundles.Add(new StyleBundle("~/bundles/styles").Include(
                "~/Content/normalize.css",
                "~/Content/media-queries.css",
                "~/Content/site.css"));
        }
    }
}
