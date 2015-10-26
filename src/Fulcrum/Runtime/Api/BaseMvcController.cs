using System.Text;
using System.Web.Mvc;

namespace Fulcrum.Runtime.Api
{
	public class BaseMvcController : Controller
	{
		protected override JsonResult Json(object data, string contentType,
				Encoding contentEncoding, JsonRequestBehavior behavior)
		{
			return new JsonNetResult
			{
				Data = data,
				ContentType = contentType,
				ContentEncoding = contentEncoding,
				JsonRequestBehavior = JsonRequestBehavior.AllowGet
			};
		}

		protected JsonResult JsonWithoutNulls(object data)
		{
			return new JsonNetResult(false)
			{
				Data = data,
				JsonRequestBehavior = JsonRequestBehavior.AllowGet
			};
		} 

	}
}