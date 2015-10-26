using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Fulcrum.Runtime.Api
{
	public class JsonNetResult : JsonResult
	{
		public JsonNetResult(bool includeNulls)
		{
			var nullRule = includeNulls ? NullValueHandling.Include : NullValueHandling.Ignore;

			Settings = new JsonSerializerSettings
			{
				ReferenceLoopHandling = ReferenceLoopHandling.Error,
				NullValueHandling = nullRule,
				Formatting = Formatting.None,
				ContractResolver = new CamelCasePropertyNamesContractResolver(),
				Converters = new List<JsonConverter>
					{
						new StringEnumConverter()
					}
			};
		}

		public JsonNetResult()
		{
			Settings = new JsonSerializerSettings
			{
				ReferenceLoopHandling = ReferenceLoopHandling.Error,
				Formatting = Formatting.None,
				ContractResolver = new CamelCasePropertyNamesContractResolver(),
				Converters = new List<JsonConverter>
					{
						new StringEnumConverter()
					}
			};
		}

		public JsonSerializerSettings Settings { get; private set; }

		public override void ExecuteResult(ControllerContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			if (JsonRequestBehavior == JsonRequestBehavior.DenyGet &&
			    string.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
			{
				throw new InvalidOperationException("JSON GET is not allowed");
			}

			var response = context.HttpContext.Response;
			response.ContentType = string.IsNullOrEmpty(ContentType) ? "application/json" : ContentType;

			if (ContentEncoding != null)
			{
				response.ContentEncoding = ContentEncoding;
			}
			if (Data == null)
			{
				return;
			}

			var scriptSerializer = JsonSerializer.Create(Settings);

			using (var sw = new StringWriter())
			{
				scriptSerializer.Serialize(sw, Data);
				response.Write(sw.ToString());
			}
		}
	}
}
