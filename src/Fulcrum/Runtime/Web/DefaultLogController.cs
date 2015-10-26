using System;
using System.Web.Mvc;
using Fulcrum.Common;
using Fulcrum.Runtime.Api;

namespace Fulcrum.Runtime.Web
{

	/// <summary>
	/// Provides an HTTP API to write to the system-wide logs.
	/// 
	/// To use, create a concrete implementation of this abstract controller
	/// in your web project's Controllers directory, override the virtual methods,
	/// and define your own routes on them as attributes. If you prefer, you can
	/// use System.Web.Routing.
	/// </summary>
	public abstract class DefaultLogController : BaseMvcController, ILoggingSource
	{
		public virtual ActionResult WriteDebug(string message)
		{
			try
			{
				// we probably want to use Logmanager directly in order to 
				// set a meaningful source. all of these messages will appear
				// to have originated from InfoLogController. the source should
				// be passed as an argument to the action or extracted from
				// the Request object
				this.LogDebug(message);

				return Json("OK");
			}
			catch (Exception e)
			{
				return Content("An error occurred.");
			}
		}

		public virtual ActionResult WriteFatal(string message)
		{
			try
			{
				this.LogFatal(message);

				return Json("OK");
			}
			catch (Exception e)
			{
				return Content("An error occurred.");
			}
		}

		public virtual ActionResult WriteInfo(string message)
		{
			try
			{
				this.LogInfo(message);

				return Json("OK");
			}
			catch (Exception e)
			{
				return Content("An error occurred.");
			}
		}

		public virtual ActionResult WriteWarn(string message)
		{
			try
			{
				this.LogWarn(message);

				return Json("OK");
			}
			catch (Exception e)
			{
				return Content("An error occurred.");
			}
		}
	}
}
