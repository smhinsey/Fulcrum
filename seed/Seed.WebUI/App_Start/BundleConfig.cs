using System.Web.Optimization;

namespace Seed.WebUI
{
	public class BundleConfig
	{
		// For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/bundles/libraries")
				.IncludeDirectory("~/scripts", "*.js", true)
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
