using System;
using System.Web.Mvc;
using Elmah;
using Fulcrum.Runtime.Api;

namespace Fulcrum.Runtime.Web
{
	/// <summary>
	///   Provides an HTTP API to write to ELMAH's error handling system.
	///   To use, create a concrete implementation of this abstract controller
	///   in your web project's Controllers directory, override the virtual methods,
	///   and define your own routes on them as attributes. If you prefer, you can
	///   use System.Web.Routing.
	/// </summary>
	public abstract class DefaultErrorLogController : BaseMvcController
	{
		public virtual ActionResult Capture(string errorMessage)
		{
			try
			{
				ErrorSignal.FromCurrentContext().Raise(new ClientSideException(errorMessage));

				return Json("OK");
			}
			catch (Exception e)
			{
				return Content("An error occurred.");
			}
		}
	}
}
