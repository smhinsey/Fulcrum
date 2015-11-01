using System.Web.Mvc;

namespace FulcrumSeed.WebUI.Controllers
{
	public class ShellController : Controller
	{
		private readonly SeedSettings _settings;

		public ShellController(SeedSettings settings)
		{
			_settings = settings;
		}

		public ActionResult Shell()
		{
			ViewBag.ApiBasePath = _settings.ApiBasePath;
			ViewBag.AppRootNamespace = _settings.AppRootNamespace;
			ViewBag.ReleaseLabel = _settings.ReleaseLabel;
			ViewBag.Version = _settings.Version;

			return View();
		}
	}
}
