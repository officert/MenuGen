using System.Web.Optimization;

namespace MenuGen.SampleApp.App_Start
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/scripts")
                .Include("~/Scripts/libs/jquery-1.10.2.js")
                .Include("~/Scripts/libs/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css")
                .Include("~/Content/normalize.css")
                .Include("~/Content/site.css"));
        }
    }
}