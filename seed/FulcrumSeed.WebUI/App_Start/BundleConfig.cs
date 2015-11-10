using System.Web.Optimization;

namespace FulcrumSeed.WebUI
{
	public class BundleConfig
	{
		// For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/bundles/libraries")
				.Include("~/Scripts/lodash.js")
				.Include("~/Scripts/jquery-{version}.js")
				.Include("~/Scripts/angular.js")
				.Include("~/Scripts/select.js")
				.Include("~/Scripts/angular-http-auth/http-auth-interceptor.js")
				.Include("~/Scripts/angular-ui.js")
				.Include("~/Scripts/ui-grid.js")
				.Include("~/Scripts/loading-bar.js")
				.Include("~/Scripts/angular-cookies.js")
				.Include("~/Scripts/angular-sanitize.js")
				.Include("~/Scripts/angular-translate.js")
				.Include("~/Scripts/angular-file-upload.js")
				.Include("~/Scripts/angular-ui-router.js")
				.Include("~/Scripts/angular-ui/ui-bootstrap.js")
				.Include("~/Scripts/angular-ui/ui-bootstrap-tpls.js")
				.Include("~/Scripts/angular-ui/ui-utils.js")
				.Include("~/Scripts/angular-local-storage/dist/angular-local-storage.js")
				.Include("~/Scripts/tv4-1.2.7/tv4.js")
				.Include("~/Scripts/objectpath-1.2.1/lib/ObjectPath.js")
				.Include("~/Scripts/angular-schema-form-0.8.12/dist/schema-form.js")
				.Include("~/Scripts/angular-schema-form-0.8.12/dist/bootstrap-decorator.js")
				.Include("~/Scripts/angular-schema-form-dynamic-select-0.12.4/angular-schema-form-dynamic-select.js")
				.Include("~/Scripts/ui-grid.js")
				);

			bundles.Add(new ScriptBundle("~/bundles/app")
				.Include("~/app/app.js")
				.IncludeDirectory("~/app/global", "*.js", true)
				.IncludeDirectory("~/app/screens", "*.js", true)
				);

			bundles.Add(new StyleBundle("~/bundles/css")
				.IncludeDirectory("~/content", "*.css", true)
				);

			// don't set this so that its value is determined at runtime
			// from web.config's debug attribute
			//BundleTable.EnableOptimizations = false;
		}
	}
}
