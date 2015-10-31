using System.Web.Mvc;
using Fulcrum.Runtime;
using SeedComponents;

namespace Seed.WebUI.Controllers
{
	public class ShellController : Controller
	{
		private readonly UserSystemSettings _settings;

		public ShellController(UserSystemSettings settings)
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
