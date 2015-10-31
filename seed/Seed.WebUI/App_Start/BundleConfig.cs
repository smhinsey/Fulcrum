using System.Web.Optimization;

namespace Seed.WebUI
{
	public class BundleConfig
	{
		// For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/bundles/libraries")
				.Include("~/Scripts/molodashment.js")
				.Include("~/Scripts/angular.js")
				.Include("~/Scripts/angular-cookies.js")
				.Include("~/Scripts/angular-cache-2.4.1.js")
				.Include("~/Scripts/angular-sanitize.js")
				.Include("~/Scripts/angular-file-upload.js")
				.Include("~/Scripts/angular-ui-router.js")
				.Include("~/Scripts/angular-ui/ui-bootstrap.js")
				.Include("~/Scripts/angular-ui/ui-bootstrap-tpls.js")
				.Include("~/Scripts/angular-ui/ui-utils.js")
				.Include("~/Scripts/angular-local-storage/dist/angular-local-storage.js")
				);

			bundles.Add(new ScriptBundle("~/bundles/app")
				.IncludeDirectory("~/app", "*.js", true)
				);

			bundles.Add(new StyleBundle("~/bundles/css")
				.IncludeDirectory("~/content", "*.css", true)
				);

			// don't set this so that its value is determined at runtime
			// from web.config's debug attribute
			BundleTable.EnableOptimizations = false;
		}
	}
}
